namespace E_Commerce.Domain.Contracts;

public interface ICacheRepository
{
    Task<string?> GetAsync(string cacheKey, CancellationToken cancellationToken = default);

    Task SetAsync(string cacheKey, string cacheValue, TimeSpan timeToLive, CancellationToken cancellationToken = default);
}
