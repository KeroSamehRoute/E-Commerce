using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.Baskets;

namespace E_Commerce.Application.Contracts;

public interface IBasketService
{
    Task<Result<BasketDto>> GetBasketAsync(string id, CancellationToken cancellationToken = default);

    Task<Result<BasketDto>> CreateOrUpdateBasketAsync(BasketDto basket, CancellationToken cancellationToken = default);

    Task<Result<bool>> DeleteBasketAsync(string id, CancellationToken cancellationToken = default);
}
