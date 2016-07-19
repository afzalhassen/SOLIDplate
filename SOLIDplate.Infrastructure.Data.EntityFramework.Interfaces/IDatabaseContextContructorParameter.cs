using SOLIDplate.Infrastructure.Data.Services.EntityFramework.Interfaces;

namespace SOLIDplate.Infrastructure.Data.EntityFramework.Interfaces
{
    public interface IDatabaseContextContructorParameter
    {
        string DatabaseConnectionString { get; }
        IEntityAuditService EntityAuditService { get; }
        string DataConfigurationDllName { get; }
        string DataConfigurationClassFullName { get; }
    }
}