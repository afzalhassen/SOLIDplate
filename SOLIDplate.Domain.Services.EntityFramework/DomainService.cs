using SOLIDplate.Data.EntityFramework.Interfaces;
using SOLIDplate.Domain.Entities;
using SOLIDplate.Repository.EntityFramework.Interfaces;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace SOLIDplate.Domain.Services.EntityFramework
{
    public abstract class DomainService<TEntity> : DomainService<TEntity, IUnitOfWork, IEntityRepository<TEntity>>
          where TEntity : class, new()
    {
        protected DomainService(IUnitOfWork unitOfWork, IEntityRepository<TEntity> entityRepository)
            : base(unitOfWork, entityRepository)
        {
        }

        protected Expression<Func<TEntity, bool>> GeneratePredicateExpression(int entityQueryId)
        {
            var entityTypeString = typeof(TEntity).ToString();
            var results = UnitOfWork.All<EntityQuery>()
                                        .Include(eq => eq.EntityPropertyFilters)
                                        .Where(eq => eq.Id == entityQueryId &&
                                                    eq.EntityTypeSerialised.Equals(entityTypeString, StringComparison.InvariantCulture));
            return base.GeneratePredicateExpression(results.First());
        }
    }
}
