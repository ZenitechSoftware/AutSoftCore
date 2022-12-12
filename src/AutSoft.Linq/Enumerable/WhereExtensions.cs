namespace AutSoft.Linq.Enumerable;

/// <summary>
/// Extensions methods for <see cref="IEnumerable{T}"/> with helper functionality for Where methods
/// </summary>
public static class WhereExtensions
{
    /// <summary>
    /// Conditionally append a where expression to <see cref="IEnumerable{T}"/> for performance considerations.
    /// </summary>
    /// <typeparam name="TSource">Element's type</typeparam>
    /// <param name="source">An <see cref="IEnumerable{T}" /> to conditionally filter</param>
    /// <param name="condition">Condition to determine which function to use in Where clause</param>
    /// <param name="conditionTruePredicate">Function to use if the <paramref name="condition"/> is true</param>
    /// <param name="conditionFalsePredicate">Optional function to use if the <paramref name="condition"/> is false</param>
    /// <returns><see cref="IEnumerable{T}"/> with filtering expressions.</returns>
    public static IEnumerable<TSource> Where<TSource>(
        this IEnumerable<TSource> source,
        bool condition,
        Func<TSource, bool> conditionTruePredicate,
        Func<TSource, bool>? conditionFalsePredicate = null)
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
    /// Conditionally append statements to a <see cref="IEnumerable{T}"/> fluent call chain
    /// in order to keep the fluent syntax
    /// </summary>
    /// <typeparam name="TSource">Element's type</typeparam>
    /// <param name="source">An <see cref="IEnumerable{T}" /> to extend</param>
    /// <param name="condition">Condition to determine transform the source or not</param>
    /// <param name="transform">Function which describes the modifications on the <paramref name="source"/>.</param>
    /// <returns>An extended <see cref="IEnumerable{T}"/></returns>
    public static IEnumerable<TSource> If<TSource>(
        this IEnumerable<TSource> source,
        bool condition,
        Func<IEnumerable<TSource>, IEnumerable<TSource>> transform)
    {
        return condition ? transform(source) : source;
    }
}
