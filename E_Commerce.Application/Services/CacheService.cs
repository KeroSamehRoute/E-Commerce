using E_Commerce.Application.Contracts;
using E_Commerce.Domain.Contracts;
using System.Text.Json;

namespace E_Commerce.Application.Services;

public class CacheService(ICacheRepository cacheRepository) : ICacheService
{
    private readonly ICacheRepository _cacheRepository = cacheRepository;

    private static readonly JsonSerializerOptions s_jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public Task<string?> GetAsync(string cacheKey, CancellationToken cancellationToken = default)
    {
        return _cacheRepository.GetAsync(cacheKey, cancellationToken);
    }

    public Task SetAsync(string cacheKey, object cacheValue, TimeSpan timeToLive, CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(cacheValue, s_jsonOptions);

        return _cacheRepository.SetAsync(cacheKey, json, timeToLive, cancellationToken);
    }

}
