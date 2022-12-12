using System.Linq.Expressions;

namespace AutSoft.Linq.Queryable;

/// <summary>
/// Extensions methods for <see cref="IQueryable{T}"/> with helper functionality for Where methods
/// </summary>
public static class WhereExtensions
{
    /// <summary>
    /// Conditionally append a where expression to <see cref="IQueryable{T}"/> for performance considerations.
    /// </summary>
    /// <typeparam name="TSource">Element's type</typeparam>
    /// <param name="source">An <see cref="IQueryable{T}" /> to conditionally filter</param>
    /// <param name="condition">Condition to determine which expression to use in Where clause</param>
    /// <param name="conditionTruePredicate">Expression to use if the <paramref name="condition"/> is true</param>
    /// <param name="conditionFalsePredicate">Optional expression to use if the <paramref name="condition"/> is false</param>
    /// <returns><see cref="IQueryable{T}"/> with filtering expressions.</returns>
    public static IQueryable<TSource> Where<TSource>(
        this IQueryable<TSource> source,
        bool condition,
        Expression<Func<TSource, bool>> conditionTruePredicate,
        Expression<Func<TSource, bool>>? conditionFalsePredicate = null)
    {
        if (condition)
        {
            return source.Where(conditionTruePredicate);
        }
        else if (conditionFalsePredicate != null)
        {
            return source.Where(conditionFalsePredicate);
        }
        else
        {
            return source;
        }
    }

    /// <summary>
    /// Conditionally append statements to a <see cref="IQueryable{T}"/> fluent call chain
    /// in order to keep the fluent syntax
    /// </summary>
    /// <typeparam name="TSource">Element's type</typeparam>
    /// <param name="source">An <see cref="IQueryable{T}" /> to extend</param>
    /// <param name="condition">Condition to determine transform the source or not</param>
    /// <param name="transform">Function which describes the modifications on the <paramref name="source"/>.</param>
    /// <returns>An extended <see cref="IQueryable{T}"/></returns>
    public static IQueryable<TSource> If<TSource>(
        this IQueryable<TSource> source,
        bool condition,
        Func<IQueryable<TSource>, IQueryable<TSource>> transform)
    {
        return condition ? transform(source) : source;
    }
}
