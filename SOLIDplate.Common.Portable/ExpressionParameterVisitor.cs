using System.Collections.Generic;
using System.Linq.Expressions;

namespace SOLIDplate.Common.Portable
{
    internal class ExpressionParameterVisitor : ExpressionVisitor
	{
		private readonly Dictionary<ParameterExpression, ParameterExpression> _expressionMap;

		private ExpressionParameterVisitor(Dictionary<ParameterExpression, ParameterExpression> expressionMap)
		{
			_expressionMap = expressionMap ?? new Dictionary<ParameterExpression, ParameterExpression>();
		}

		public static Expression VisitParameters(Dictionary<ParameterExpression, ParameterExpression> parameterExpressionMap, Expression expression)
		{
			return new ExpressionParameterVisitor(parameterExpressionMap).Visit(expression);
		}

		protected override Expression VisitParameter(ParameterExpression parameterExpression)
		{
			ParameterExpression replacement;
			if (_expressionMap.TryGetValue(parameterExpression, out replacement))
			{
				parameterExpression = replacement;
			}
			return base.VisitParameter(parameterExpression);
		}
	}
}