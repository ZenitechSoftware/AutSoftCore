using AutSoft.Linq.Queryable;

namespace AutSoft.Linq.Tests.Queryable;

public class PersonDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int Age { get; set; }
    [NotSortable]
    public string? Address { get; set; }
}
