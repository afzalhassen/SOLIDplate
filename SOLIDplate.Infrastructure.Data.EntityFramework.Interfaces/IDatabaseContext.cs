using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace SOLIDplate.Infrastructure.Data.EntityFramework.Interfaces
{
    public interface IDatabaseContext : IDisposable
    {
        /// <summary>
        /// Runs the the registered System.Data.Entity.IDatabaseInitializer`1 on this context.
        /// If "force" is set to true, then the initializer is run regardless of whether
        /// or not it has been run before. This can be useful if a database is deleted while
        /// an app is running and needs to be reinitialized. If "force" is set to false,
        /// then the initializer is only run if it has not already been run for this context,
        /// model, and connection in this app domain. This method is typically used when
        /// it is necessary to ensure that the database has been created and seeded before
        /// starting some operation where doing so lazily will cause issues, such as when
        /// the operation is part of a transaction.
        /// </summary>
        /// <param name="force">
        /// If set to true the initializer is run even if it has already been run.
        /// </param>
        void Initialize(bool force);
        /// <summary>
        /// Checks whether or not the database exists on the server.
        /// </summary>
        /// <returns>
        /// True if the database exists; false otherwise.
        /// </returns>
        bool Exists();
        /// <summary>
        /// Gets a value indicating whether there are uncommitted changes with the context.
        /// </summary>
        /// <returns>
        /// <c>true</c> if this instance is dirty; otherwise, <c>false</c>.
        /// </returns>
        bool IsDirty();

        /// <summary>
        /// Collation of IDbSet
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>IDbSet of TEntity</returns>
        DbSet<TEntity> EntitySet<TEntity>() where TEntity : class, new();

        /// <summary>
        /// Gets a System.Data.Entity.Infrastructure.DbEntityEntry&lt;TEntity&gt; object for the given entity providing access to information about the entity and the ability to perform actions on the entity.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="entity"></param>
        /// <returns>An entry for the entity.</returns>
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class, new();

        /// <summary>
        /// Saves all changes made in this context to the underlying database.
        /// </summary>
        /// <returns>The number of objects written to the underlying database</returns>
        int SaveChanges();

        /// <summary>
        /// Detaches all.
        /// </summary>
        void DetachAll();
    }
}
