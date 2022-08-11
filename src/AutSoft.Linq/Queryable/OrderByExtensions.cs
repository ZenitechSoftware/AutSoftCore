using AutoMapper;
using AutoMapper.Internal;

using AutSoft.Common.Exceptions;
using AutSoft.Linq.Models;

using Microsoft.EntityFrameworkCore;

using System.Linq.Expressions;
using System.Reflection;

namespace AutSoft.Linq.Queryable;

public static class OrderByExtensions
{
    public static bool IsSortable(this PropertyInfo propertyInfo)
    {
        return !Attribute.IsDefined(propertyInfo, typeof(NotSortableAttribute));
    }

    public static IOrderedQueryable<TSource> OrderBy<TSource, TKey>(
        this IQueryable<TSource> source,
        Expression<Func<TSource, TKey>> keySelector,
        OrderDirection orderDirection)
    {
        return orderDirection == OrderDirection.Descending
            ? source.OrderByDescending(keySelector)
            : source.OrderBy(keySelector);
    }

    public static IOrderedQueryable<TSource> OrderBy<TSource>(
        this IQueryable<TSource> source,
        PageRequest pageRequest,
        Expression<Func<TSource, object>> defaultOrderingSelector)
    {
        return source.OrderBy(
            string.IsNullOrEmpty(pageRequest.OrderBy)
                ? defaultOrderingSelector
                : e => EF.Property<object>(e!, pageRequest.OrderBy),
            pageRequest.OrderDirection);
    }

    public static IOrderedQueryable<TSource> OrderBy<TSource, TDto>(
        this IQueryable<TSource> source,
        PageRequest pageRequest,
        Expression<Func<TSource, object>> defaultOrderingSelector,
        IConfigurationProvider mappings)
    {
        var orderKeySelector = GetOrderKeySelector<TSource, TDto>(pageRequest, defaultOrderingSelector, mappings);

        return source.OrderBy(orderKeySelector, pageRequest.OrderDirection);
    }

    public static IOrderedQueryable<TSource> OrderBy<TSource, TDto>(
       this IQueryable<TSource> source,
       PageRequest pageRequest,
       Expression<Func<TSource, object>> defaultOrderingSelector,
       OrderDirection defaultOrderDirection,
       IConfigurationProvider mappings)
    {
        var orderKeySelector = GetOrderKeySelector<TSource, TDto>(pageRequest, defaultOrderingSelector, mappings);

        if (orderKeySelector == defaultOrderingSelector)
            return source.OrderBy(orderKeySelector, defaultOrderDirection);

        return source.OrderBy(orderKeySelector, pageRequest.OrderDirection);
    }

    public static IOrderedQueryable<TSource> ThenBy<TSource, TKey>(
        this IOrderedQueryable<TSource> source,
        Expression<Func<TSource, TKey>> keySelector,
        OrderDirection orderDirection)
    {
        if (orderDirection == OrderDirection.Descending)
            return source.ThenByDescending(keySelector);

        return source.ThenBy(keySelector);
    }

    private static Expression<Func<TSource, object>> GetOrderKeySelector<TSource, TDto>(
        PageRequest pageRequest,
        Expression<Func<TSource, object>> defaultOrderingSelector,
        IConfigurationProvider mappings)
    {
        if (!string.IsNullOrEmpty(pageRequest.OrderBy))
        {
            // A kliens olyan property-re szeretne rendezni,
            // amire a lekérdezésben nincs ráhatásunk,
            // vagy nem szeretnénk engedni a rendezést
            var pi = typeof(TDto).GetProperty(pageRequest.OrderBy);
            if (pi?.IsSortable() != true)
                throw new ValidationException(pageRequest.OrderBy, "Ezt az oszlopot nem lehet sorba rendezni!");
        }

        var orderKeySelector = defaultOrderingSelector;

        if (!string.IsNullOrEmpty(pageRequest.OrderBy))
        {
            var expression = mappings.Internal().FindTypeMapFor<TSource, TDto>()
                ?.PropertyMaps
                ?.FirstOrDefault(m => m.CustomMapExpression != null && m.DestinationName == pageRequest.OrderBy)
                ?.CustomMapExpression;

            orderKeySelector = expression != null
                ? Expression.Lambda<Func<TSource, object>>(Expression.Convert(expression.Body, typeof(object)), expression.Parameters)
                : e => EF.Property<object>(e!, pageRequest.OrderBy);
        }

        return orderKeySelector;
    }
}
