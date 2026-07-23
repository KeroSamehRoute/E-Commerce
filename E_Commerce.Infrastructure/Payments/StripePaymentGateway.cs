using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.Services;
using Microsoft.Extensions.Options;
using Stripe;

namespace E_Commerce.Infrastructure.Payments;

internal class StripePaymentGateway : IPaymentGateway
{
    private readonly PaymentIntentService _paymentIntentService = new();

    public StripePaymentGateway(IOptions<PaymentGatewaySettings> options)
    {
        StripeConfiguration.ApiKey = options.Value.SecretKey;
    }


    public async Task<PaymentIntentResult> CreatePaymentIntentAsync(decimal amount, string currency, CancellationToken cancellationToken = default)
    {
        var options = new PaymentIntentCreateOptions
        {
            Amount = (long)amount,
            Currency = currency.ToLowerInvariant(),
            PaymentMethodTypes = ["card"]
        };

        var intent = await _paymentIntentService.CreateAsync(options, cancellationToken: cancellationToken);

        return new PaymentIntentResult(intent.Id, intent.ClientSecret);
    }


    public async Task<PaymentIntentResult> UpdatePaymentIntentAsync(string paymentIntentId, decimal amount, CancellationToken cancellationToken = default)
    {
        var options = new PaymentIntentUpdateOptions { Amount = (long)amount };

        var intent = await _paymentIntentService.UpdateAsync(paymentIntentId, options, cancellationToken: cancellationToken);

        return new PaymentIntentResult(intent.Id, intent.ClientSecret);
    }

}
