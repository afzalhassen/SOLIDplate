using System.Collections.Generic;

namespace SOLIDplate.Domain.Integration.Services.Interfaces
{
    public interface IDomainIntegrationService<out TEntity, in TKeyType>
		 where TEntity : class, new()
	{
		IEnumerable<TEntity> Get();
		TEntity Get(TKeyType key);
	}
}
