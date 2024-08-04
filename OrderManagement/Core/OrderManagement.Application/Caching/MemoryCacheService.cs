using Microsoft.Extensions.Caching.Memory;

namespace OrderManagement.Application.Caching;

public class MemoryCacheService : ICachingService
{
    private readonly IMemoryCache _memoryCache;

    public MemoryCacheService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    public async Task<T> GetAsync<T>(string key)
    {
        return await Task.FromResult(_memoryCache.TryGetValue(key, out T value) ? value : default);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan expiration)
    {
        var cacheEntryOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = expiration
        };

        await Task.Run(() => _memoryCache.Set(key, value, cacheEntryOptions));
    }

    public async Task RemoveAsync(string key)
    {
        await Task.Run(() => _memoryCache.Remove(key));
    }
}
