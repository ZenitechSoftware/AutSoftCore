namespace AutSoft.Common.Enumeration;

/// <summary>
/// Enumeration type items.
/// </summary>
/// <typeparam name="TEnum">Collection of the enumeration types.</typeparam>
public class EnumerationObject<TEnum> where TEnum : Enum
{
    /// <summary>
    /// Default constructor of the Enumeration.
    /// </summary>
    public EnumerationObject()
    {
        Type = default!;
        Items = new List<EnumerationObjectItem>();
    }

    /// <summary>
    /// Constructor of the Enumeration.
    /// </summary>
    /// <param name="type">Enumeration type.</param>
    /// <param name="items">Items of the enumerations.</param>
    public EnumerationObject(TEnum type, List<EnumerationObjectItem> items)
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
    public List<EnumerationObjectItem> Items { get; set; }
}
