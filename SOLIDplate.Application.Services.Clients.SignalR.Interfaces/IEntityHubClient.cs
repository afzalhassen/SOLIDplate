using System.Threading.Tasks;

namespace SOLIDplate.Application.Services.Clients.SignalR.Interfaces
{
    public interface IEntityHubClient<in TEntity, TEntityHubInterface>
        where TEntity : class, new()
    {
        Task NotifyAllOfNew(TEntity entity);
        Task NotifyAllOfUpdate(TEntity entity);
        Task NotifyAllOfDelete(int id);

        Task NotifyAllExceptCallerOfNew(TEntity entity);
        Task NotifyAllExceptCallerOfUpdate(TEntity entity);
        Task NotifyAllExceptCallerOfDelete(int id);
    }
}
