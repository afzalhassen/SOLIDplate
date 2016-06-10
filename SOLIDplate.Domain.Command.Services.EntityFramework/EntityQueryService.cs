using SOLIDplate.Domain.Command.Services.Interfaces;
using SOLIDplate.Domain.Entities;
using SOLIDplate.Infrastructure.Data.EntityFramework.Interfaces;
using SOLIDplate.Infrastructure.Repository.EntityFramework.Interfaces;
using System;
using System.Data.Entity;
using System.Linq;

namespace SOLIDplate.Domain.Command.Services.EntityFramework
{
    public class EntityQueryService : DomainService<EntityQuery>, IEntityQueryService
    {
        public EntityQueryService(IUnitOfWork unitOfWork, IEntityRepository<EntityQuery> entityRepository)
            : base(unitOfWork, entityRepository)
        {
        }
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
    }
}