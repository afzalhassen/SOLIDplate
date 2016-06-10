using SOLIDplate.Domain.Entities;
using SOLIDplate.Domain.Query.Services.Interfaces;
using SOLIDplate.Infrastructure.Data.EntityFramework.Interfaces;
using SOLIDplate.Infrastructure.Repository.EntityFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SOLIDplate.Domain.Query.Services.EntityFramework
{
    public class EntityQueryService : DomainService<EntityQuery>, IEntityQueryService
    {
        public EntityQueryService(IUnitOfWork unitOfWork, IEntityRepository<EntityQuery> entityRepository)
            : base(unitOfWork, entityRepository, null)
        {
        }

        public override IQueryable<EntityQuery> Get()
        {
            return Repository.AllUnwrapped()
                             .Include(entity => entity.EntityQueryCategory)
                             .Include(entity => entity.EntityPropertyFilters);
        }

        public override EntityQuery Get(int id)
        {
            var result = Repository.One(entity => entity.Id == id);
            UnitOfWork.Entry(result).Reference(entity => entity.EntityQueryCategory).Load();
            UnitOfWork.Entry(result).Collection(entity => entity.EntityPropertyFilters).Load();

            return result;
        }
        public EntityQuery Get(int id, Type type)
        {
            var result = Repository.One(entity => entity.Id == id &&
                                                  entity.EntityTypeSerialised.Equals(type.ToString(), StringComparison.InvariantCulture));
            UnitOfWork.Entry(result).Reference(entity => entity.EntityQueryCategory).Load();
            UnitOfWork.Entry(result).Collection(entity => entity.EntityPropertyFilters).Load();

            return result;
        }

        public IQueryable<EntityQuery> Get(Type entityType)
        {
            var result = Get().Where(entity => entity.EntityType == entityType);

            return result;
        }

        public IQueryable<EntityQuery> Get(Type entityType, int entityQueryCategoryId)
        {
            var result = Get(entityType).Where(entity => entity.EntityQueryCategoryId == entityQueryCategoryId);

            return result;
        }



        public override IQueryable<EntityQuery> ExecuteQuery(int queryId)
        {
            throw new NotSupportedException($"{nameof(ExecuteQuery)} operations are not supprted for the type: {EntityTypeName}");
        }
        protected override Expression<Func<EntityQuery, bool>> GeneratePredicateExpression(int entityQueryId)
        {
            throw new NotSupportedException($"{nameof(GeneratePredicateExpression)} operations are not supprted for the type: {EntityTypeName}");
        }

        public IEnumerable<Type> GetQueryableEntityTypes()
        {
            var baseType = typeof(Entity<>);
            // TODO: Add any extra type(s) to exclude from being available to Query to this list
            var typeExclusions = new List<Type>
                                 {
                                     typeof(Person),
                                     typeof(EntityQuery),
                                     typeof(EntityPropertyFilter),
                                     typeof(EntityAudit)
                                 };

            Func<Type, bool> predicate = type => !typeExclusions.Select(t => t.Name).Contains(type.Name) &&
                                                 type.IsClass &&
                                                 type.IsAbstract == false &&
                                                 type.BaseType != null &&
                                                 type.BaseType.Name != typeof(LookupEntity<>).Name &&
                                                 type.BaseType.Name != typeof(IntegrationEntity<,>).Name;

            var result = Assembly.GetAssembly(baseType).GetTypes().Where(predicate);
            return result;
        }

        public IEnumerable<EntityPropertyFilter> GetQueryableEntityTypePropertyFilters(Type queryableEntityType)
        {
            var queryableEntityTypes = GetQueryableEntityTypes();

            if (queryableEntityTypes.All(type => type != queryableEntityType))
            {
                throw new NotSupportedException($"The type '{queryableEntityType.FullName}' is not a supported queryable type of the '{GetType().Name}' domain service.");
            }

            var propertyInfos = queryableEntityType.GetProperties();

            var result = GenerateEntityPropertyFilters(propertyInfos, string.Empty);

            return result;
        }

        private static IEnumerable<EntityPropertyFilter> GenerateEntityPropertyFilters(ICollection<PropertyInfo> propertyInfos, string parentPropertyName)
        {
            var result = new List<EntityPropertyFilter>();
            var domainEntitiesNameSpace = typeof(Entity<>).Namespace;
            var nativeProperties = propertyInfos.Where(p => p.PropertyType.Namespace != domainEntitiesNameSpace &&
                                                            !p.Name.EndsWith("Id"));
            foreach (var propertyInfo in nativeProperties)
            {
                var propertyName = !string.IsNullOrEmpty(parentPropertyName)
                                       ? $"{parentPropertyName}.{propertyInfo.Name}"
                                       : propertyInfo.Name;
                var entityPropertyFilter = new EntityPropertyFilter
                {
                    Name = propertyName,
                    ValueType = propertyInfo.PropertyType
                };

                result.Add(entityPropertyFilter);
            }

            var entityProperties = propertyInfos.Where(propertyInfo => propertyInfo.PropertyType.Namespace == domainEntitiesNameSpace &&
                                                                       !propertyInfo.Name.EndsWith("Id"));
            foreach (var propertyInfo in entityProperties)
            {
                var nextParentPropertyName = !string.IsNullOrEmpty(parentPropertyName)
                                                 ? $"{parentPropertyName}.{propertyInfo.Name}"
                                                 : propertyInfo.Name;
                var nextPropertyInfos = propertyInfo.PropertyType.GetProperties();
                var nextResult = GenerateEntityPropertyFilters(nextPropertyInfos, nextParentPropertyName);

                result.AddRange(nextResult);
            }

            return result;
        }

        public IQueryable<EntityAudit> GetEntityAudits(int entityId)
        {
            throw new NotImplementedException();
        }
    }
}