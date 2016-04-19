using System.Collections.Generic;

namespace SOLIDplate.Domain.Services.Interfaces
{
	public interface IDomainService<TEntity>
		 where TEntity : class, new()
	{
		IEnumerable<TEntity> Get();
		TEntity Get(int id);
		void Add(TEntity entityToAdd);
		void Update(TEntity entityWithUpdates);
		void Delete(int id);
		IEnumerable<TEntity> ExecuteQuery(int queryId);
	}
}
