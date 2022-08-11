using AutSoft.Common.Exceptions;

using Microsoft.EntityFrameworkCore;

using System.Linq.Expressions;

namespace AutSoft.Linq.Queryable;

/// <summary>
/// Extensions methods for <see cref="IQueryable{T}"/> with extended functionality for Single and First methods/>
/// </summary>
public static class FirstAndSingleExtensions
{
    /// <summary>
    /// Similar to <see cref="EntityFrameworkQueryableExtensions.SingleOrDefaultAsync{TSource}(IQueryable{TSource}, CancellationToken)"/>
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="source" />.</typeparam>
    /// <param name="source">An <see cref="IQueryable{T}" /> to return the single element of.</param>
    /// <param name="predicate">A function to test an element for a condition.</param>
    /// <param name="entityId">Identifier of the requested element</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <exception cref="EntityNotFoundException">Throws when no result found</exception>
    public static async Task<T> SingleEntityAsync<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate, long entityId, CancellationToken cancellationToken = default)
    {
        return await source.SingleOrDefaultAsync(predicate, cancellationToken)
            ?? throw EntityNotFoundException.CreateForType<T>(entityId);
    }

    /// <summary>
    /// Similar to <see cref="EntityFrameworkQueryableExtensions.SingleOrDefaultAsync{TSource}(IQueryable{TSource}, CancellationToken)"/>
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="source" />.</typeparam>
    /// <param name="source">An <see cref="IQueryable{T}" /> to return the single element of.</param>
    /// <param name="predicate">A function to test an element for a condition.</param>
    /// <param name="queryParameters">Query parameters of the requested element</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
    /// <exception cref="EntityNotFoundException">Throws when no result found</exception>
    public static async Task<T> SingleEntityAsync<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate, object[] queryParameters, CancellationToken cancellationToken = default)
    {
        return await source.SingleAsync(predicate, cancellationToken)
            ?? throw EntityNotFoundException.CreateForTypeCustomParams<T>(queryParameters);
    }
}
