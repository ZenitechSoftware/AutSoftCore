using AutSoft.Linq.Models;
using AutSoft.Linq.Queryable;

using FluentAssertions;

namespace AutSoft.Linq.Tests.Queryable.OrderByExtensions;

public partial class ThenByFluent : OrderByExtensionsTests
{
    [Theory]
    [InlineData(OrderDirection.Ascending)]
    [InlineData(OrderDirection.Descending)]
    public void Should_ReturnOrdered(OrderDirection thenByDirection)
    {
        // Act
        var ordered = Subject.OrderBy(x => x.Name, OrderDirection.Ascending)
            .ThenBy(x => x.DateOfBirth, thenByDirection)
            .ToList();

        // Assert
        ordered.Should().HaveCount(Subject.Count());

        if (thenByDirection is OrderDirection.Ascending)
        {
            var expected = Subject.OrderBy(x => x.Name).ThenBy(x => x.DateOfBirth).ToList();
            ordered.Should().ContainInOrder(expected);
        }
        else if (thenByDirection is OrderDirection.Descending)
        {
            var expected = Subject.OrderBy(x => x.Name).ThenByDescending(x => x.DateOfBirth).ToList();
            ordered.Should().ContainInOrder(expected);
        }
        else
        {
            throw new NotSupportedException();
        }
    }
}
