using E_Commerce.Domain.Contracts;
using StackExchange.Redis;

namespace E_Commerce.Infrastructure.Repositories;

internal class CacheRepository(IConnectionMultiplexer connection) : ICacheRepository
{
    private readonly IDatabase _database = connection.GetDatabase();


    public async Task<string?> GetAsync(string cacheKey, CancellationToken cancellationToken = default)
    {
        var value = await _database.StringGetAsync(cacheKey);

        return value.IsNullOrEmpty ? null : value.ToString();
    }


    public Task SetAsync(string cacheKey, string cacheValue, TimeSpan timeToLive, CancellationToken cancellationToken = default)
    {
        return _database.StringSetAsync(cacheKey, cacheValue, timeToLive);
    }

}
