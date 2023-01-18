using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Query;

using System.Linq.Expressions;

namespace AutSoft.EntityFrameworkCore.EntityTypeBuilders;

/// <summary>
/// Extension method for <see cref="EntityTypeBuilder"/> to help configure a query filter
/// </summary>
public static class AddQueryFilterExtension
{
    /// <summary>
    /// Replace the parameters of the <paramref name="expression"/>
    /// and after that register it for query filter as an <see cref="LambdaExpression"/>
    /// </summary>
    /// <typeparam name="T">Type of the parameter of <paramref name="expression"/></typeparam>
    /// <param name="entityTypeBuilder">An <see cref="EntityTypeBuilder"/> object</param>
    /// <param name="expression">The filter logic of the query filter</param>
    public static void AddQueryFilter<T>(this EntityTypeBuilder entityTypeBuilder, Expression<Func<T, bool>> expression)
    {
        var parameterType = Expression.Parameter(entityTypeBuilder.Metadata.ClrType);
        var expressionFilter = ReplacingExpressionVisitor.Replace(
            expression.Parameters.Single(), parameterType, expression.Body);

        var currentQueryFilter = entityTypeBuilder.Metadata.GetQueryFilter();
        if (currentQueryFilter != null)
        {
            var currentExpressionFilter = ReplacingExpressionVisitor.Replace(
                currentQueryFilter.Parameters.Single(), parameterType, currentQueryFilter.Body);
            expressionFilter = Expression.AndAlso(currentExpressionFilter, expressionFilter);
        }

        var lambdaExpression = Expression.Lambda(expressionFilter, parameterType);
        entityTypeBuilder.HasQueryFilter(lambdaExpression);
    }
}
