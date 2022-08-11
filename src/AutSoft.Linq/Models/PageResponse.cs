namespace AutSoft.Linq.Models;

/// <summary>
/// Wraps a page of the requested objects
/// </summary>
/// <typeparam name="T">Type of requested objects</typeparam>
public class PageResponse<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PageResponse{T}"/> class.
    /// </summary>
    /// <param name="results">List of objects of the current page</param>
    /// <param name="currentPage">Current page's index (start's from 0)</param>
    /// <param name="totalCount">Total object count on all pages</param>
    /// <param name="pageCount">Total page count</param>
    public PageResponse(List<T> results, int currentPage, int totalCount, int pageCount)
    {
        Results = results;
        CurrentPage = currentPage;
        TotalCount = totalCount;
        PageCount = pageCount;
    }

    /// <summary>
    /// Gets or sets objects of the current page
    /// </summary>
    public List<T> Results { get; set; }

    /// <summary>
    /// Gets or sets current page's index (start's from 0)
    /// </summary>
    public int CurrentPage { get; set; }

    /// <summary>
    /// Gets or sets total page count
    /// </summary>
    public int PageCount { get; set; }

    /// <summary>
    /// Gets or sets total object count on all pages
    /// </summary>
    public int TotalCount { get; set; }
}
