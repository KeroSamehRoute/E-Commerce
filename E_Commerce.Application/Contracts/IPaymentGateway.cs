using E_Commerce.Application.Common;

namespace E_Commerce.Application.Contracts;

public interface IPaymentGateway
{
    Task<PaymentIntentResult> CreatePaymentIntentAsync(decimal amount, string currency, CancellationToken cancellationToken = default);

    Task<PaymentIntentResult> UpdatePaymentIntentAsync(string paymentIntentId, decimal amount, CancellationToken cancellationToken = default);
}
