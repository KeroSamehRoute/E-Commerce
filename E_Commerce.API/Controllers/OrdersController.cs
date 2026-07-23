using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers;

public class OrdersController(IOrderService orderService) : ApiBaseController
{
    private readonly IOrderService _orderService = orderService;

    [Authorize]
    [HttpPost]
    [ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
    public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto, CancellationToken cancellationToken)
    {
        return ToActionResult(await _orderService.CreateOrderAsync(orderDto, GetEmailFromToken(), cancellationToken));
    }


    [AllowAnonymous]
    [HttpGet("deliveryMethods")]
    public async Task<ActionResult<IReadOnlyList<DeliveryMethodDto>>> GetDeliveryMethods(CancellationToken cancellationToken)
    {
        return ToActionResult(await _orderService.GetAllDeliveryMethodsAsync(cancellationToken));
    }


    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetAllOrders(CancellationToken cancellationToken)
    {
        return ToActionResult(await _orderService.GetAllOrdersAsync(GetEmailFromToken(), cancellationToken));
    }


    [Authorize]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<OrderToReturnDto>> GetOrderById(Guid id, CancellationToken cancellationToken)
    {
        return ToActionResult(await _orderService.GetOrderByIdAndEmailAsync(id, GetEmailFromToken(), cancellationToken));
    }

}
