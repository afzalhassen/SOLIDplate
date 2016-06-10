using SOLIDplate.Infrastructure.Data.EntityFramework.Interfaces;
using SOLIDplate.Infrastructure.Repository.EntityFramework.Interfaces;

namespace SOLIDplate.Infrastructure.Repository.EntityFramework
{
    public class CompositeEntityRepository<TCompositeEntity, TIdentityEntity1, TIdentityEntity2> : EntityRepository<TCompositeEntity>, ICompositeEntityRepository<TCompositeEntity, TIdentityEntity1, TIdentityEntity2>
        where TIdentityEntity1 : class, new()
        where TIdentityEntity2 : class, new()
        where TCompositeEntity : class, new()
    {
        public CompositeEntityRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }

    public class CompositeEntityRepository<TCompositeEntity, TIdentityEntity1, TIdentityEntity2, TIdentityEntity3> : EntityRepository<TCompositeEntity>, ICompositeEntityRepository<TCompositeEntity, TIdentityEntity1, TIdentityEntity2, TIdentityEntity3>
        where TIdentityEntity1 : class, new()
        where TIdentityEntity2 : class, new()
        where TIdentityEntity3 : class, new()
        where TCompositeEntity : class, new()
    {
        public CompositeEntityRepository(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}