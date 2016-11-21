using System.Collections.Generic;

namespace SOLIDplate.Domain.Services.Interfaces
{
    public interface IDomainService<TEntity>
         where TEntity : class, new()
    {
        /// <summary>
        /// Gets a queryable collection of TEntity
        /// </summary>
        /// <returns>An enumerable collection of TEntity</returns>
        IEnumerable<TEntity> Get();
        /// <summary>
        /// Gets an instance of TEntity.
        /// </summary>
        /// <param name="id">An integer value that will be used to check for equality against the TEntity.Id property value.</param>
        /// <returns>An instance of TEntity whose Id property value is euqal to the value of <see cref="id"/></returns>
        TEntity Get(int id);
        /// <summary>
        /// Persists the provided TEntity instance to the underlying repository's data-context as a new entry.
        /// </summary>
        /// <param name="entityToAdd">The instance of TEntity to add to the underlying data-context as a new entry</param>
        void Add(TEntity entityToAdd);
        /// <summary>
        /// Persists the provided TEntity instance's updates to the underlying repository's data-context entry.
        /// </summary>
        /// <param name="entityWithUpdates">An instance of TEntity containing the updates that will be applied to the underlying data-context entry.</param>
        void Update(TEntity entityWithUpdates);
        /// <summary>
        /// Deletes the TEntity from the underlying repository's data-context where the TEntity.Id equals the value of <see cref="id"/>
        /// </summary>
        /// <param name="id">An integer value that will be used to check for equality against the TEntity.Id property value.</param>
        void Delete(int id);
    }
}
