namespace AutSoft.Common.Enumeration;

/// <summary>
/// Enumeration item data.
/// </summary>
public class EnumerationObjectItem
{
    /// <summary>
    /// Default constructor of the EnumerationItem.
    /// </summary>
    public EnumerationObjectItem()
    {
    }

    /// <summary>
    /// Constructor of the EnumerationItem.
    /// </summary>
    /// <param name="key">Key of the enum item.</param>
    /// <param name="value">Value of the enum item.</param>
    public EnumerationObjectItem(int key, string value)
    {
        Id = key;
        DisplayName = value;
    }

    /// <summary>
    /// Enumeration item id.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Enumeration diplay name.
    /// </summary>
    public string DisplayName { get; set; } = null!;
}
