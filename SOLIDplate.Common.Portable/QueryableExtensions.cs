using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SOLIDplate.Common.Portable
{
	public static class QueryableExtensions
	{
		public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string sortExpression)
		{
			const string lambdaMethodName = "Lambda";
			const string orderByMethodName = "OrderBy";

			if (source == null)
			{
				throw new ArgumentNullException("source", "source is null.");
			}

			if (string.IsNullOrEmpty(sortExpression))
			{
				throw new ArgumentException("sortExpression is null or empty.", "sortExpression");
			}

			var sortExpressionParts = sortExpression.Split(' ');
			var isDescending = false;
			var typeOfT = typeof(T);

			if (sortExpressionParts.Length <= 0 || sortExpressionParts[0] == string.Empty)
			{
				return source;
			}

			if (sortExpressionParts.Length > 1)
			{
				isDescending = sortExpressionParts[1].ToLower().Contains("esc");
			}
			var orderByMethodNameToUse = (isDescending ? string.Format("{0}Descending", orderByMethodName) : orderByMethodName);
			var propertyName = sortExpressionParts[0];
			var propertyInfo = typeOfT.GetRuntimeProperty(propertyName);

			if (propertyInfo == null)
			{
				var message = string.Format("No property named '{0}' exists on the type '{1}'.", propertyName, typeOfT.Name);
				throw new ArgumentException(message);
			}

			var delegateType = typeof(Func<,>).MakeGenericType(typeOfT, propertyInfo.PropertyType);
			var parameterExpression = Expression.Parameter(typeOfT);
			var propertyExpression = Expression.Property(parameterExpression, propertyInfo);
			Func<MethodInfo, bool> lamdaMatch = methodInfo => methodInfo.Name == lambdaMethodName &&
															  methodInfo.ContainsGenericParameters &&
															  methodInfo.GetParameters().Length == 2;
			var orderByLambda = typeof(Expression)
								.GetRuntimeMethods()
								.First(lamdaMatch)
								.MakeGenericMethod(delegateType)
								.Invoke(null, new object[] { propertyExpression, new[] { parameterExpression } });
			Func<MethodInfo, bool> orderByMatch = x => x.Name == orderByMethodNameToUse &&
													   x.GetParameters().Length == 2;
			var orderByMethodInfo = typeof(Queryable)
									.GetRuntimeMethods()
									.FirstOrDefault(orderByMatch)
									.MakeGenericMethod(typeOfT, propertyInfo.PropertyType);

			return (IQueryable<T>)orderByMethodInfo.Invoke(null, new[] { source, orderByLambda });
		}
	}
}
