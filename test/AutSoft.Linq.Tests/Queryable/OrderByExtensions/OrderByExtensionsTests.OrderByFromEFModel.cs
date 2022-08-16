using AutSoft.Linq.Models;
using AutSoft.Linq.Queryable;

using FluentAssertions;

using System.Linq.Expressions;

namespace AutSoft.Linq.Tests.Queryable.OrderByExtensions;

public partial class OrderByFromEFModel : OrderByExtensionsTests
{
    public static List<(string orderColumn, Expression<Func<Person, object?>> expectedOrderExpression)> DefaultOrderExpression() => new()
    {
        (nameof(Person.Id), p => p.Id),
        (nameof(Person.Name), p => p.Name),
        (nameof(Person.Address), p => p.Address),
        (nameof(Person.DateOfBirth), p => p.DateOfBirth),
    };

    [Theory]
    [CombinatorialData]
    public void Should_ReturnOrdered(
        [CombinatorialMemberData(nameof(DefaultOrderExpression))] (string orderColumn, Expression<Func<Person, object?>> expectedOrderExpression) ordering,
        OrderDirection orderDirection)
    {
        // Act
        var ordered = Subject
            .OrderBy(new PageRequest { OrderBy = ordering.orderColumn, OrderDirection = orderDirection }, p => p.Id)
            .ToList();

        // Assert
        ordered.Should().HaveCount(Subject.Count());
        if (orderDirection is OrderDirection.Ascending)
        {
            ordered.Should().BeInAscendingOrder(ordering.expectedOrderExpression);
        }
        else if (orderDirection is OrderDirection.Descending)
        {
            ordered.Should().BeInDescendingOrder(ordering.expectedOrderExpression);
        }
        else
        {
            throw new NotSupportedException("Not valid test case input");
        }
    }
}
