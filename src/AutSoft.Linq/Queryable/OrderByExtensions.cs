using AutoMapper;
using AutoMapper.Internal;

using AutSoft.Common.Exceptions;
using AutSoft.Linq.Models;

using Microsoft.EntityFrameworkCore;

using System.Linq.Expressions;
using System.Reflection;

namespace AutSoft.Linq.Queryable;

/// <summary>
/// Extensions methods for <see cref="IQueryable{T}"/> with extended functionality fo element ordering
/// </summary>
public static class OrderByExtensions
{
    /// <summary>
    /// Determine that a property has <see cref="NotSortableAttribute"/>
    /// </summary>
    /// <param name="propertyInfo">Property to examine</param>
    /// <returns>True if the property has <see cref="NotSortableAttribute"/></returns>
    public static bool IsSortable(this PropertyInfo propertyInfo)
    {
        return !Attribute.IsDefined(propertyInfo, typeof(NotSortableAttribute));
    }

    /// <summary>
    /// OrderBy where the direction is coming from a parameter in order to write order by with fluent syntax
    /// </summary>
    /// <typeparam name="TSource">Element's type</typeparam>
    /// <typeparam name="TKey">Ordering key's type</typeparam>
    /// <param name="source">An <see cref="IQueryable{T}" /> to order</param>
    /// <param name="keySelector">Expression of the ordering key</param>
    /// <param name="orderDirection">Direction of ordering represents with <see cref="OrderDirection"/> enum</param>
    /// <returns><see cref="IQueryable{T}" /> with ordering information</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S4136:Method overloads should be grouped together", Justification = "ThenBy is more related than the other OrderBy methods")]
    public static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(
        this IQueryable<TSource> source,
        Expression<Func<TSource, TKey>> keySelector,
        OrderDirection orderDirection)
    {
        return orderDirection == OrderDirection.Descending
            ? source.OrderByDescending(keySelector)
            : source.OrderBy(keySelector);
    }

    /// <summary>
    /// ThenBy where the direction is coming from a parameter in order to write order by with fluent syntax
    /// </summary>
    /// <typeparam name="TSource">Element's type</typeparam>
    /// <typeparam name="TKey">Ordering key's type</typeparam>
    /// <param name="source">An <see cref="IQueryable{T}" /> to order</param>
    /// <param name="keySelector">Expression of the ordering key</param>
    /// <param name="orderDirection">Direction of ordering represents with <see cref="OrderDirection"/> enum</param>
    /// <returns><see cref="IQueryable{T}" /> with ordering information</returns>
    public static IOrderedQueryable<TSource> ThenBy<TSource, TKey>(
        this IOrderedQueryable<TSource> source,
        Expression<Func<TSource, TKey>> keySelector,
        OrderDirection orderDirection)
    {
        return orderDirection == OrderDirection.Descending
            ? source.ThenByDescending(keySelector)
            : source.ThenBy(keySelector);
    }

    /// <summary>
    /// OrderBy where the desired ordering is coming from a <see cref="PageRequest"/>
    /// </summary>
    /// <typeparam name="TSource">Element's type</typeparam>
    /// <param name="source">An <see cref="IQueryable{T}" /> to order</param>
    /// <param name="pageRequest">A page request which contains the desired column's name to order.</param>
    /// <param name="defaultOrderingSelector">If the page request does not define any ordering information</param>
    /// <returns><see cref="IQueryable{T}" /> with ordering information</returns>
    /// <remarks>
    /// This overload fits if the page request contains ordering information in entity model level.
    /// If you use AutoMapper's <see cref="IMapper.ProjectTo"/> consider to use overloads with TDto type parameters
    /// </remarks>
    public static IOrderedQueryable<TSource> OrderBy<TSource>(
        this IQueryable<TSource> source,
        PageRequest pageRequest,
        Expression<Func<TSource, object?>> defaultOrderingSelector)
    {
        return source.OrderBy(
            string.IsNullOrEmpty(pageRequest.OrderBy)
                ? defaultOrderingSelector
                : e => EF.Property<object>(e!, pageRequest.OrderBy),
            pageRequest.OrderDirection);
    }

