namespace SOLIDplate.Domain.Services.Interfaces
{
    public interface IDomainService<TEntity>: Query.Services.Interfaces.IDomainService<TEntity>, Command.Services.Interfaces.IDomainService<TEntity>
         where TEntity : class, new()
    {
    }
}
