using SOLIDplate.Infrastructure.Data.EntityFramework.Interfaces;
using SOLIDplate.Infrastructure.Data.Services.EntityFramework.Interfaces;
using System;

namespace SOLIDplate.Infrastructure.Data.EntityFramework
{
    public abstract class DatabaseContextContructorParameter : IDatabaseContextContructorParameter
    {
        protected DatabaseContextContructorParameter(string dataConfigurationDllName, string dataConfigurationClassFullName, string databaseConnectionString, IEntityAuditService entityAuditService)
        {

            if (string.IsNullOrEmpty(dataConfigurationDllName))
            {
                throw new ArgumentNullException(nameof(dataConfigurationDllName));
            }

            if (string.IsNullOrEmpty(dataConfigurationClassFullName))
            {
                throw new ArgumentNullException(nameof(dataConfigurationClassFullName));
            }

            if (string.IsNullOrEmpty(databaseConnectionString))
            {
                throw new ArgumentNullException(nameof(databaseConnectionString));
            }

            if (entityAuditService == null)
            {
                throw new ArgumentNullException(nameof(entityAuditService));
            }

            DatabaseConnectionString = databaseConnectionString;
            EntityAuditService = entityAuditService;
            DataConfigurationDllName = dataConfigurationDllName;
            DataConfigurationClassFullName = dataConfigurationClassFullName;
        }
        public string DatabaseConnectionString { get; }
        public IEntityAuditService EntityAuditService { get; }
        public string DataConfigurationDllName { get; }
        public string DataConfigurationClassFullName { get; }
    }
}
