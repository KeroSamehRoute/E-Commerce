using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Baskets;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers;

public class BasketsController(IBasketService basketService) : ApiBaseController
{
    private readonly IBasketService _basketService = basketService;


    [HttpGet("{id}")]
    [ProducesResponseType(typeof(BasketDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BasketDto>> GetBasket(string id, CancellationToken cancellationToken)
    {
        var basket = await _basketService.GetBasketAsync(id, cancellationToken);
        return ToActionResult(basket);
    }


    [HttpPost]
    public async Task<ActionResult<BasketDto>> CreateOrUpdateBasket(BasketDto basket, CancellationToken cancellationToken)
    {
        var saved = await _basketService.CreateOrUpdateBasketAsync(basket, cancellationToken);
        return ToActionResult(saved);
    }


    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteBasket(string id, CancellationToken cancellationToken)
    {
        var result = await _basketService.DeleteBasketAsync(id, cancellationToken);
        return ToActionResult(result);
    }

}
