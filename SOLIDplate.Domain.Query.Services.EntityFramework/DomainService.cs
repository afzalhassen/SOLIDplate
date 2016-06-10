using SOLIDplate.Domain.Query.Services.Interfaces;
using SOLIDplate.Infrastructure.Data.EntityFramework.Interfaces;
using SOLIDplate.Infrastructure.Repository.EntityFramework.Interfaces;
using System;
using System.Linq.Expressions;

namespace SOLIDplate.Domain.Query.Services.EntityFramework
{
    public abstract class DomainService<TEntity> : DomainService<TEntity, IUnitOfWork, IEntityRepository<TEntity>>
          where TEntity : class, new()
    {
        private readonly IEntityQueryService _entityQueryService;
        protected DomainService(IUnitOfWork unitOfWork, IEntityRepository<TEntity> entityRepository, IEntityQueryService entityQueryService)
            : base(unitOfWork, entityRepository)
        {
            _entityQueryService = entityQueryService;
        }

        protected virtual Expression<Func<TEntity, bool>> GeneratePredicateExpression(int entityQueryId)
        {
            var xx = _entityQueryService.Get(entityQueryId, typeof(TEntity));
            return base.GeneratePredicateExpression(xx);
        }
    }
}
