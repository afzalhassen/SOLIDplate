using SOLIDplate.Domain.Entities;
using SOLIDplate.Domain.Services.Interfaces;
using SOLIDplate.Infrastructure.Data.EntityFramework.Interfaces;
using SOLIDplate.Infrastructure.Repository.EntityFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;

namespace SOLIDplate.Domain.Services.EntityFramework
{
    public class EntityQueryService : DomainService<EntityQuery, IUnitOfWork, IEntityRepository<EntityQuery>>, IEntityQueryService
    {
        public EntityQueryService(IUnitOfWork unitOfWork, IEntityRepository<EntityQuery> entityRepository)
            : base(unitOfWork, entityRepository)
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

        public IQueryable<EntityQuery> Get(Type entityType) => Get().Where(entity => entity.EntityType == entityType);

        public IQueryable<EntityQuery> Get(Type entityType, int entityQueryCategoryId) => Get(entityType).Where(entity => entity.EntityQueryCategoryId == entityQueryCategoryId);

        public override void Add(EntityQuery entityToAdd)
        {
            if (entityToAdd.Id > 0)
            {
                throw new NotSupportedException($"{nameof(Add)} operations for entity of type '{EntityTypeName}' cannot have an Id greater than 0 specified. If this is a existing entity consider using the Update method for this service instead.");
            }

            PrimeEntityForPersistance(entityToAdd, true);
            UnitOfWork.SaveChanges();
        }

        public override void Update(EntityQuery entityWithUpdates)
        {
            if (entityWithUpdates.Id <= 0)
            {
                throw new NotSupportedException($"{nameof(Update)} operations for entity of type '{EntityTypeName}' cannot have an Id of 0 specified. If this is a new entity consider using the Add method for this service instead.");
            }

            const EntityState entityState = EntityState.Modified;
            PrimeEntityForPersistance(entityWithUpdates, false);
            UnitOfWork.Entry(entityWithUpdates).State = entityState;

            // TODO Perform any other necessary modifications to EntityState of the object in question.
            entityWithUpdates.EntityPropertyFilters.ToList()
                             .ForEach(entity =>
                                      {
                                          UnitOfWork.Entry(entity).State = entity.Id > 0 ? entityState : EntityState.Added;
                                      });

            var entityPropertyFilterIdsToKeep = entityWithUpdates.EntityPropertyFilters.Select(epf => epf.Id).ToArray();
            UnitOfWork.Entry(entityWithUpdates).Collection(x => x.EntityPropertyFilters).Query().Where(epf => !entityPropertyFilterIdsToKeep.Contains(epf.Id)).Load();

            var entityPropertyFiltersToRemove = entityWithUpdates.EntityPropertyFilters.Where(epf => !entityPropertyFilterIdsToKeep.Contains(epf.Id));
            UnitOfWork.ThreadSafeDatabaseContext.EntitySet<EntityPropertyFilter>().RemoveRange(entityPropertyFiltersToRemove);
            UnitOfWork.Entry(entityWithUpdates).State = entityState;

            UnitOfWork.SaveChanges();
        }
        public override void Delete(int id)
        {
            throw new NotSupportedException($"{nameof(Delete)} operations are not supported for the type: {EntityTypeName}");
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

        protected override void PrimeEntityForPersistance(EntityQuery entityToPrime, bool primeForAdd)
        {
            ValidateEntity(entityToPrime);
            entityToPrime.EntityQueryCategoryId = entityToPrime.EntityQueryCategory.Id;
            entityToPrime.EntityPropertyFilters.ToList().ForEach(entityPropertyFilter => entityPropertyFilter.EntityQueryId = entityToPrime.Id);

            if (primeForAdd)
            {
                Repository.Add(entityToPrime);
            }
            else
            {
                Repository.Attach(entityToPrime);
            }

            UnitOfWork.Entry(entityToPrime.EntityQueryCategory).State = EntityState.Unchanged;
        }

        protected override void ValidateEntity(EntityQuery entity)
        {
            // DO NOTHING FOR NOW.
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