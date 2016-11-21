using System.Collections.Generic;

namespace SOLIDplate.Domain.Query.Services.Interfaces
{
    public interface IQueryableDomainService<out TEntity> : IDomainService<TEntity>
        where TEntity : class, new()
    {

        /// <summary>
        /// Executes a stored user-composed query on the underlying repository's data-context.
        /// </summary>
        /// <param name="queryId">the Id of the stored EntityQuery to execute.</param>
        /// <returns>An enumerable collection of TEntity</returns>
        IEnumerable<TEntity> ExecuteQuery(int queryId);
    }
}