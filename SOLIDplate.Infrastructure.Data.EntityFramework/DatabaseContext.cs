using SOLIDplate.Domain.Entities;
using SOLIDplate.Infrastructure.Data.EntityFramework.Configurations;
using SOLIDplate.Infrastructure.Data.EntityFramework.Interfaces;
using SOLIDplate.Infrastructure.Data.Services.EntityFramework.Interfaces;
using System;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Transactions;

namespace SOLIDplate.Infrastructure.Data.EntityFramework
{
    public class DatabaseContext : DbContext, IDatabaseContext
    {
        private const int DefaultCommandTimeOut = 600;
        private readonly string _dataConfigurationDllName;
        private readonly string _dataConfigurationClassFullName;
        private readonly IEntityAuditService _entityAuditService;

        public DatabaseContext(string dataConfigurationDllName, string dataConfigurationClassFullName, string databaseConnectionString, IEntityAuditService entityAuditService)
            : base(databaseConnectionString)
        {
            if (string.IsNullOrEmpty(dataConfigurationDllName))
            {
                throw new ArgumentNullException(nameof(dataConfigurationDllName));
            }

            if (string.IsNullOrEmpty(dataConfigurationClassFullName))
            {
                throw new ArgumentNullException(nameof(dataConfigurationClassFullName));
            }

            if (entityAuditService == null)
            {
                throw new ArgumentNullException(nameof(entityAuditService));
            }

            _dataConfigurationDllName = dataConfigurationDllName;
            _dataConfigurationClassFullName = dataConfigurationClassFullName;
            _entityAuditService = entityAuditService;
            GetObjectContext().CommandTimeout = DefaultCommandTimeOut;
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
            Configuration.AutoDetectChangesEnabled = false;
        }

        public void Initialize(bool force)
        {
            Database.Initialize(force);
        }
        public bool Exists()
        {
            return Database.Exists();
        }
        public bool IsDirty()
        {
            return ChangeTracker.Entries().Any(e => e.State == EntityState.Added ||
                                                    e.State == EntityState.Modified ||
                                                    e.State == EntityState.Deleted);
        }

        public DbSet<TEntity> EntitySet<TEntity>()
            where TEntity : class, new()
        {
            return Set<TEntity>();
        }

        public new DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity)
            where TEntity : class, new()
        {
            return base.Entry(entity);
        }

        public override int SaveChanges()
        {
            var timeSpan = new TimeSpan(0, 30, 0);

            ChangeTracker.DetectChanges();

            using (var entitiesTransactionScope = new TransactionScope(TransactionScopeOption.Required, timeSpan))
            {
#if DEBUG
                try
                {
#endif
                    var entityAudits = _entityAuditService.AuditDbEntityEntries(ChangeTracker.Entries()).ToList();
                    var result = base.SaveChanges();

                    entitiesTransactionScope.Complete();

                    using (var entityAuditsTransactionScope = new TransactionScope(TransactionScopeOption.RequiresNew, timeSpan))
                    {
                        entityAudits = _entityAuditService.UpdateEntityAuditEntityIdsIfRequired(ChangeTracker.Entries(), entityAudits.ToList()).ToList();
                        EntitySet<EntityAudit>().AddRange(entityAudits);
                        base.SaveChanges();
                        entityAuditsTransactionScope.Complete();
                    }

                    return result;
#if DEBUG
                }
                catch (DbEntityValidationException dbEntityValidationException)
                {
                    foreach (var entityValidationError in dbEntityValidationException.EntityValidationErrors)
                    {
                        Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", entityValidationError.Entry.Entity.GetType().Name, entityValidationError.Entry.State);

                        foreach (var validationError in entityValidationError.ValidationErrors)
                        {
                            Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                    throw;
                }
#endif
            }
        }

        public void DetachAll()
        {
            var objectContext = GetObjectContext();

            // Only detach that which hasn't already been detached.
            const EntityState entityState = EntityState.Added | EntityState.Deleted | EntityState.Modified | EntityState.Unchanged;
            foreach (var objectStateEntry in objectContext.ObjectStateManager
                                            .GetObjectStateEntries(entityState)
                                            .Where(objectStateEntry => objectStateEntry.State != EntityState.Detached && 
                                                                       objectStateEntry.Entity != null
                                                  )
                    )
            {
                objectContext.Detach(objectStateEntry.Entity);
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            AddConfigurations(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void AddConfigurations(DbModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            var appSettings = ConfigurationManager.AppSettings;

            if (!appSettings.AllKeys.Contains(_dataConfigurationDllName) ||
                !appSettings.AllKeys.Contains(_dataConfigurationClassFullName))
            {
                var message = $"Either '{_dataConfigurationDllName}' or '{_dataConfigurationClassFullName}' or both keys cannot be found in the appsettings NameValueCollection of the application's configuration file.";
                throw new SettingsPropertyNotFoundException(message);
            }

            var typeDescriptor = $"{appSettings.Get(_dataConfigurationClassFullName)},{appSettings.Get(_dataConfigurationDllName)}";
            var baseType = typeof(EntityConfiguration<,>);
            var dataConfigurationType = Type.GetType(typeDescriptor);

            if (dataConfigurationType == null)
            {
                throw new NullReferenceException($"{nameof(dataConfigurationType)} with type descriptor '{typeDescriptor}' cannot be found.");
            }

            Assembly.GetAssembly(dataConfigurationType)
                    .GetTypes()
                    .Where(type => type.IsClass &&
                                    type.IsAbstract == false &&
                                    type.BaseType != null &&
                                    type.BaseType.IsAbstract &&
                                    type.BaseType.IsGenericType &&
                                    type.BaseType.GetGenericTypeDefinition() == baseType)
                    .Select(Activator.CreateInstance)
                    .ToList()
                    .ForEach(repositoryConfiguration => modelBuilder.Configurations.Add((dynamic)repositoryConfiguration));
        }

        private ObjectContext GetObjectContext()
        {
            return (this as IObjectContextAdapter).ObjectContext;
        }
    }
}