using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.Order;

namespace E_Commerce.Application.Contracts;

public interface IOrderService
{
    Task<Result<OrderToReturnDto>> CreateOrderAsync(OrderDto orderDto, string email, CancellationToken cancellationToken = default);

    Task<Result<IReadOnlyList<DeliveryMethodDto>>> GetAllDeliveryMethodsAsync(CancellationToken cancellationToken = default);

    Task<Result<IReadOnlyList<OrderToReturnDto>>> GetAllOrdersAsync(string email, CancellationToken cancellationToken = default);

    Task<Result<OrderToReturnDto>> GetOrderByIdAndEmailAsync(Guid id, string email, CancellationToken cancellationToken = default);
}
