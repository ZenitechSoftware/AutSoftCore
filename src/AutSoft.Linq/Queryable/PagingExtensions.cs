using AutSoft.Linq.Models;

using Microsoft.EntityFrameworkCore;

namespace AutSoft.Linq.Queryable;

public static class PagingExtensions
{
    public static async Task<PageResponse<TSource>> ToPagedListAsync<TSource>(
        this IQueryable<TSource> source, PageRequest pageRequest, CancellationToken cancellationToken = default)
    {
        var totalCount = await source.CountAsync(cancellationToken);
        var pageCount = (totalCount + pageRequest.PageSize - 1) / pageRequest.PageSize;

        return new PageResponse<TSource>(
            await source.Skip(pageRequest.Page * pageRequest.PageSize).Take(pageRequest.PageSize).ToListAsync(cancellationToken), pageRequest.Page, totalCount, pageCount);
    }
}
