namespace AutSoft.AspNetCore.Blazor.Enumeration;

/// <summary>
/// Enumeration item data.
/// </summary>
public class EnumerationItem
{
    /// <summary>
    /// Default constructor of the EnumerationItem.
    /// </summary>
    public EnumerationItem()
    {
    }

    /// <summary>
    /// Constructor of the EnumerationItem.
    /// </summary>
    /// <param name="key">Key of the enum item.</param>
    /// <param name="value">Value of the enum item.</param>
    public EnumerationItem(int key, string value)
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
