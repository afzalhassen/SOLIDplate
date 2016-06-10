using SOLIDplate.Domain.Entities;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;

namespace SOLIDplate.Infrastructure.Data.Services.EntityFramework.Interfaces
{
    public interface IEntityAuditService
    {
        IEnumerable<EntityAudit> AuditDbEntityEntries(IEnumerable<DbEntityEntry> dbEntityEntries);
        IEnumerable<EntityAudit> UpdateEntityAuditEntityIdsIfRequired(IEnumerable<DbEntityEntry> dbEntityEntries, IList<EntityAudit> entityAudits);
    }
}