namespace SOLIDplate.Infrastructure.Repository.EntityFramework.Interfaces
{
    public interface ICompositeEntityRepository<TEntity, TIdentityEntity1, TIdentityEntity2> : IEntityRepository<TEntity>
        where TIdentityEntity1 : class, new()
        where TIdentityEntity2 : class, new()
        where TEntity : class, new()
    {
    }

    public interface ICompositeEntityRepository<TEntity, TIdentityEntity1, TIdentityEntity2, TIdentityEntity3> : IEntityRepository<TEntity>
        where TIdentityEntity1 : class, new()
        where TIdentityEntity2 : class, new()
        where TIdentityEntity3 : class, new()
        where TEntity : class, new()
    {
    }
}