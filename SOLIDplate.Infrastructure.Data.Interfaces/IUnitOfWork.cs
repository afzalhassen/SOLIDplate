using System;
using System.Collections.Generic;
using System.Linq;

namespace SOLIDplate.Infrastructure.Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        void SaveChanges();
        void DiscardChanges();
        bool IsDirty();
        void Add<TEntity>(TEntity entity) where TEntity : class, new();
        void AddRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class, new();
        IQueryable<TEntity> All<TEntity>() where TEntity : class, new();
        void Delete<TEntity>(TEntity entity) where TEntity : class, new();
    }
}
