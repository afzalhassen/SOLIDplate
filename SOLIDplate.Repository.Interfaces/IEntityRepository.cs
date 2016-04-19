using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SOLIDplate.Repository.Interfaces
{
	public interface IEntityRepository<TEntity>
		where TEntity : class, new()
	{
		/// <summary>
		/// Adds the specified instance of TEntity.
		/// </summary>
		/// <param name="entity">
		/// Instance of TEntity.
		/// </param>
		void Add(TEntity entity);

		/// <summary>
		/// Adds the specified instances of TEntities.
		/// </summary>
		/// <param name="entities">
		/// Instances of TEntity.
		/// </param>
		void Add(IEnumerable<TEntity> entities);

		/// <summary>
		/// All instances of TEntity.
		/// </summary>
		/// <returns>All instances of TEntity</returns>
		IQueryable<TEntity> All();

		/// <summary>
		/// All instances of TEntity matching search.
		/// </summary>
		/// <param name="predicate">
		/// The predicate expression to filter instances.
		/// </param>
		/// <returns>
		/// All matching instances of type TEntity
		/// </returns>
		IQueryable<TEntity> All(Expression<Func<TEntity, bool>> predicate);

		/// <summary>
		/// Paged All based on a predicate.
		/// </summary>
		/// <param name="predicate">The predicate.</param>
		/// <param name="pageSize">Number of records per page.</param>
		/// <param name="pageIndex">)Based index of the page.</param>
		/// <returns>Collection per page</returns>
		IQueryable<TEntity> All(Expression<Func<TEntity, bool>> predicate, int pageSize, int pageIndex);

		/// <summary>
		/// Paged All based on a predicate with sorting.
		/// </summary>
		/// <param name="predicate">The predicate.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <param name="pageIndex">Index of the page.</param>
		/// <param name="orderingField">The order field.</param>
		/// <param name="orderingDirection">The order direction.</param>
		/// <returns>
		/// Collection per page
		/// </returns>
		IQueryable<TEntity> All(Expression<Func<TEntity, bool>> predicate, int pageSize, int pageIndex, string orderingField, string orderingDirection);

		/// <summary>
		/// Find specific instance of TEntity for a table which will always only have maximum of 1 record.
		/// </summary>
		/// <returns>
		/// Only record else null
		/// </returns>
		TEntity One();

		/// <summary>
		/// Find specific instance of TEntity.
		/// </summary>
		/// <param name="predicate">
		/// The predicate expression to find first match.
		/// </param>
		/// <returns>
		/// First instance matching predicate else null
		/// </returns>
		TEntity One(Expression<Func<TEntity, bool>> predicate);

		/// <summary>
		/// Total number of entities.
		/// </summary>
		/// <returns>Total Count</returns>
		int Count();

		/// <summary>
		/// Counts the specified predicate.
		/// </summary>
		/// <param name="predicate">The predicate.</param>
		/// <returns>Count of the predicate</returns>
		int Count(Expression<Func<TEntity, bool>> predicate);

		/// <summary>
		/// Deletes all instances of type TEntity.
		/// </summary>
		void Delete();

		/// <summary>
		/// Deletes the specified TEntity.
		/// </summary>
		/// <param name="entity">
		/// Instance of TEntity.
		/// </param>
		void Delete(TEntity entity);

		/// <summary>
		/// Deletes the specified instances of type TEntity.
		/// </summary>
		/// <param name="entities">
		/// TEntities to delete.
		/// </param>
		void Delete(IEnumerable<TEntity> entities);

		/// <summary>
		/// Deletes all matching TEntities.
		/// </summary>
		/// <param name="predicate">
		/// Predicate to match specific instances of TEntity.
		/// </param>
		void Delete(Expression<Func<TEntity, bool>> predicate);

		/// <summary>
		/// All instances of TEntity.
		/// </summary>
		/// <returns>All instances of TEntity</returns>
		IQueryable<TEntity> AllUnwrapped();

		/// <summary>
		/// All instances of TEntity matching search.
		/// </summary>
		/// <param name="predicate">
		/// The predicate expression to filter instances.
		/// </param>
		/// <returns>
		/// All matching instances of type TEntity
		/// </returns>
		IQueryable<TEntity> AllUnwrapped(Expression<Func<TEntity, bool>> predicate);

		/// <summary>
		/// Paged All based on a predicate.
		/// </summary>
		/// <param name="predicate">The predicate.</param>
		/// <param name="pageSize">Number of records per page.</param>
		/// <param name="pageIndex">)Based index of the page.</param>
		/// <returns>Collection per page</returns>
		IQueryable<TEntity> AllUnwrapped(Expression<Func<TEntity, bool>> predicate, int pageSize, int pageIndex);

		/// <summary>
		/// Paged All based on a predicate with sorting.
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
	}
}
