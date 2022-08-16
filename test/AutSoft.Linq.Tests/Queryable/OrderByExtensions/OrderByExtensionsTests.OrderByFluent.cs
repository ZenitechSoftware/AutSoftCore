using AutSoft.Linq.Models;
using AutSoft.Linq.Queryable;

using FluentAssertions;

namespace AutSoft.Linq.Tests.Queryable.OrderByExtensions;

public partial class OrderByFluent : OrderByExtensionsTests
{
    [Theory]
    [InlineData(OrderDirection.Ascending)]
    [InlineData(OrderDirection.Descending)]
    public void Should_ReturnOrdered(OrderDirection orderDirection)
    {
        // Act
        var ordered = Subject.OrderBy(x => x.Name, orderDirection).ToList();

        // Assert
        ordered.Should().HaveCount(Subject.Count());
        _ = orderDirection switch
        {
            OrderDirection.Ascending => ordered.Should().BeInAscendingOrder(p => p.Name),
            OrderDirection.Descending => ordered.Should().BeInDescendingOrder(p => p.Name),
            _ => throw new NotSupportedException(),
        };
    }
}
