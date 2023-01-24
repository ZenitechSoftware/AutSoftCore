using AutSoft.Common.Enumeration;

namespace AutSoft.AspNetCore.Blazor.Enumeration;

/// <summary>
/// Enumeration service that is used by the cache to get the values of enumeration types.
/// </summary>
/// <typeparam name="TEnum">Collection of the enumeration types.</typeparam>
public interface IEnumerationService<TEnum> where TEnum : Enum
{
    /// <summary>
    /// Lists the specified enumeration types.
    /// </summary>
    /// <param name="enumerationTypes">Enumeration types.</param>
    /// <returns>Values of the enumeration types.</returns>
    Task<List<EnumerationObject<TEnum>>> ListEnumerationsAsync(List<TEnum> enumerationTypes);
}
