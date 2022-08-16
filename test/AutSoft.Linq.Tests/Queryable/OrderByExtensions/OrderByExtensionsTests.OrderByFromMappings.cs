using AutoMapper.QueryableExtensions;

using AutSoft.Common.Exceptions;
using AutSoft.Linq.Models;
using AutSoft.Linq.Queryable;

using FluentAssertions;

using System.Linq.Expressions;

namespace AutSoft.Linq.Tests.Queryable.OrderByExtensions;

public partial class OrderByExtensionsTests
{
    public class OrderByFromMappings : OrderByExtensionsTests
    {
        public static List<(string orderColumn, Expression<Func<PersonDto, object?>> expectedOrderExpression)> OrderExpression() => new()
        {
            (nameof(PersonDto.Id), p => p.Id),
            (nameof(PersonDto.Name), p => p.Name),
            (nameof(PersonDto.Age), p => p.Age),
        };

        [Theory]
        [CombinatorialData]
        public void Should_ReturnOrdered(
            [CombinatorialMemberData(nameof(OrderExpression))] (string orderColumn, Expression<Func<PersonDto, object?>> expectedOrderExpression) ordering,
            OrderDirection orderDirection)
        {
            // Act
            var ordered = Subject
                .OrderBy<Person, PersonDto>(
                    new PageRequest { OrderBy = ordering.orderColumn, OrderDirection = orderDirection },
                    p => p.Id,
                    Mapper.ConfigurationProvider)
                .ProjectTo<PersonDto>(Mapper.ConfigurationProvider)
                .ToList();

            // Assert
            ordered.Should().HaveCount(Subject.Count()).And.HaveCountGreaterThan(0);
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

        public static List<(Expression<Func<Person, object?>> defaultOrderExpression, Expression<Func<PersonDto, object?>> expectedOrderExpression)> DefaultOrderExpression() => new()
        {
            (p => p.Id, p => p.Id),
            (p => p.Name, p => p.Name),
            (p => p.Address, p => p.Address),
        };

        [Theory]
        [CombinatorialData]
        public void Should_ReturnDefaultOrdered_WithoutOrderingInfo(
            [CombinatorialMemberData(nameof(DefaultOrderExpression))]
            (Expression<Func<Person, object?>> defaultOrderExpression, Expression<Func<PersonDto, object?>> expectedOrderExpression) expressions,
            OrderDirection orderDirection)
        {
            // Act
            var ordered = Subject
                .OrderBy<Person, PersonDto>(
                    new PageRequest() { OrderDirection = orderDirection },
                    expressions.defaultOrderExpression,
                    Mapper.ConfigurationProvider)
                .ProjectTo<PersonDto>(Mapper.ConfigurationProvider)
                .ToList();

            // Assert
            ordered.Should().HaveCount(Subject.Count()).And.HaveCountGreaterThan(0);
            if (orderDirection is OrderDirection.Ascending)
            {
                ordered.Should().BeInAscendingOrder(expressions.expectedOrderExpression);
            }
            else if (orderDirection is OrderDirection.Descending)
            {
                ordered.Should().BeInDescendingOrder(expressions.expectedOrderExpression);
            }
            else
            {
                throw new NotSupportedException("Not valid test case input");
            }
        }

        [Theory]
        [CombinatorialData]
        public void Should_ReturnDefaultOrdered_WithDefaultOrderDirection(
            [CombinatorialMemberData(nameof(DefaultOrderExpression))]
            (Expression<Func<Person, object?>> defaultOrderExpression, Expression<Func<PersonDto, object?>> expectedOrderExpression) expressions,
            OrderDirection defaultOrderDirection)
        {
            // Act
            var ordered = Subject
                .OrderBy<Person, PersonDto>(new PageRequest(), expressions.defaultOrderExpression, defaultOrderDirection, Mapper.ConfigurationProvider)
                .ProjectTo<PersonDto>(Mapper.ConfigurationProvider)
                .ToList();

            // Assert
            ordered.Should().HaveCount(Subject.Count()).And.HaveCountGreaterThan(0);
            if (defaultOrderDirection is OrderDirection.Ascending)
            {
                ordered.Should().BeInAscendingOrder(expressions.expectedOrderExpression);
            }
            else if (defaultOrderDirection is OrderDirection.Descending)
            {
                ordered.Should().BeInDescendingOrder(expressions.expectedOrderExpression);
            }
            else
            {
                throw new NotSupportedException("Not valid test case input");
            }
        }

        [Fact]
        public void Should_ThrowOnNotSortableColumn()
        {
            // Act
            var func = () => Subject
                .OrderBy<Person, PersonDto>(new PageRequest { OrderBy = nameof(PersonDto.Address) }, p => p.Id, Mapper.ConfigurationProvider);

            // Assert
            func.Should().Throw<ValidationException>();
        }

        [Fact]
        public void Should_ThrowOnNotExistedColumn()
        {
            // Act
            var func = () => Subject
                .OrderBy<Person, PersonDto>(new PageRequest { OrderBy = "TEST" }, p => p.Id, Mapper.ConfigurationProvider);

            // Assert
            func.Should().Throw<ValidationException>();
        }
    }
}
