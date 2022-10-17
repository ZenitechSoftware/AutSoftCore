using AutSoft.Linq.Expressions.ExpressionVisitors;

using System.Linq.Expressions;

namespace AutSoft.Linq.Expressions;

/// <summary>
/// Extension methods for <see cref="Expression{T}"/> with extended functionality for logical operators
/// </summary>
public static class LogicalOperatorExtensions
{
    /// <summary>
    /// Performing an AND operation on two <see cref="Expression{T}"/>
    /// </summary>
    /// <typeparam name="T">Type of input paramter of the <see cref="Expression{T}"/>'s function</typeparam>
    /// <param name="leftExp">The left sided <see cref="Expression{T}"/> of AND operation</param>
    /// <param name="rightExp">The right sided <see cref="Expression{T}"/> of AND operation</param>
    /// <returns>The result <see cref="Expression{T}"/> after the AND operation</returns>
    public static Expression<Func<T, bool>> And<T>(
        this Expression<Func<T, bool>> leftExp,
        Expression<Func<T, bool>> rightExp)
    {
        var visitor = new ParameterUpdateVisitor(rightExp.Parameters[0], leftExp.Parameters[0]);
        rightExp = (Expression<Func<T, bool>>)visitor.Visit(rightExp);

        var binExp = Expression.And(leftExp.Body, rightExp.Body);
        return Expression.Lambda<Func<T, bool>>(binExp, rightExp.Parameters);
    }

    /// <summary>
    /// Performing an OR operation on two <see cref="Expression{T}"/>
    /// </summary>
    /// <typeparam name="T">Type of input paramter of the <see cref="Expression{T}"/>'s function</typeparam>
    /// <param name="leftExp">The left sided <see cref="Expression{T}"/> of OR operation</param>
    /// <param name="rightExp">The right sided <see cref="Expression{T}"/> of OR operation</param>
    /// <returns>The result <see cref="Expression{T}"/> after the OR operation</returns>
    public static Expression<Func<T, bool>> Or<T>(
        this Expression<Func<T, bool>> leftExp,
        Expression<Func<T, bool>> rightExp)
    {
        var visitor = new ParameterUpdateVisitor(rightExp.Parameters[0], leftExp.Parameters[0]);
        rightExp = (Expression<Func<T, bool>>)visitor.Visit(rightExp);

        var binExp = Expression.Or(leftExp.Body, rightExp.Body);
        return Expression.Lambda<Func<T, bool>>(binExp, rightExp.Parameters);
    }
}
