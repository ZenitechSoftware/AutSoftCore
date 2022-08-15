using AutSoft.Linq.Queryable;

using FluentAssertions;

using System.Reflection;

namespace AutSoft.Linq.Tests.Queryable.OrderByExtensions;

public partial class IsSortable : OrderByExtensionsTests
{
    public static TheoryData<PropertyInfo?, bool> SortablePropertyTheoryData => new()
    {
        { typeof(PersonDto).GetProperty(nameof(PersonDto.Age)), true },
        { typeof(PersonDto).GetProperty(nameof(PersonDto.Address)), false },
    };

    [Theory]
    [MemberData(nameof(SortablePropertyTheoryData))]
    public void Should_ReturnCorrentIncication(PropertyInfo? propertyInfo, bool expectedResult)
    {
        propertyInfo!.IsSortable().Should().Be(expectedResult);
    }
}
