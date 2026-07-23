using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Baskets;
using StackExchange.Redis;
using System.Text.Json;

namespace E_Commerce.Infrastructure.Repositories;

internal class BasketRepository(IConnectionMultiplexer connection) : IBasketRepository
{
    private readonly IDatabase _database = connection.GetDatabase();


    public async Task<CustomerBasket?> CreateOrUpdateBasketAsync(CustomerBasket basket, TimeSpan? timeToLive = null, CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(basket);

        var success = await _database.StringSetAsync(basket.Id, json, timeToLive ?? TimeSpan.FromDays(30));

        return success ? basket : null;
    }


    public async Task<bool> DeleteBasketAsync(string basketId, CancellationToken cancellationToken = default)
    {
        return await _database.KeyDeleteAsync(basketId);
    }


    public async Task<CustomerBasket?> GetBasketAsync(string basketId, CancellationToken ct = default)
    {
        var basket = await _database.StringGetAsync(basketId);

        return (basket.IsNullOrEmpty) ? null : JsonSerializer.Deserialize<CustomerBasket>(basket!);
    }

}
