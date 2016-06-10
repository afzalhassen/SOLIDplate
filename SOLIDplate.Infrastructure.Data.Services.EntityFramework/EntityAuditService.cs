using SOLIDplate.Domain.Entities;
using SOLIDplate.Domain.Entities.Interfaces;
using SOLIDplate.Infrastructure.Data.Services.EntityFramework.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace SOLIDplate.Infrastructure.Data.Services.EntityFramework
{
    public class EntityAuditService : IEntityAuditService
    {
        private static readonly Dictionary<Type, List<string>> TypeIdentifierPropertiesCache = new Dictionary<Type, List<string>>();

        public IEnumerable<EntityAudit> AuditDbEntityEntries(IEnumerable<DbEntityEntry> dbEntityEntries)
        {
            var actionedBy = Thread.CurrentPrincipal.Identity.Name;
            var dateAndTimeOfChange = DateTime.Now;
            var entityAuditResults = new List<EntityAudit>();

            foreach (var dbEntityEntry in dbEntityEntries)
            {
                var tableName = dbEntityEntry.Entity.GetType().Name;
                var entityIdentities = string.Join(", ", GetEntityIdentityValues(dbEntityEntry));
                var actionType = dbEntityEntry.State.ToString();

                switch (dbEntityEntry.State)
                {
                    case EntityState.Added:
                        entityAuditResults.AddRange(
                            dbEntityEntry.CurrentValues.PropertyNames
                            .Where(propertyName =>
                                !GetIdentityPropertyNameForEntity().Equals(propertyName) &&
                                !GetIdentityPropertyNamesForCompositeEntity(dbEntityEntry.Entity.GetType()).Contains(propertyName))
                                .Select(propertyName => new EntityAudit
                                {
                                    ActionedBy = actionedBy,
                                    DateTime = dateAndTimeOfChange,
                                    TableName = tableName,
                                    EntityId = entityIdentities,
                                    ActionType = actionType,
                                    PropertyName = propertyName,
                                    NewValue = GetCurrentPropertyValue(dbEntityEntry, propertyName)
                                }));
                        break;
                    case EntityState.Modified:
                        entityAuditResults.AddRange(
                            dbEntityEntry.OriginalValues.PropertyNames
                            .Where(propertyName =>
                                !Equals(dbEntityEntry.GetDatabaseValues().GetValue<object>(propertyName), dbEntityEntry.CurrentValues.GetValue<object>(propertyName)))
                                .Select(propertyName => new EntityAudit
                                {
                                    ActionedBy = actionedBy,
                                    DateTime = dateAndTimeOfChange,
                                    TableName = tableName,
                                    EntityId = entityIdentities,
                                    ActionType = actionType,
                                    PropertyName = propertyName,
                                    OldValue = dbEntityEntry.GetDatabaseValues().GetValue<object>(propertyName) == null ? null : dbEntityEntry.GetDatabaseValues().GetValue<object>(propertyName).ToString(),
                                    NewValue = GetCurrentPropertyValue(dbEntityEntry, propertyName)
                                }));
                        break;
                    case EntityState.Deleted:
                        entityAuditResults.Add(new EntityAudit
                        {
                            ActionedBy = actionedBy,
                            DateTime = dateAndTimeOfChange,
                            TableName = tableName,
                            EntityId = entityIdentities,
                            ActionType = actionType,
                            PropertyName = "ALL"
                        });
                        break;
                }
                // Otherwise, don't do anything, we don't care about Unchanged or Detached entities
            }
            return entityAuditResults;
        }

        public IEnumerable<EntityAudit> UpdateEntityAuditEntityIdsIfRequired(IEnumerable<DbEntityEntry> dbEntityEntries, IList<EntityAudit> entityAudits)
        {
            var namesOfTablesThatHaveAddedEntities = (from addedEntityAudit in entityAudits
                                                      where addedEntityAudit.ActionType.Equals(EntityState.Added.ToString())
                                                      select addedEntityAudit.TableName).Distinct();
            var dbEntityEntriesMatchingTableNames = (from dbEntityEntry in dbEntityEntries
                                                     where namesOfTablesThatHaveAddedEntities.Contains(dbEntityEntry.Entity.GetType().Name)
                                                     select dbEntityEntry).ToList();

            foreach (var entityAudit in from addedEntityAudit in entityAudits
                                        where addedEntityAudit.ActionType.Equals(EntityState.Added.ToString())
                                        select addedEntityAudit)
            {
                foreach (var dbEntityEntryMatchingTableNames in dbEntityEntriesMatchingTableNames)
                {
                    foreach (var propertyName in dbEntityEntryMatchingTableNames.OriginalValues.PropertyNames)
                    {
                        if (entityAudit.EntityId.Equals("0") && // make sure that the ID hasnt already been set
                            entityAudit.TableName.Equals(dbEntityEntryMatchingTableNames.Entity.GetType().Name) && // that we are acquiring the entityAudit for the correct table
                            entityAudit.PropertyName.Equals(propertyName) && // that we are referencing the correct property name
                                                                             // and then finally comparing the correct values of said propertyName
                            ((entityAudit.NewValue == null && GetCurrentPropertyValue(dbEntityEntryMatchingTableNames, propertyName) == null) ||
                            (entityAudit.NewValue != null && entityAudit.NewValue.Equals(GetCurrentPropertyValue(dbEntityEntryMatchingTableNames, propertyName)))))
                        {
                            var entityIdentityValues = string.Join(", ", GetEntityIdentityValues(dbEntityEntryMatchingTableNames));
                            entityAudit.EntityId = entityIdentityValues;
                        }
                    }
                }
            }
            return entityAudits;
        }

        private static string GetCurrentPropertyValue(DbEntityEntry dbEntityEntry, string propertyName)
        {
            return dbEntityEntry.CurrentValues.GetValue<object>(propertyName) == null ? null : dbEntityEntry.CurrentValues.GetValue<object>(propertyName).ToString();
        }

        private static IEnumerable<string> GetEntityIdentityValues(DbEntityEntry dbEntityEntry)
        {
            var identityPropertyNameForEntity = GetIdentityPropertyNameForEntity();
            var result = new List<string>();

            if (DoesEntityImplementInterface(typeof(IEntity<>), dbEntityEntry.Entity.GetType()))
            {
                result.Add(dbEntityEntry.CurrentValues.GetValue<object>(identityPropertyNameForEntity).ToString());
            }
            else if (DoesEntityImplementInterface(typeof(ICompositeEntity<,>), dbEntityEntry.Entity.GetType()))
            {
                var compositeEntityIdentityPropertyNames = GetIdentityPropertyNamesForCompositeEntity(dbEntityEntry.Entity.GetType());
                result.AddRange(compositeEntityIdentityPropertyNames
                                .Select(identityPropertyNameForCompositeEntity => $"{identityPropertyNameForCompositeEntity}{identityPropertyNameForEntity}")
                                .Select(identityPropertyNameForCompositeEntityFormatted => dbEntityEntry.CurrentValues.GetValue<object>(identityPropertyNameForCompositeEntityFormatted).ToString()));
            }
            else if (IsEntityIdentifierAGuid(dbEntityEntry.CurrentValues.GetValue<object>(identityPropertyNameForEntity)))
            {
                result.Add(dbEntityEntry.CurrentValues.GetValue<object>(identityPropertyNameForEntity).ToString());
            }
            else
            {
                throw new NotSupportedException("The identifier(s) for this entity are unable to be determined because it has not been catered for in the design of the system.");
            }
            return result;
        }

        private static bool IsEntityIdentifierAGuid(object identifier)
        {
            Guid newGuid;
            var result = Guid.TryParse(identifier.ToString(), out newGuid);
            return result;
        }

        private static string GetIdentityPropertyNameForEntity()
        {
            if (!TypeIdentifierPropertiesCache.ContainsKey(typeof(IEntity<>)))
            {
                var identityPropertyName = (from member in typeof(IEntity<>).GetMembers()
                                            where member.MemberType == MemberTypes.Property
                                            select member.Name).First();
                TypeIdentifierPropertiesCache.Add(typeof(IEntity<>), new List<string> { identityPropertyName });
            }

            return TypeIdentifierPropertiesCache[typeof(IEntity<>)][0];
        }

        private static IEnumerable<string> GetIdentityPropertyNamesForCompositeEntity(Type entityType)
        {
            var compositeIdentityEntities = (from interfaces in entityType.GetInterfaces()
                                             where interfaces.Name.Equals(typeof(ICompositeEntity<,>).Name)
                                             select interfaces.GenericTypeArguments).FirstOrDefault();

            if (!TypeIdentifierPropertiesCache.ContainsKey(entityType))
            {
                var identityPropertyNames = compositeIdentityEntities == null ?
                                            new List<string>() :
                                            (from compositeIdentityEntity in compositeIdentityEntities
                                             select compositeIdentityEntity.Name).ToList();
                TypeIdentifierPropertiesCache.Add(entityType, identityPropertyNames);
            }

            return TypeIdentifierPropertiesCache[entityType];
        }

        private static bool DoesEntityImplementInterface(Type interfaceType, Type entityType)
        {
            return entityType.GetInterfaces()
                             .Select(implementedInterface => implementedInterface.Name)
                             .Any(interfaceName => interfaceName.Equals(interfaceType.Name));
        }
    }
}
