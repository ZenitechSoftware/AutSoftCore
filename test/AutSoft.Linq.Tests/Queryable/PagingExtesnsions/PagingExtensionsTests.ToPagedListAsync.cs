using AutSoft.Linq.Models;
using AutSoft.Linq.Queryable;

using Bogus;

using FluentAssertions;

namespace AutSoft.Linq.Tests.Queryable.PagingExtesnsions;

public partial class PagingExtensionsTests
{
    public class ToPagedListAsync : PagingExtensionsTests
    {
        [Theory]
        [InlineData(5, 0, 10)]
        [InlineData(5, 1, 10)]
        [InlineData(105, 0, 10)]
        [InlineData(105, 1, 10)]
        [InlineData(105, 10, 10)]
        public async Task Should_ReturnPagedList(int totalItemCount, int page, int pageSize)
        {
            // Arrange
            var items = Faker.Generate(totalItemCount);
            DbContext.People.AddRange(items);
            DbContext.SaveChanges();
            var pageRequest = new PageRequest { Page = page, PageSize = pageSize };
            var subject = DbContext.People;

            // Act
            var result = await subject.ToPagedListAsync(pageRequest);

            // Assert
            result.CurrentPage.Should().Be(pageRequest.Page);
            result.PageCount.Should().Be((int)Math.Ceiling(totalItemCount / (double)pageSize));
            result.TotalCount.Should().Be(items.Count);
            result.Results.Should().HaveCount(Math.Min(pageRequest.PageSize, Math.Max(0, items.Count - pageRequest.Page * pageRequest.PageSize)));
            result.Results.Should().ContainInOrder(subject.Skip(pageRequest.Page * pageRequest.PageSize).Take(pageRequest.PageSize));
        }
    }
}
