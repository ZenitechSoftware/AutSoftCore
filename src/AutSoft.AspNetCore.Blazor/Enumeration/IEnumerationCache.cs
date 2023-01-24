using AutSoft.Common.Enumeration;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutSoft.AspNetCore.Blazor.Enumeration;

/// <summary>
/// Cache for enumeration types.
/// </summary>
/// <typeparam name="TEnum">Collection of the enumeration types.</typeparam>
public interface IEnumerationCache<TEnum> where TEnum : Enum
{
    /// <summary>
    /// Resolves the display name of the specified enumeration type with id.
    /// </summary>
    /// <param name="type">Enumeration type.</param>
    /// <param name="id">Id of the enumeration type.</param>
    /// <returns>Display name.</returns>
    Task<string?> ResolveDisplayNameAsync(TEnum type, int id);

    /// <summary>
    /// Resolves enumeration items of the specified enumeration type.
    /// </summary>
    /// <param name="type">Enumeration type.</param>
    /// <returns>Enumeration items.</returns>
    Task<List<EnumerationObjectItem>> ResolveEnumerationItemsAsync(TEnum type);

    /// <summary>
    /// Caches the specified enumeration types if they are not in the cache.
    /// </summary>
    /// <param name="enumerationTypes">Enumeration types.</param>
    Task EnsureHasCachedEnumerationAsync(params TEnum[] enumerationTypes);
}
