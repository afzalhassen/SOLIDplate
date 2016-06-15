using System.Linq;

namespace SOLIDplate.Domain.Query.Services.Interfaces
{
    public interface IQueryableDomainService<out TEntity> : IDomainService<TEntity>
        where TEntity : class, new()
    {

        /// <summary>
        /// Executes a stored user-composed query on the underlying repository's data-context.
        /// </summary>
        /// <param name="queryId">the Id of the stored EntityQuery to execute.</param>
        /// <returns>A queryable collection of TEntity</returns>
        IQueryable<TEntity> ExecuteQuery(int queryId);
    }
}