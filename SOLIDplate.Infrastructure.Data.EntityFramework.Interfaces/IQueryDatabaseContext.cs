using System.Data.Entity;

namespace SOLIDplate.Infrastructure.Data.EntityFramework.Interfaces
{
    public interface IQueryDatabaseContext : IDatabaseContext
    {
        /// <summary>
        /// Collation of IDbSet
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns>IDbSet of TEntity</returns>
        DbSet<TEntity> EntitySet<TEntity>() where TEntity : class, new();
    }
}