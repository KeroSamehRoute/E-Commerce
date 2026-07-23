using AutoMapper;
using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Baskets;
using E_Commerce.Application.Specifications;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Orders;
using E_Commerce.Domain.Entities.Products;
using Microsoft.Extensions.Options;

namespace E_Commerce.Application.Services;

internal class PaymentService(
    IBasketRepository basketRepository, IUnitOfWork unitOfWork, IPaymentGateway paymentGateway,
    IOptions<PaymentGatewaySettings> stripeSettings, IMapper mapper): IPaymentService
{
    private readonly IBasketRepository _basketRepository = basketRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IPaymentGateway _paymentGateway = paymentGateway;
    private readonly PaymentGatewaySettings _stripe = stripeSettings.Value;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<BasketDto>> CreateOrUpdatePaymentIntentAsync(string basketId, CancellationToken cancellationToken = default)
    {
        var basket = await _basketRepository.GetBasketAsync(basketId, cancellationToken);

        if (basket == null)
        {
            return Result<BasketDto>.Fail(Error.NotFound("Basket Not Found", $"Basket With Id {basketId} Is Not Found"));
        }

        if (basket.Items.Count == 0)
        {
            return Result<BasketDto>.Fail(Error.Validation("Basket is Empty", $"Can Not Create Order With Basket With Id {basketId}"));
        }

        var productRepo = _unitOfWork.GetRepository<Product, int>();

        var productIds = basket.Items.Select(i => i.Id).ToHashSet();

        var products = (await productRepo.GetAllAsync(new ProductsWithIdsSpecifications(productIds), cancellationToken)).ToDictionary(x => x.Id);

        foreach (var item in basket.Items)
        {
            if (!products.TryGetValue(item.Id, out var product))
            {
                return Result<BasketDto>.Fail(Error.NotFound("Product Not Found", $"Product With Id {item.Id} Is Not Found "));
            }

            item.Price = product.Price;
        }

        if (!basket.DeliveryMethodId.HasValue)
        {
            return Result<BasketDto>.Fail(Error.Validation("Delivery Method Not Selected", $"No delivery method selected for basket {basketId}"));
        }

        var deliveryRepo = _unitOfWork.GetRepository<DeliveryMethod, int>();

        int deliveryMethodId = basket.DeliveryMethodId.Value;

        var deliveryMethod = await deliveryRepo.GetByIdAsync(deliveryMethodId, cancellationToken);

        if (deliveryMethod == null)
        {
            return Result<BasketDto>.Fail(Error.NotFound("Delivery Method Not Found", $"DeliveryMethod With Id {deliveryMethodId} Is Not Found "));
        }

        basket.ShippingPrice = deliveryMethod.Cost;

        var subtotal = basket.Items.Sum(i => i.Quantity * i.Price);

        var amount = (long)Math.Round((subtotal + deliveryMethod.Cost) * 100m);

        if (!string.IsNullOrEmpty(basket.PaymentIntentId))
        {
            await _paymentGateway.UpdatePaymentIntentAsync(basket.PaymentIntentId, amount, cancellationToken);
        }
        else
        {
            var result = await _paymentGateway.CreatePaymentIntentAsync(amount, _stripe.DefaultCurrency, cancellationToken);

            basket.PaymentIntentId = result.PaymentIntentId;

            basket.ClientSecret = result.ClientSecret;
        }

        await _basketRepository.CreateOrUpdateBasketAsync(basket, cancellationToken: cancellationToken);

        return _mapper.Map<BasketDto>(basket);
    }


    public async Task PaymentSucceeded(string paymentIntentId)
    {
        var orderRepo = _unitOfWork.GetRepository<Order, Guid>();

        var order = await orderRepo.GetByIdAsync(new PaymentIntentSpec(paymentIntentId));

        if (order == null)
            return;

        order.Status = OrderStatus.PaymentReceived;

        await _unitOfWork.SaveChangesAsync();
    }


    public async Task PaymentFailed(string paymentIntentId)
    {
        var orderRepo = _unitOfWork.GetRepository<Order, Guid>();

        var order = await orderRepo.GetByIdAsync(new PaymentIntentSpec(paymentIntentId));

        if (order == null)
            return;

        order.Status = OrderStatus.PaymentFailed;

        await _unitOfWork.SaveChangesAsync();
    }

}

public class PaymentGatewaySettings
{
    public string SecretKey { get; set; } = default!;

    public string DefaultCurrency { get; set; } = "USD";

    public string WebhookSecret { get; set; } = default!;
}
