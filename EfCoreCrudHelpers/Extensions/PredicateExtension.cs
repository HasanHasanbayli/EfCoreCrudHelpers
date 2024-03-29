using System.Linq.Expressions;

namespace EfCoreCrudHelpers.Extensions;

public static class PredicateExtension
{
    public static Expression<Func<T, bool>> And<T>(
        this Expression<Func<T, bool>> expr1,
        Expression<Func<T, bool>> expr2)
    {
        ParameterExpression target = expr1.Parameters[0];
        ParameterExpression source = expr2.Parameters[0];

        BinaryExpression body = Expression.AndAlso(left: expr1.Body, right: expr2.Body
            .ReplaceParameter(source: source, target: target));

        return Expression.Lambda<Func<T, bool>>(body, source);
    }

    public static Expression<Func<T, bool>> Or<T>(
        this Expression<Func<T, bool>> expr1,
        Expression<Func<T, bool>> expr2)
    {
        ParameterExpression target = expr1.Parameters[0];
        ParameterExpression source = expr2.Parameters[0];

        BinaryExpression body = Expression.OrElse(left: expr1.Body, right: expr2.Body
            .ReplaceParameter(source: source, target: target));

        return Expression.Lambda<Func<T, bool>>(body, target);
    }

    public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expr)
    {
        ParameterExpression param = expr.Parameters[0];

        UnaryExpression body = Expression.Not(expr.Body);

        return Expression.Lambda<Func<T, bool>>(body, param);
    }

    private static Expression ReplaceParameter(
        this Expression expression,
        ParameterExpression source,
        ParameterExpression target)
    {
        return new ParameterReplacer(source, target).Visit(expression);
    }

    private class ParameterReplacer(ParameterExpression source, ParameterExpression target) : ExpressionVisitor
    {
        protected override Expression VisitParameter(ParameterExpression node)
        {
            return node == source
                ? target
                : base.VisitParameter(node);
        }
    }
}