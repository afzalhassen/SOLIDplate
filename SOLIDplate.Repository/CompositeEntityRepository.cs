using SOLIDplate.Data.Interfaces;
using SOLIDplate.Repository.Interfaces;

namespace SOLIDplate.Repository
{
	public abstract class CompositeEntityRepository<TCompositeEntity, TIdentityEntity1, TIdentityEntity2, TUnitOfWork> : EntityRepository<TCompositeEntity, TUnitOfWork>, ICompositeEntityRepository<TCompositeEntity, TIdentityEntity1, TIdentityEntity2>
		where TIdentityEntity1 : class, new()
		where TIdentityEntity2 : class, new()
		where TCompositeEntity : class, new()
		where TUnitOfWork : IUnitOfWork
	{
		protected CompositeEntityRepository(TUnitOfWork unitOfWork)
			: base(unitOfWork)
		{
		}
	}

	public abstract class CompositeEntityRepository<TCompositeEntity, TIdentityEntity1, TIdentityEntity2, TIdentityEntity3, TUnitOfWork> : EntityRepository<TCompositeEntity, TUnitOfWork>, ICompositeEntityRepository<TCompositeEntity, TIdentityEntity1, TIdentityEntity2, TIdentityEntity3>
		where TIdentityEntity1 : class, new()
		where TIdentityEntity2 : class, new()
		where TIdentityEntity3 : class, new()
		where TCompositeEntity : class, new()
		where TUnitOfWork : IUnitOfWork
	{
		protected CompositeEntityRepository(TUnitOfWork unitOfWork)
			: base(unitOfWork)
		{
		}
	}
}