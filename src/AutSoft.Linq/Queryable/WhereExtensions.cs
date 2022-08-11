using System.Linq.Expressions;

namespace AutSoft.Linq.Queryable;

public static class WhereExtensions
{
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

    public static IQueryable<TSource> Where<TSource>(
        this IQueryable<TSource> source,
        bool? condition,
        Expression<Func<TSource, bool>> truePredicate,
        Expression<Func<TSource, bool>> falsePredicate)
    {
        if (!condition.HasValue)
            return source;

        return condition.Value
            ? source.Where(truePredicate)
            : source.Where(falsePredicate);
    }

    public static IQueryable<T> If<T>(
        this IQueryable<T> source,
        bool condition,
        Func<IQueryable<T>, IQueryable<T>> transform)
    {
        return condition ? transform(source) : source;
    }
}
