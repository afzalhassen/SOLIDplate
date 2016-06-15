using SOLIDplate.Infrastructure.Data.EntityFramework.Interfaces;
using SOLIDplate.Infrastructure.Repository.EntityFramework.Interfaces;

namespace SOLIDplate.Domain.Services.EntityFramework
{
    public abstract class DomainService<TEntity> : DomainService<TEntity, IUnitOfWork, IEntityRepository<TEntity>>
        where TEntity : class, new()
    {
        protected DomainService(IUnitOfWork unitOfWork, IEntityRepository<TEntity> entityRepository)
            : base(unitOfWork, entityRepository)
        {
        }
    }
}