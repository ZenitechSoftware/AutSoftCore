using System.Linq.Expressions;

namespace AutSoft.Linq.Expressions.ExpressionVisitors;

/// <summary>
/// An <see cref="ExpressionVisitor"/>, which update a <see cref="ParameterExpression"/>
/// </summary>
public class ParameterUpdateVisitor : ExpressionVisitor
{
    private readonly ParameterExpression _oldParameter;
    private readonly ParameterExpression _newParameter;

    /// <summary>
    /// Initializes a new instance of the <see cref="ParameterUpdateVisitor"/> class.
    /// </summary>
    /// <param name="oldParameter">The <see cref="ParameterExpression"/>, with which identical ones must be replaced</param>
    /// <param name="newParameter">The <see cref="ParameterExpression"/>, for which the matches must be exchanged</param>
    public ParameterUpdateVisitor(ParameterExpression oldParameter, ParameterExpression newParameter)
    {
        _oldParameter = oldParameter;
        _newParameter = newParameter;
    }

    /// <summary>
    /// Visit the <see cref="ParameterExpression"/>
    /// </summary>
    /// <param name="node">The <see cref="ParameterExpression"/>, that we visit</param>
    /// <returns>The <see cref="ParameterExpression"/> after the visit</returns>
    protected override Expression VisitParameter(ParameterExpression node)
    {
        return ReferenceEquals(node, _oldParameter) ? _newParameter : base.VisitParameter(node);
    }
}
