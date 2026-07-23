using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.Baskets;

namespace E_Commerce.Application.Contracts;

public interface IPaymentService
{
    Task<Result<BasketDto>> CreateOrUpdatePaymentIntentAsync(string basketId, CancellationToken cancellationToken = default);

    Task PaymentSucceeded(string paymentIntentId);

    Task PaymentFailed(string paymentIntentId);
}
