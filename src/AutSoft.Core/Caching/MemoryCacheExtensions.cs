using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace AutSoft.Common.Caching;

/// <summary>
/// Extension functions for <see cref="IMemoryCache"/>
/// </summary>
public static class MemoryCacheExtensions
{
    private const string Postfix = "CancellationTokenSource";

    /// <summary>
    /// Get or create a cached element with error handling
    /// </summary>
    /// <typeparam name="TItem">The type of the cached element</typeparam>
    /// <param name="cache">An <see cref="IMemoryCache"/> object</param>
    /// <param name="key">The key of the cached element</param>
    /// <param name="factory">The factory method, which can create the element if it isn't cached</param>
    /// <param name="options">Options of the caching</param>
    /// <returns>The finded or created element</returns>
    public static async Task<TItem> GetOrCreateWithErrorHandlingAsync<TItem>(
        this IMemoryCache cache,
        string key,
        Func<ICacheEntry, Task<TItem>> factory,
        MemoryCacheEntryOptions? options = null)
    {
        try
        {
            return await cache.GetOrCreateAsync(key, entry =>
            {
                entry.SetOptions(options);
                entry.AddExpirationToken(cache.CreateToken(key));
                return factory(entry);
            });
        }
        catch
        {
            cache.Remove(key);
            throw;
        }
    }

    /// <summary>
    /// Create or override an element in the cache
    /// </summary>
    /// <typeparam name="TItem">The type of the cached element</typeparam>
    /// <param name="cache">An <see cref="IMemoryCache"/> object</param>
    /// <param name="key">The key of the cached element</param>
    /// <param name="value">The cached value</param>
    /// <param name="size">The size of the cached value</param>
    /// <param name="options">Options of the caching</param>
    public static void SetWithErrorHandling<TItem>(
        this IMemoryCache cache,
        string key,
        TItem value,
        int size,
        MemoryCacheEntryOptions? options = null)
    {
        try
        {
            using var entry = cache.CreateEntry(key);
            entry.Size = size;
            entry.SetOptions(options);
            entry.AddExpirationToken(cache.CreateToken(key));
            entry.Value = value;
        }
        catch
        {
            cache.Remove(key);
            throw;
        }
    }

    private static void SetOptions(this ICacheEntry entry, MemoryCacheEntryOptions? options = null)
    {
        if (options == null)
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
            entry.SlidingExpiration = TimeSpan.FromMinutes(10);
            entry.Priority = CacheItemPriority.Normal;
        }
        else
        {
            entry.AbsoluteExpirationRelativeToNow = options.AbsoluteExpirationRelativeToNow;
            entry.SlidingExpiration = options.SlidingExpiration;
            entry.Priority = options.Priority;
        }
    }

    private static IChangeToken CreateToken(this IMemoryCache cache, string key)
    {
        var tokenSourceKey = key + Postfix;

        var source = cache.Set(
            tokenSourceKey,
            new CancellationTokenSource(),
            new MemoryCacheEntryOptions { Size = 1 });

        return new CancellationChangeToken(source.Token);
    }

    /// <summary>
    /// Invalidate a cached element's value
    /// </summary>
    /// <param name="cache">An <see cref="IMemoryCache"/> object</param>
    /// <param name="key">The key of the cached element</param>
    /// <returns>The element invalidated successfully or not</returns>
    public static bool Invalidate(this IMemoryCache cache, string key)
    {
        var tokenSourceKey = key + Postfix;

        if (!cache.TryGetValue(tokenSourceKey, out CancellationTokenSource source))
            return false;

        source.Cancel();
        cache.Remove(tokenSourceKey);

        return true;
    }
}
