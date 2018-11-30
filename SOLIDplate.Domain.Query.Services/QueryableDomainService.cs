using SOLIDplate.Common;
using SOLIDplate.Common.Portable;
using SOLIDplate.Domain.Entities;
using SOLIDplate.Domain.Query.Services.Interfaces;
using SOLIDplate.Infrastructure.Data.Interfaces;
using SOLIDplate.Infrastructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SOLIDplate.Domain.Query.Services
{
    public abstract class QueryableDomainService<TEntity> : DomainService<TEntity, IUnitOfWork, IEntityRepository<TEntity>>, IQueryableDomainService<TEntity>
          where TEntity : class, new()
    {
        private readonly IEntityQueryService _entityQueryService;
        protected QueryableDomainService(IUnitOfWork unitOfWork, IEntityRepository<TEntity> entityRepository, IEntityQueryService entityQueryService)
            : base(unitOfWork, entityRepository)
        {
            _entityQueryService = entityQueryService;
        }

        public abstract IEnumerable<TEntity> ExecuteQuery(int queryId);

        protected virtual Expression<Func<TEntity, bool>> GeneratePredicateExpression(int entityQueryId) => GeneratePredicateExpression(_entityQueryService.Get(entityQueryId, typeof(TEntity)));

        private static Expression<Func<TEntity, bool>> GeneratePredicateExpression(EntityQuery entityQuery)
        {
            var entityPropertyFilters = entityQuery.EntityPropertyFilters.OrderBy(epf => epf.Id);
            var firstEntityPropertyFilter = entityPropertyFilters.First(epf => epf.LogicalOperator == LogicalOperator.None);
            var restOfEntityPropertyFilters = entityPropertyFilters.Where(epf => epf.LogicalOperator != LogicalOperator.None);

            var expressionBody = Express(firstEntityPropertyFilter);

            foreach (var entityPropertyFilter in restOfEntityPropertyFilters)
            {
                var nextBody = Express(entityPropertyFilter);

                if (entityPropertyFilter.LogicalOperator == LogicalOperator.And)
                {
                    expressionBody = expressionBody.AndAlso(nextBody);
                }

                if (entityPropertyFilter.LogicalOperator == LogicalOperator.Or)
                {
                    expressionBody = expressionBody.OrElse(nextBody);
                }
            }
            return expressionBody;
        }

        private static Expression<Func<TEntity, bool>> Express(EntityPropertyFilter entityPropertyFilter)
        {
            var entityParameter = Expression.Parameter(typeof(TEntity), typeof(TEntity).Name.ToCamelCase());
            var entityPropertyExpression = NestedExpressionProperty(entityParameter, entityPropertyFilter.Name);

            if (entityPropertyFilter.Values == null)
            {
                var message = $"Values property cannot be null for PropertyFilter with Id: '{entityPropertyFilter.Id}'";
                throw new ArgumentException(message);
            }
            if (!entityPropertyFilter.Values.Any())
            {
                var message = $"Values property cannot be empty for PropertyFilter with Id: '{entityPropertyFilter.Id}'";
                throw new ArgumentException(message);
            }

            ConstantExpression valuesExpression;
            ConstantExpression firstValueExpression;
            ConstantExpression secondValueExpression;

            if (entityPropertyFilter.ValueType == typeof(int))
            {
                var values = entityPropertyFilter.Values.ConvertAllToInt().ToArray();
                valuesExpression = Expression.Constant(values, values.GetType());
                firstValueExpression = Expression.Constant(values.First(), entityPropertyFilter.ValueType);
                secondValueExpression = (values.Length > 1) ? Expression.Constant(values.Skip(1).First(), entityPropertyFilter.ValueType) : null;
            }
            else if (entityPropertyFilter.ValueType == typeof(DateTime))
            {
                var values = entityPropertyFilter.Values.ConvertAllToDateTime().ToArray();
                valuesExpression = Expression.Constant(values, values.GetType());
                firstValueExpression = Expression.Constant(values.First(), entityPropertyFilter.ValueType);
                secondValueExpression = (values.Length > 1) ? Expression.Constant(values.Skip(1).First(), entityPropertyFilter.ValueType) : null;
            }
            else if (entityPropertyFilter.ValueType == typeof(double))
            {
                var values = entityPropertyFilter.Values.ConvertAllToDouble().ToArray();
                valuesExpression = Expression.Constant(values, values.GetType());
                firstValueExpression = Expression.Constant(values.First(), entityPropertyFilter.ValueType);
                secondValueExpression = (values.Length > 1) ? Expression.Constant(values.Skip(1).First(), entityPropertyFilter.ValueType) : null;
            }
            else if (entityPropertyFilter.ValueType == typeof(string))
            {
                valuesExpression = Expression.Constant(entityPropertyFilter.Values, entityPropertyFilter.Values.GetType());
                firstValueExpression = Expression.Constant(entityPropertyFilter.Values.First(), entityPropertyFilter.ValueType);
                secondValueExpression = (entityPropertyFilter.Values.Count > 1) ? Expression.Constant(entityPropertyFilter.Values.Skip(1).First(), entityPropertyFilter.ValueType) : null;
            }
            else
            {
                var message = $"An implementation for ValueType of '{entityPropertyFilter.ValueType}' has not been found for PropertyFilter with Id: '{entityPropertyFilter.Id}'.";
                throw new NotImplementedException(message);
            }

            var stringStartsWithMethodInfo = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
            var stringContainsMethodInfo = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            var stringEndsWithMethodInfo = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
            var enumerableContainsMethodInfo = typeof(Enumerable).GetMethods(BindingFlags.Public | BindingFlags.Static)
                                                                 .First(m => m.Name == "Contains" &&
                                                                             m.GetParameters().Length == 2)
                                                                 .MakeGenericMethod(entityPropertyFilter.ValueType);

            var noneExpression = Expression.Constant(true);
            var defaultExpression = Expression.Constant(false);
            var arguments = new Expression[] { firstValueExpression };
            var startsWithExpression = entityPropertyFilter.ValueType == typeof(string) ? Expression.Call(entityPropertyExpression, stringStartsWithMethodInfo, arguments) : null;
            var containsExpression = entityPropertyFilter.ValueType == typeof(string) ? Expression.Call(entityPropertyExpression, stringContainsMethodInfo, arguments) : null;
            var endsWithExpression = entityPropertyFilter.ValueType == typeof(string) ? Expression.Call(entityPropertyExpression, stringEndsWithMethodInfo, arguments) : null;
            var inExpression = Expression.Call(enumerableContainsMethodInfo, new Expression[] { valuesExpression, entityPropertyExpression });
            var equalsExpression = Expression.Equal(entityPropertyExpression, firstValueExpression);
            BinaryExpression greaterThanExpression = null;
            BinaryExpression lessThanExpression = null;
            BinaryExpression betweenExpression = null;

            if (entityPropertyFilter.ValueType != typeof(string))
            {
                greaterThanExpression = Expression.GreaterThan(entityPropertyExpression, firstValueExpression);
                lessThanExpression = Expression.LessThan(entityPropertyExpression, firstValueExpression);
                if (secondValueExpression != null)
                {
                    betweenExpression = Expression.And(greaterThanExpression, Expression.LessThan(entityPropertyExpression, secondValueExpression));
                }
            }

            switch (entityPropertyFilter.ComparisonOperator)
            {
                case ComparisonOperator.None:
                    return Expression.Lambda<Func<TEntity, bool>>(noneExpression, entityParameter);
                case ComparisonOperator.In:
                    return Expression.Lambda<Func<TEntity, bool>>(inExpression, entityParameter);
                case ComparisonOperator.NotIn:
                    return Expression.Lambda<Func<TEntity, bool>>(Expression.Not(inExpression), entityParameter);
                case ComparisonOperator.Between:
                    return Expression.Lambda<Func<TEntity, bool>>(betweenExpression, entityParameter);
                case ComparisonOperator.StartsWith:
                    return Expression.Lambda<Func<TEntity, bool>>(startsWithExpression, entityParameter);
                case ComparisonOperator.Contains:
                    return Expression.Lambda<Func<TEntity, bool>>(containsExpression, entityParameter);
                case ComparisonOperator.EndsWith:
                    return Expression.Lambda<Func<TEntity, bool>>(endsWithExpression, entityParameter);
                case ComparisonOperator.Equals:
                    return Expression.Lambda<Func<TEntity, bool>>(equalsExpression, entityParameter);
                case ComparisonOperator.NotEquals:
                    return Expression.Lambda<Func<TEntity, bool>>(Expression.Not(equalsExpression), entityParameter);
                case ComparisonOperator.GreaterThan:
                    return Expression.Lambda<Func<TEntity, bool>>(greaterThanExpression, entityParameter);
                case ComparisonOperator.LessThan:
                    return Expression.Lambda<Func<TEntity, bool>>(lessThanExpression, entityParameter);
                default:
                    return Expression.Lambda<Func<TEntity, bool>>(defaultExpression, entityParameter);
            }
        }

        private static MemberExpression NestedExpressionProperty(Expression expression, string propertyName)
        {
            var propertyParts = propertyName.Split('.');
            var numberOfPropertyParts = propertyParts.Length;

            if (numberOfPropertyParts > 1)
            {
                var name = propertyParts.Take(numberOfPropertyParts - 1).Aggregate((a, i) => a + "." + i);
                var memberExpression = NestedExpressionProperty(expression, name);
                return Expression.Property(memberExpression, propertyParts[numberOfPropertyParts - 1]);
            }

            return Expression.Property(expression, propertyName);
        }
    }
}
