using AutSoft.Common.Concurrency;
using AutSoft.Common.Enumeration;

using Microsoft.Extensions.Logging;

namespace AutSoft.AspNetCore.Blazor.Enumeration;

/// <inheritdoc />
public class EnumerationCache<TEnum> : IEnumerationCache<TEnum>, IDisposable where TEnum : Enum
{
    private readonly Dictionary<TEnum, List<EnumerationObjectItem>> _enumerations = new();

    private readonly AsyncLock _downloadLock = new();

    private readonly IEnumerationService<TEnum> _enumerationClient;
    private readonly ILogger<EnumerationCache<TEnum>> _logger;

    /// <summary>
    /// Constructor of the EnumerationCache.
    /// </summary>
    /// <param name="enumerationService">Enumeration service.</param>
    /// <param name="logger">Logger of the enumeration cache.</param>
    public EnumerationCache(IEnumerationService<TEnum> enumerationService, ILogger<EnumerationCache<TEnum>> logger)
    {
        _enumerationClient = enumerationService;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task EnsureHasCachedEnumerationAsync(params TEnum[] enumerationTypes)
    {
        using (await AsyncLockContext.CreateAsync(_downloadLock))
        {
            var enumerationTypesToDownload = enumerationTypes.Where(t => !_enumerations.Any(m => m.Key.Equals(t))).ToList();
            if (enumerationTypesToDownload.Count == 0)
                return;

            var result = await _enumerationClient.ListEnumerationsAsync(enumerationTypesToDownload);

            foreach (var enumerationItem in result)
            {
                if (!_enumerations.ContainsKey(enumerationItem.Type))
                    _enumerations.Add(enumerationItem.Type, enumerationItem.Items);
            }
        }
    }

    private async Task TryEnsureHasCachedEnumerationAsync(params TEnum[] enumerationTypes)
    {
        try
        {
            await EnsureHasCachedEnumerationAsync(enumerationTypes);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to load enumerations.");
        }
    }

    /// <inheritdoc />
    public async Task<string?> ResolveDisplayNameAsync(TEnum type, int id)
    {
        await TryEnsureHasCachedEnumerationAsync(type);

        if (!_enumerations.ContainsKey(type))
            return null;

        return _enumerations[type].SingleOrDefault(i => i.Id == id)?.DisplayName;
    }

    /// <inheritdoc />
    public async Task<List<EnumerationObjectItem>> ResolveEnumerationItemsAsync(TEnum type)
    {
        await TryEnsureHasCachedEnumerationAsync(type);

        if (!_enumerations.ContainsKey(type))
            return new List<EnumerationObjectItem>();

        return _enumerations[type];
    }

    /// <inheritdoc />
    public void Dispose()
    {
        _downloadLock.Dispose();
        GC.SuppressFinalize(this);
    }
}
