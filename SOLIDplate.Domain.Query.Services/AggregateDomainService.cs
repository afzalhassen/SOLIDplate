using System.Collections.Generic;
using SOLIDplate.Domain.Query.Services.Interfaces;

namespace SOLIDplate.Domain.Query.Services
{
    public abstract class AggregateDomainService<TEntity> : IAggregateDomainService<TEntity>
        where TEntity : class, new()
    {
        protected string EntityTypeName => typeof(TEntity).Name;

        public abstract IEnumerable<TEntity> Get();

        public abstract TEntity Get(int id);
    }
}