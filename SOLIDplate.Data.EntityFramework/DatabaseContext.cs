using SOLIDplate.Data.EntityFramework.Configurations;
using SOLIDplate.Data.EntityFramework.Interfaces;
using SOLIDplate.Data.Services.EntityFramework.Interfaces;
using SOLIDplate.Domain.Entities;
using System;
using System.Collections.Generic;
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

namespace SOLIDplate.Data.EntityFramework
{
	public class DatabaseContext : DbContext, IDatabaseContext
	{
		private const int DefaultCommandTimeOut = 600;
		private readonly IEntityAuditService _entityAuditService;

		public DatabaseContext(string databaseConnectionString, IEntityAuditService entityAuditService)
			: base(databaseConnectionString)
		{
			_entityAuditService = entityAuditService;
			GetObjectContext().CommandTimeout = DefaultCommandTimeOut;
			EnableAuditing = true;
			Configuration.LazyLoadingEnabled = false;
			Configuration.ProxyCreationEnabled = false;
			Configuration.AutoDetectChangesEnabled = false;
		}

		public bool EnableAuditing { get; set; }

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
				var entityAudits = new List<EntityAudit>();

				if (EnableAuditing)
				{
					entityAudits = _entityAuditService.AuditDbEntityEntries(ChangeTracker.Entries()).ToList();
				}

				var result = base.SaveChanges();

				entitiesTransactionScope.Complete();

				if (EnableAuditing)
				{
					using (var entityAuditsTransactionScope = new TransactionScope(TransactionScopeOption.RequiresNew, timeSpan))
					{
						entityAudits = _entityAuditService.UpdateEntityAuditEntityIdsIfRequired(ChangeTracker.Entries(), entityAudits.ToList()).ToList();
						EntitySet<EntityAudit>().AddRange(entityAudits);
						base.SaveChanges();
						entityAuditsTransactionScope.Complete();
					}
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
											.Where(objectStateEntry => objectStateEntry.State != EntityState.Detached && objectStateEntry.Entity != null))
			{
				objectContext.Detach(objectStateEntry.Entity);
			}
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			if (modelBuilder == null)
			{
				throw new ArgumentNullException("modelBuilder");
			}

			modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

			AddConfigurations(modelBuilder);

			base.OnModelCreating(modelBuilder);
		}

		private static void AddConfigurations(DbModelBuilder modelBuilder)
		{
			if (modelBuilder == null)
			{
				throw new ArgumentNullException("modelBuilder");
			}

			const string dataConfigurationDllName = "DataConfigurationDllName";
			const string dataConfigurationClassFullName = "DataConfigurationClassFullName";
			var appSettings = ConfigurationManager.AppSettings;

			if (!appSettings.AllKeys.Contains(dataConfigurationDllName) ||
				!appSettings.AllKeys.Contains(dataConfigurationClassFullName))
			{
				var message = string.Format("Either '{0}' or '{1}' or both keys cannot be found in the appsettings NameValueCollection of the application's configuration file.", dataConfigurationDllName, dataConfigurationClassFullName);
				throw new SettingsPropertyNotFoundException(message);
			}

			var typeDescriptor = string.Format("{0},{1}", appSettings.Get(dataConfigurationDllName), appSettings.Get(dataConfigurationClassFullName));
			var baseType = typeof(EntityConfiguration<,>);
			var dataConfigurationType = Type.GetType(typeDescriptor);

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