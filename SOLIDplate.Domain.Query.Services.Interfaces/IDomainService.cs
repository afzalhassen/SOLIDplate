using System;
using System.Linq;
using System.Linq.Expressions;

namespace SOLIDplate.Domain.Query.Services.Interfaces
{
    public interface IDomainService<TEntity>: IAggregateDomainService<TEntity>
         where TEntity : class, new()
    {
        /// <summary>
        /// Gets a queryable collection of TEntity
        /// </summary>
        /// <returns>A queryable collection of TEntity</returns>
        IQueryable<TEntity> GetQueryable();
        /// <summary>
        /// Gets a queryable collection of TEntity
        /// </summary>
        /// <param name="predicate">The expression predicate to apply</param>
        /// <returns>A queryable collection of TEntity having applied the <paramref name="predicate"/>.</returns>
        IQueryable<TEntity> GetQueryable(Expression<Func<TEntity, bool>> predicate);
    }
}
