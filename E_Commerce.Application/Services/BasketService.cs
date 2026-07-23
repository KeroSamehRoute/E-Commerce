using AutoMapper;
using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Baskets;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Baskets;

namespace E_Commerce.Application.Services;

internal class BasketService(IBasketRepository basketRepository, IMapper mapper) : IBasketService
{
    private readonly IBasketRepository _basketRepository = basketRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<BasketDto>> CreateOrUpdateBasketAsync(BasketDto basket, CancellationToken cancellationToken = default)
    {
        var customerBasket = _mapper.Map<CustomerBasket>(basket);

        var basketResult = await _basketRepository.CreateOrUpdateBasketAsync(customerBasket, cancellationToken: cancellationToken);

        return (basketResult != null) ? 
            Result<BasketDto>.Ok(_mapper.Map<BasketDto>(basketResult)) : Result<BasketDto>.Fail(Error.Failure("BasketDelete.Failure", "Can Not Delete Basket"));
    }

    public async Task<Result<bool>> DeleteBasketAsync(string id, CancellationToken cancellationToken = default)
    {
        var result = await _basketRepository.DeleteBasketAsync(id, cancellationToken);

        return (result) ?
            Result<bool>.Ok(true) : Result<bool>.Fail(Error.Failure("BasketDelete.Failure", "Can Not Delete Basket"));
    }

    public async Task<Result<BasketDto>> GetBasketAsync(string id, CancellationToken cancellationToken = default)
    {
        var basket = await _basketRepository.GetBasketAsync(id, cancellationToken);

        if (basket == null)
        {
            return Result<BasketDto>.Fail(Error.NotFound("Basket Not Found"));
        }

        return _mapper.Map<BasketDto>(basket);
    }

}
