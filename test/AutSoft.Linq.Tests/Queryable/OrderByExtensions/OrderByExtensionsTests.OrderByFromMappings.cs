using AutoMapper.QueryableExtensions;

using AutSoft.Linq.Models;
using AutSoft.Linq.Queryable;

using FluentAssertions;

using System.Linq.Expressions;

namespace AutSoft.Linq.Tests.Queryable.OrderByExtensions;

public partial class OrderByFromMappings : OrderByExtensionsTests
{
    [Theory]
    [InlineData(nameof(PersonDto.Id))]
    [InlineData(nameof(PersonDto.Name))]
    [InlineData(nameof(PersonDto.Age))]
    public void Should_ReturnOrdered(string orderBy)
    {
        // Act
        var ordered = Subject
            .OrderBy<Person, PersonDto>(new PageRequest { OrderBy = orderBy }, p => p.Id, Mapper.ConfigurationProvider)
            .ProjectTo<PersonDto>(Mapper.ConfigurationProvider)
            .ToList();

        // Assert
        ordered.Should().HaveCount(Subject.Count());
        ordered.Should().BeInAscendingOrder(orderBy switch
        {
            nameof(PersonDto.Id) => (Expression<Func<PersonDto, object?>>)(p => p.Id),
            nameof(PersonDto.Name) => p => p.Name,
            nameof(PersonDto.Age) => p => p.Age,
            _ => throw new NotSupportedException("Not valid test case input"),
        });
    }
}
