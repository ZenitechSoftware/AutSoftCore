using AutSoft.Linq.Models;

using Microsoft.EntityFrameworkCore;

namespace AutSoft.Linq.Queryable;

/// <summary>
/// Extensions methods for <see cref="IQueryable{T}"/> to created paged responses
/// </summary>
public static class PagingExtensions
{
    /// <summary>
    /// Create a paged response based on <see cref="PageRequest"/>
    /// </summary>
    /// <typeparam name="TSource">Element's type</typeparam>
    /// <param name="source">An <see cref="IQueryable{T}" /> to query its page</param>
    /// <param name="pageRequest">Paging parameters</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation.
    ///     The task result contains a <see cref="List{T}" /> that contains elements from the input sequence.
    /// </returns>
    public static async Task<PageResponse<TSource>> ToPagedListAsync<TSource>(
        this IQueryable<TSource> source,
        PageRequest pageRequest,
        CancellationToken cancellationToken = default)
    {
        var totalCount = await source.CountAsync(cancellationToken);
        var pageCount = (totalCount + pageRequest.PageSize - 1) / pageRequest.PageSize;

        return new PageResponse<TSource>(
            await source.Skip(pageRequest.Page * pageRequest.PageSize)
                .Take(pageRequest.PageSize)
                .ToListAsync(cancellationToken),
            pageRequest.Page,
            totalCount,
            pageCount);
    }
}
