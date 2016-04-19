namespace SOLIDplate.Repository.EntityFramework.Interfaces
{
	public interface IEntityRepository<TEntity> : Repository.Interfaces.IEntityRepository<TEntity>
		where TEntity : class, new()
	{
		void Attach(TEntity entity);
	}
}
