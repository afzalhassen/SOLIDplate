using SOLIDplate.Data.EntityFramework.Interfaces;
using SOLIDplate.Data.Services.EntityFramework.Interfaces;

namespace SOLIDplate.Data.EntityFramework
{
	public abstract class DatabaseContextContructorParameter : IDatabaseContextContructorParameter
	{
		protected DatabaseContextContructorParameter(string databaseConnectionString, IEntityAuditService entityAuditService)
		{
			DatabaseConnectionString = databaseConnectionString;
			EntityAuditService = entityAuditService;
		}
		public string DatabaseConnectionString { get; }
		public IEntityAuditService EntityAuditService { get; }
	}
}
