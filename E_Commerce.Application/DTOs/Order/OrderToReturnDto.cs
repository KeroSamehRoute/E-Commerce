using E_Commerce.Application.DTOs.Authentications;

namespace E_Commerce.Application.DTOs.Order;

public class OrderToReturnDto
{
    public Guid Id { get; set; }

    public string BuyerEmail { get; set; } = default!;

    public DateTimeOffset OrderDate { get; set; }

    public ICollection<OrderItemDto> Items { get; set; } = [];

    public AddressDto ShipToAddress { get; set; } = default!;

    public string DeliveryMethod { get; set; } = default!;

    public string Status { get; set; } = default!;

    public decimal SubTotal { get; set; }

    public decimal DeliveryCost { get; set; }

    public decimal Total { get; set; }
}
