namespace E_Commerce.Application.Common;

public sealed class PaymentIntentResult(string paymentIntentId, string clientSecret)
{
    public string PaymentIntentId { get; } = paymentIntentId;
    public string ClientSecret { get; } = clientSecret;
}