    /// <summary>
    /// OrderBy where the desired ordering is coming from a <see cref="PageRequest"/>
    /// and mapping expression calculated based on the provided mapping configuration
    /// </summary>
    /// <typeparam name="TSource">Element's type</typeparam>
    /// <typeparam name="TDto">Target DTO's type to find mapping information</typeparam>
    /// <param name="source">An <see cref="IQueryable{T}" /> to order</param>
    /// <param name="pageRequest">A page request which contains the desired column's name to order.</param>
    /// <param name="defaultOrderingSelector">If the page request does not define any ordering information</param>
    /// <param name="mappings">AutoMapper mapping configuration which contains mapping expressions for TSource -> TDto type conversion</param>
    /// <returns><see cref="IQueryable{T}" /> with ordering information</returns>
    public static IOrderedQueryable<TSource> OrderBy<TSource, TDto>(
        this IQueryable<TSource> source,
        PageRequest pageRequest,
        Expression<Func<TSource, object?>> defaultOrderingSelector,
        IConfigurationProvider mappings)
    {
        var orderKeySelector = GetOrderKeySelector<TSource, TDto>(pageRequest, defaultOrderingSelector, mappings);

        return source.OrderBy(orderKeySelector, pageRequest.OrderDirection);
    }

    /// <summary>
    /// OrderBy where the desired ordering is coming from a <see cref="PageRequest"/>
    /// and mapping expression calculated based on the provided mapping configuration
    /// </summary>
    /// <typeparam name="TSource">Element's type</typeparam>
    /// <typeparam name="TDto">Target DTO's type to find mapping information</typeparam>
    /// <param name="source">An <see cref="IQueryable{T}" /> to order</param>
    /// <param name="pageRequest">A page request which contains the desired column's name to order.</param>
    /// <param name="defaultOrderingSelector">If the page request does not define any ordering information</param>
    /// <param name="defaultOrderDirection">Ordering direction for <paramref name="defaultOrderingSelector"/></param>
    /// <param name="mappings">AutoMapper mapping configuration which contains mapping expressions for TSource -> TDto type conversion</param>
    /// <returns><see cref="IQueryable{T}" /> with ordering information</returns>
    public static IOrderedQueryable<TSource> OrderBy<TSource, TDto>(
       this IQueryable<TSource> source,
       PageRequest pageRequest,
       Expression<Func<TSource, object?>> defaultOrderingSelector,
       OrderDirection defaultOrderDirection,
       IConfigurationProvider mappings)
    {
        var orderKeySelector = GetOrderKeySelector<TSource, TDto>(pageRequest, defaultOrderingSelector, mappings);

        if (orderKeySelector == defaultOrderingSelector)
            return source.OrderBy(orderKeySelector, defaultOrderDirection);

        return source.OrderBy(orderKeySelector, pageRequest.OrderDirection);
    }

    private static Expression<Func<TSource, object?>> GetOrderKeySelector<TSource, TDto>(
        PageRequest pageRequest,
        Expression<Func<TSource, object?>> defaultOrderingSelector,
        IConfigurationProvider mappings)
    {
        if (!string.IsNullOrEmpty(pageRequest.OrderBy))
        {
            // The caller want to order based on a not existed or an unsortable property
            var pi = typeof(TDto).GetProperty(pageRequest.OrderBy);
            if (pi?.IsSortable() != true)
                throw new ValidationException(pageRequest.OrderBy, "Cannot sort based on this property!");
        }

        var orderKeySelector = defaultOrderingSelector;

        if (!string.IsNullOrEmpty(pageRequest.OrderBy))
        {
            var expression = mappings.Internal().FindTypeMapFor<TSource, TDto>()
                ?.PropertyMaps
                ?.FirstOrDefault(m => m.CustomMapExpression != null && m.DestinationName == pageRequest.OrderBy)
                ?.CustomMapExpression;

            orderKeySelector = expression != null
                ? Expression.Lambda<Func<TSource, object?>>(Expression.Convert(expression.Body, typeof(object)), expression.Parameters)
                : e => EF.Property<object>(e!, pageRequest.OrderBy);
        }

        return orderKeySelector;
    }
}
