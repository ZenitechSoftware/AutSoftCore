using System.Linq.Expressions;

namespace AutSoft.Linq.Expressions.ExpressionVisitors;

/// <summary>
/// An <see cref="ExpressionVisitor"/>, which swap the visited <see cref="Expression"/> to an other
/// </summary>
public class SwapVisitor : ExpressionVisitor
{
    private readonly Expression _source;
    private readonly Expression _replacement;

    /// <summary>
    /// Initializes a new instance of the <see cref="SwapVisitor"/> class.
    /// </summary>
    /// <param name="source">The <see cref="Expression"/>, with which identical ones must be replaced</param>
    /// <param name="replacement">The <see cref="Expression"/>, for which the matches must be exchanged</param>
    public SwapVisitor(Expression source, Expression replacement)
    {
        _source = source;
        _replacement = replacement;
    }

    /// <summary>
    /// Visit the <see cref="Expression"/>
    /// </summary>
    /// <param name="node">The <see cref="Expression"/>, that we visit</param>
    /// <returns>The <see cref="Expression"/> after the visit</returns>
    public override Expression? Visit(Expression? node)
    {
        return node == _source ? _replacement : base.Visit(node);
    }
}
