using System.Linq;

namespace SOLIDplate.Domain.Query.Services.Interfaces
{
    public interface IDomainService<out TEntity>
         where TEntity : class, new()
    {
        /// <summary>
        /// Gets a queryable collection of TEntity
        /// </summary>
        /// <returns>A queryable collection of TEntity</returns>
        IQueryable<TEntity> Get();
        /// <summary>
        /// Gets an instance of TEntity.
        /// </summary>
        /// <param name="id">An integer value that will be used to check for equality against the TEntity.Id property value.</param>
        /// <returns>An instance of TEntity whose Id property value is euqal to the value of <see cref="id"/></returns>
        TEntity Get(int id);
    }
}
