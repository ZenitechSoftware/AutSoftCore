namespace AutSoft.Linq.Models;

/// <summary>
/// Common query parameters for a paged request (mostly for grids and lists)
/// </summary>
public class PageRequest
{
    /// <summary>
    /// Gets or sets requested page's size, default is 50
    /// </summary>
    public int PageSize { get; set; } = 50;

    /// <summary>
    /// Gets or sets requested page, 0 represents first page
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Gets or sets order by based on this property's name
    /// </summary>
    public string? OrderBy { get; set; }

    /// <summary>
    /// Gets or sets order direction, default is <see cref="OrderDirection.Ascending"/>
    /// </summary>
    public OrderDirection OrderDirection { get; set; } = OrderDirection.Ascending;
}
