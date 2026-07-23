namespace E_Commerce.Application.DTOs.Baskets;

public class BasketDto
{
    public string Id { get; set; } = default!;

    public ICollection<BasketItemDto> Items { get; set; } = [];

    public string? PaymentIntentId { get; set; }

    public int? DeliveryMethodId { get; set; }

    public decimal? ShippingPrice { get; set; }

    public string? ClientSecret { get; set; }
}
