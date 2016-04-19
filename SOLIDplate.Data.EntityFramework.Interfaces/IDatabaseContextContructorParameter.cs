using SOLIDplate.Data.Services.EntityFramework.Interfaces;

namespace SOLIDplate.Data.EntityFramework.Interfaces
{
	public interface IDatabaseContextContructorParameter
	{
		string DatabaseConnectionString { get; }
		IEntityAuditService EntityAuditService { get; }
	}
}