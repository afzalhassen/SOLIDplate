using System;
using System.Linq;
using System.Linq.Expressions;

namespace SOLIDplate.Common.Portable
{
    public static class ExpressionExtensions
    {
        public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> leftExpression, Expression<Func<T, bool>> rightExpression)
        {
            return leftExpression.Compose(rightExpression, Expression.AndAlso);
        }

        public static Expression<Func<T, bool>> OrElse<T>(this Expression<Func<T, bool>> leftExpression, Expression<Func<T, bool>> rightExpression)
        {
            return leftExpression.Compose(rightExpression, Expression.OrElse);
        }

        private static Expression<T> Compose<T>(this Expression<T> leftExpression, Expression<T> rightExpression, Func<Expression, Expression, Expression> binaryExpression)
        {
            // Build parameter map (from parameters of rightExpression to parameters of leftExpression)
            var parameterExpressionMap = leftExpression.Parameters.Select((leftExpressionParameter, leftExpressionParameterIndex) => new
            {
                Key = rightExpression.Parameters[leftExpressionParameterIndex],
                Value = leftExpressionParameter
            })
                                                       .ToDictionary(parameterMap => parameterMap.Key, p => p.Value);

            // Replace parameters in the rightExpression with parameters from the leftExpression
            var rightExpressionBody = ExpressionParameterVisitor.VisitParameters(parameterExpressionMap, rightExpression.Body);

            // Apply composition of lambda expression bodies to parameters from the leftExpression 
            var binaryExpressionBody = binaryExpression(leftExpression.Body, rightExpressionBody);
            return Expression.Lambda<T>(binaryExpressionBody, leftExpression.Parameters);
        }

        public static string GetPropertyName<TEntity>(Expression<Func<TEntity, object>> expression)
            where TEntity : class
        {
            var body = expression.Body as MemberExpression ?? ((UnaryExpression)expression.Body).Operand as MemberExpression;

            if (body != null)
            {
                return body.Member.Name;
            }

            throw new ArgumentNullException(nameof(expression));
        }

        #region StaticReflection
        public static string GetMemberName<T>(this T instance, Expression<Func<T, object>> expression)
        {
            return GetMemberName(expression);
        }

        private static string GetMemberName<T>(Expression<Func<T, object>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentException("The expression cannot be null.");
            }

            return GetMemberName(expression.Body);
        }

        public static string GetMemberName<T>(this T instance, Expression<Action<T>> expression)
        {
            return GetMemberName(expression);
        }

        private static string GetMemberName<T>(Expression<Action<T>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentException("The expression cannot be null.");
            }

            return GetMemberName(expression.Body);
        }

        private static string GetMemberName(Expression expression)
        {
            if (expression == null)
            {
                throw new ArgumentException("The expression cannot be null.");
            }

            if (expression is MemberExpression)
            {
                // Reference type property or field
                var memberExpression = (MemberExpression)expression;
                return memberExpression.Member.Name;
            }

            if (expression is MethodCallExpression)
            {
                // Reference type method
                var methodCallExpression = (MethodCallExpression)expression;
                return methodCallExpression.Method.Name;
            }

            if (expression is UnaryExpression)
            {
                // Property, field of method returning value type
                var unaryExpression = (UnaryExpression)expression;
                return GetMemberName(unaryExpression);
            }

            throw new ArgumentException("Invalid expression");
        }

        private static string GetMemberName(UnaryExpression unaryExpression)
        {
            if (unaryExpression.Operand is MethodCallExpression)
            {
                var methodExpression = (MethodCallExpression)unaryExpression.Operand;
                return methodExpression.Method.Name;
            }

            return ((MemberExpression)unaryExpression.Operand).Member.Name;
        }
        #endregion
    }
}