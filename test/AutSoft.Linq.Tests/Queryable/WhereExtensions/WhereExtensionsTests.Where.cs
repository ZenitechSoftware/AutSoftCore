using AutSoft.Linq.Queryable;

using FluentAssertions;

namespace AutSoft.Linq.Tests.Queryable.WhereExtensions;

public partial class WhereExtensionsTests
{
    public class Where : WhereExtensionsTests
    {
        [Fact]
        public void Should_AppendTruePredicate()
        {
            // Arrange
            var subject = System.Linq.Enumerable.Range(1, 10).AsQueryable();

            // Act
            var result = subject.Where(true, x => x % 2 == 0).ToList();

            // Assert
            result.Should().HaveCount(subject.Count() / 2);
            result.Should().AllSatisfy(x => x.Should().Match(i => i % 2 == 0));
        }

        [Fact]
        public void Should_NotAppendTruePredicate()
        {
            // Arrange
            var subject = System.Linq.Enumerable.Range(1, 10).AsQueryable();

            // Act
            var result = subject.Where(false, x => x % 2 == 0).ToList();

            // Assert
            result.Should().HaveCount(subject.Count());
            result.Should().BeEquivalentTo(subject);
        }

        [Fact]
        public void Should_AppendFalsePredicate()
        {
            // Arrange
            var subject = System.Linq.Enumerable.Range(1, 10).AsQueryable();

            // Act
            var result = subject.Where(false, x => x % 2 == 0, x => x % 2 == 1).ToList();

            // Assert
            result.Should().HaveCount(subject.Count() / 2);
            result.Should().AllSatisfy(x => x.Should().Match(i => i % 2 == 1));
        }
    }
}
