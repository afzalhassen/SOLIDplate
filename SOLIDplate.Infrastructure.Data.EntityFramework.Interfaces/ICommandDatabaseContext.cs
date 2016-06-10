namespace SOLIDplate.Infrastructure.Data.EntityFramework.Interfaces
{
    public interface ICommandDatabaseContext : IDatabaseContext
    {
        /// <summary>
        /// Gets a value indicating whether there are uncommitted changes with the context.
        /// </summary>
        /// <returns>
        /// <c>true</c> if this instance is dirty; otherwise, <c>false</c>.
        /// </returns>
        bool IsDirty();

        /// <summary>
        /// Saves all changes made in this context to the underlying database.
        /// </summary>
        /// <returns>The number of objects written to the underlying database</returns>
        int SaveChanges();

    }
}