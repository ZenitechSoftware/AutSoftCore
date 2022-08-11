using AutSoft.Common.Exceptions;

using Microsoft.EntityFrameworkCore;

using System.Linq.Expressions;

namespace AutSoft.Linq.Queryable;

public static class FirstAndSingleExtensions
{
    public static async Task<T> SingleEntityAsync<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate, long entityId)
    {
        try
        {
            return await source.SingleAsync(predicate);
        }
        catch (InvalidOperationException e)
        {
            // Single throws an InvalidOperationException when no entities are found
            throw EntityNotFoundException.CreateForType<T>(e, entityId);
        }
    }

    public static async Task<T> SingleEntityAsync<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate, params object[] queryParameters)
    {
        try
        {
            return await source.SingleAsync(predicate);
        }
        catch (InvalidOperationException e)
        {
            // Single throws an InvalidOperationException when no entities are found
            throw EntityNotFoundException.CreateForTypeCustomParams<T>(e, queryParameters);
        }
    }

    public static T SingleEntity<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate, params object[] queryParameters)
    {
        try
        {
            return source.Single(predicate);
        }
        catch (InvalidOperationException e)
        {
            // Single throws an InvalidOperationException when no entities are found
            throw EntityNotFoundException.CreateForTypeCustomParams<T>(e, queryParameters);
        }
    }

    public static async Task<T> FirstEntityAsync<T>(this IQueryable<T> source, Expression<Func<T, bool>> predicate)
    {
        try
        {
            return await source.FirstAsync(predicate);
        }
        catch (InvalidOperationException e)
        {
            throw EntityNotFoundException.CreateForType<T>(e, default);
        }
    }
}
