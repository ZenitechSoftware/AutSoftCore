using AutSoft.Linq.Expressions.ExpressionVisitors;

using System.Linq.Expressions;

namespace AutSoft.Linq.Expressions;

/// <summary>
/// Extension methods for <see cref="Expression{T}"/> with extended functionality for chaining
/// </summary>
public static class ChainExtension
{
    /// <summary>
    /// Chaining together two <see cref="Expression{T}"/>
    /// </summary>
    /// <typeparam name="TIn">Type of input paramter of the result <see cref="Expression{T}"/>'s function</typeparam>
    /// <typeparam name="TInterstitial">Type of interstitial parameter of the <see cref="Expression{T}"/> function</typeparam>
    /// <typeparam name="TOut">Type of output paramter of the result <see cref="Expression{T}"/>'s function</typeparam>
    /// <param name="incomingChainLink">The incoming <see cref="Expression{T}"/> to chaning</param>
    /// <param name="outgoingChainLink">The outgoing <see cref="Expression{T}"/> to chaning</param>
    /// <returns>The chained together <see cref="Expression{T}"/></returns>
    public static Expression<Func<TIn, TOut>> Chain<TIn, TInterstitial, TOut>(
        this Expression<Func<TIn, TInterstitial>> incomingChainLink,
        Expression<Func<TInterstitial, TOut>> outgoingChainLink)
    {
        var visitor = new SwapVisitor(outgoingChainLink.Parameters[0], incomingChainLink.Body);
        return Expression.Lambda<Func<TIn, TOut>>(visitor.Visit(outgoingChainLink.Body), incomingChainLink.Parameters);
    }
}
