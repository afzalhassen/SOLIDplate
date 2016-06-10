using System.Data.Entity.Infrastructure;

namespace SOLIDplate.Infrastructure.Data.EntityFramework.Interfaces
{
    public interface IUnitOfWork : Data.Interfaces.IUnitOfWork
    {
        IDatabaseContext ThreadSafeDatabaseContext { get; }
        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class, new();
    }
}