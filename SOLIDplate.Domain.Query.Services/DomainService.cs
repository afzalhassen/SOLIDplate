using SOLIDplate.Domain.Query.Services.Interfaces;
using SOLIDplate.Infrastructure.Repository.Interfaces;
using System.Collections.Generic;

namespace SOLIDplate.Domain.Query.Services
{
    public abstract class DomainService<TEntity, TUnitOfWork, TEntityRepository> : IDomainService<TEntity>
         where TEntity : class, new()
        where TEntityRepository : IEntityRepository<TEntity>
    {
        protected TUnitOfWork UnitOfWork { get; private set; }
        protected TEntityRepository Repository { get; private set; }
        protected string EntityTypeName => typeof(TEntity).Name;

        protected DomainService(TUnitOfWork unitOfWork, TEntityRepository entityRepository)
        {
            UnitOfWork = unitOfWork;
            Repository = entityRepository;
        }

        public abstract IEnumerable<TEntity> Get();
        public abstract TEntity Get(int id);
    }
}
