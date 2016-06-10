namespace SOLIDplate.Domain.Command.Services.Interfaces
{
    public interface IDomainService<in TEntity>
         where TEntity : class, new()
    {
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
