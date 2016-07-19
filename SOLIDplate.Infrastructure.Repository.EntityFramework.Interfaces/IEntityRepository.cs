using System;
using System.Linq;
using System.Linq.Expressions;

namespace SOLIDplate.Infrastructure.Repository.EntityFramework.Interfaces
{
    public interface IEntityRepository<TEntity> : Repository.Interfaces.IEntityRepository<TEntity>
        where TEntity : class, new()
    {
        /// <summary>
        /// All instances of TEntity with no caching on the underlying DbContext.
        /// </summary>
        /// <returns>All instances of TEntity</returns>
        IQueryable<TEntity> AllUnwrapped();

        /// <summary>
        /// All instances of TEntity matching search with no caching on the underlying DbContext.
        /// </summary>
        /// <param name="predicate">
        /// The predicate expression to filter instances.
        /// </param>
        /// <returns>
        /// All matching instances of type TEntity
        /// </returns>
        IQueryable<TEntity> AllUnwrapped(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Paged All based on a predicate with no caching on the underlying DbContext.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="pageSize">Number of records per page.</param>
        /// <param name="pageIndex">)Based index of the page.</param>
        /// <returns>Collection per page</returns>
        IQueryable<TEntity> AllUnwrapped(Expression<Func<TEntity, bool>> predicate, int pageSize, int pageIndex);

        /// <summary>
        /// Paged All based on a predicate with sorting with no caching on the underlying DbContext.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="orderingField">The order field.</param>
        /// <param name="orderingDirection">The order direction.</param>
        /// <returns>
        /// Collection per page
        /// </returns>
        IQueryable<TEntity> AllUnwrapped(Expression<Func<TEntity, bool>> predicate, int pageSize, int pageIndex, string orderingField, string orderingDirection);
        /// <summary>
        /// Attaches the instance of TEntity to the current UnitOfWork DbContext
        /// </summary>
        /// <param name="entity">The instance of TEntity to attach</param>
        void Attach(TEntity entity);
    }
}
