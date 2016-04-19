using SOLIDplate.Data.EntityFramework.Interfaces;
using SOLIDplate.Repository.EntityFramework.Interfaces;
using System.Data.Entity;
using System.Linq;
//using System.Linq.Dynamic;

namespace SOLIDplate.Repository.EntityFramework
{
	public class EntityRepository<TEntity> : EntityRepository<TEntity, IUnitOfWork>, IEntityRepository<TEntity>
		where TEntity : class, new()
	{
		public EntityRepository(IUnitOfWork unitOfWork)
			: base(unitOfWork)
		{
		}

		//public override IQueryable<TEntity> All(Expression<Func<TEntity, bool>> predicate, string orderingField, string orderingDirection, int pageSize, int pageIndex)
		//{
		//	var orderClause = FormatOrderClause(orderingField, orderingDirection);
		//	return All(predicate).OrderBy(orderClause).Skip(pageSize * pageIndex).Take(pageSize);
		//}

		public override IQueryable<TEntity> AllUnwrapped()
		{
			return All().AsNoTracking();
		}

		//public override IQueryable<TEntity> AllUnwrapped(Expression<Func<TEntity, bool>> predicate, string orderingField, string orderingDirection, int pageSize, int pageIndex)
		//{
		//	var orderClause = FormatOrderClause(orderingField, orderingDirection);
		//	return AllUnwrapped(predicate).OrderBy(orderClause).Skip(pageSize * pageIndex).Take(pageSize);
		//}

		public void Attach(TEntity entity)
		{
			UnitOfWork.ThreadSafeDatabaseContext.EntitySet<TEntity>().Attach(entity);
		}
	}
}
