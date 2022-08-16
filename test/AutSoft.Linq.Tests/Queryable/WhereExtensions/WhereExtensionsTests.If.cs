using AutSoft.Linq.Queryable;

using FluentAssertions;

namespace AutSoft.Linq.Tests.Queryable.WhereExtensions;

public partial class WhereExtensionsTests
{
    public class If : WhereExtensionsTests
    {
        [Fact]
        public void Should_AppendTrueExpression()
        {
            // Arrange
            var subject = Enumerable.Range(1, 10).AsQueryable();

            // Act
            var result = subject.If(true, q => q.Where(i => i % 2 == 0)).ToList();

            // Assert
            result.Should().HaveCount(subject.Count() / 2);
            result.Should().AllSatisfy(x => x.Should().Match(i => i % 2 == 0));
        }

        [Fact]
        public void Should_NotAppendTruePredicate()
        {
            // Arrange
            var subject = Enumerable.Range(1, 10).AsQueryable();

            // Act
            var result = subject.If(false, q => q.Where(i => i % 2 == 0)).ToList();

            // Assert
            result.Should().HaveCount(subject.Count());
            result.Should().BeEquivalentTo(subject);
        }
    }
}
