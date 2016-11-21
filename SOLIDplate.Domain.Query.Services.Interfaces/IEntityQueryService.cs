using SOLIDplate.Domain.Entities;
using System;
using System.Collections.Generic;

namespace SOLIDplate.Domain.Query.Services.Interfaces
{
    public interface IEntityQueryService : IDomainService<EntityQuery>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        EntityQuery Get(int id, Type type);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        IEnumerable<EntityQuery> Get(Type entityType);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityType"></param>
        /// <param name="entityQueryCategoryId"></param>
        /// <returns></returns>
        IEnumerable<EntityQuery> Get(Type entityType, int entityQueryCategoryId);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        IEnumerable<EntityAudit> GetEntityAudits(int entityId);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<Type> GetQueryableEntityTypes();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryableEntityType"></param>
        /// <returns></returns>
        IEnumerable<EntityPropertyFilter> GetQueryableEntityTypePropertyFilters(Type queryableEntityType);
    }
}