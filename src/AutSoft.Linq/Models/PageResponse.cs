namespace AutSoft.Linq.Models;

public class PageResponse<T>
{
    public PageResponse(List<T> results, int currentPage, int totalCount, int pageCount)
    {
        Results = results;
        CurrentPage = currentPage;
        TotalCount = totalCount;
        PageCount = pageCount;
    }

    public List<T> Results { get; set; }

    public int CurrentPage { get; set; }
    public int PageCount { get; set; }
    public int TotalCount { get; set; }
}
