using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace AutSoft.Common.Validation;

/// <summary>
/// Extension methods for object.
/// </summary>
public static class ObjectExtensions
{
    private static readonly Regex IndexRegex = new(@".get_Item\((\d+)\)", RegexOptions.Compiled);
    private static readonly Regex PatternRegex = new(@".get_Item\((.+)\).+\)", RegexOptions.Compiled);
    private static readonly Regex ElementAtRegex = new(".ElementAt.+", RegexOptions.Compiled);

    /// <summary>
    /// Gets the name of the specified function.
    /// </summary>
    /// <typeparam name="T">Type of the func parameter.</typeparam>
    /// <typeparam name="TU">Type of the func to return.</typeparam>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Extension method")]
    public static string NameOf<T, TU>(this T obj, Expression<Func<T, TU>> expression)
    {
        var result = expression.Body.ToString();

        if (!IndexRegex.IsMatch(result) && PatternRegex.IsMatch(result))
        {
            return ResolveIndexedExpression(expression);
        }
        else if (!IndexRegex.IsMatch(result) && ElementAtRegex.IsMatch(result))
        {
            return ResolveElementAtExpression(expression);
        }

        var firstDot = result.IndexOf('.');
        if (firstDot < 0)
            throw new ArgumentException("Expression body most have a dot!");

        firstDot++;
        result = result[firstDot..];

        result = IndexRegex.Replace(result, m => m.Value.Replace(".get_Item", string.Empty).Replace('(', '[').Replace(')', ']'));

        return result;
    }

    private static string ResolveElementAtExpression<T, TU>(Expression<Func<T, TU>> expression)
    {
        var names = new List<string>();
        var exp = expression.Body;

        while (exp != null)
        {
            if (exp is MemberExpression memberExpression)
            {
                exp = HandleMemberExpression(memberExpression, names);
                if (exp == null)
                    break;
            }

            if (exp is MethodCallExpression methodCallExpression)
            {
                var methodMemberExpression = methodCallExpression.Arguments[1] as MemberExpression;
                var argumentMemberExpression = methodCallExpression.Arguments[0] as MemberExpression;

                if (methodMemberExpression!.Expression is ConstantExpression constantExpression)
                {
                    var index = (int)constantExpression!.Value!.GetType()!.GetField(methodMemberExpression.Member.Name)!.GetValue(constantExpression.Value)!;
                    names.Add($"{argumentMemberExpression!.Member.Name.ToCamelCase()}[{index}]");

                    if (argumentMemberExpression.Expression is MethodCallExpression innerMethodExpression)
                        exp = innerMethodExpression;
                    else if (argumentMemberExpression.Expression is MemberExpression innerMemberExpression)
                        exp = innerMemberExpression;
                    else
                        break;
                }
                else
                {
                    HandleMethodCallExpression(methodMemberExpression, argumentMemberExpression!, names);
                    break;
                }
            }
            else
            {
                break;
            }
        }

        names.Reverse();
        return string.Join(".", names);
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0019:Use pattern matching", Justification = "Used in else")]
    private static string ResolveIndexedExpression<T, TU>(Expression<Func<T, TU>> expression)
    {
        var names = new List<string>();
        var exp = expression.Body;

        while (exp != null)
        {
            if (exp is MemberExpression memberExpression)
            {
                exp = HandleMemberExpression(memberExpression, names);
                if (exp == null)
                    break;
            }

            if (exp is MethodCallExpression methodCallExpression)
            {
                var methodMemberExpression = methodCallExpression.Object as MemberExpression;
                var argumentMemberExpression = methodCallExpression.Arguments[0] as MemberExpression;

                if (argumentMemberExpression!.Expression is ConstantExpression constantExpression)
                {
                    var index = (int)constantExpression!.Value!.GetType()!.GetField(argumentMemberExpression.Member.Name)!.GetValue(constantExpression.Value)!;

                    if (methodMemberExpression == null)
                    {
                        names.Add($"[{index}]");
                        break;
                    }

                    names.Add($"{methodMemberExpression.Member.Name.ToCamelCase()}[{index}]");

                    if (methodMemberExpression.Expression is MethodCallExpression innerMethodExpression)
                        exp = innerMethodExpression;
                    else if (methodMemberExpression.Expression is MemberExpression innerMemberExpression)
                        exp = innerMemberExpression;
                    else
                        break;
                }
                else
                {
                    HandleMethodCallExpression(methodMemberExpression!, argumentMemberExpression, names);
                    break;
                }
            }
            else
            {
                break;
            }
        }

        names.Reverse();
        return string.Join(".", names);
    }

    private static Expression? HandleMemberExpression(MemberExpression memberExpression, List<string> names)
    {
        names.Add(memberExpression.Member.Name.ToCamelCase());

        var expression = memberExpression.Expression;
        if (expression is MethodCallExpression or MemberExpression)
            return expression;

        return null;
    }

    private static void HandleMethodCallExpression(MemberExpression methodMemberExpression, MemberExpression argumentMemberExpression, List<string> names)
    {
        var @object = Expression.Convert(argumentMemberExpression, typeof(object));
        var lambda = Expression.Lambda<Func<object>>(@object);
        var value = lambda.Compile()();

        names.Add($"{methodMemberExpression.Member.Name.ToCamelCase()}[{value}]");
    }
}
