namespace AutSoft.AspNetCore.Blazor.Enumeration;

/// <summary>
/// Enumeration type items.
/// </summary>
/// <typeparam name="TEnum">Collection of the enumeration types.</typeparam>
public class Enumeration<TEnum> where TEnum : Enum
{
    /// <summary>
    /// Default constructor of the Enumeration.
    /// </summary>
    public Enumeration()
    {
        Type = default!;
        Items = new List<EnumerationItem>();
    }

    /// <summary>
    /// Default constructor of the Enumeration.
    /// </summary>
    /// <param name="type">Enumeration type.</param>
    /// <param name="items">Items of the enumerations.</param>
    public Enumeration(TEnum type, List<EnumerationItem> items)
    {
        Type = type;
        Items = items;
    }

    /// <summary>
    /// Enumeration type.
    /// </summary>
    public TEnum Type { get; set; }

    /// <summary>
    /// Items of the enumeration.
    /// </summary>
    public List<EnumerationItem> Items { get; set; }
}
