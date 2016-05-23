using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace SOLIDplate.Data.EntityFramework.Interfaces
{
    public interface IDatabaseContext : IDisposable
    {
        bool EnableAuditing { get; set; }
        /// <summary>
        /// Gets the database instance of the context.
        /// </summary>
        Database Database { get; }

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
