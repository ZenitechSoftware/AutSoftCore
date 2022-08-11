namespace AutSoft.Linq.Models;

public class PageRequest
{
    public int PageSize { get; set; } = 50;
    public int Page { get; set; }
    public string? OrderBy { get; set; }
    public OrderDirection OrderDirection { get; set; } = OrderDirection.Ascending;
}
