using Microsoft.AspNet.SignalR.Hubs;

namespace SOLIDplate.Application.Services.SignalR.Hubs.Interfaces
{
    public interface IEntityHub<in TEntity> : IHub
        where TEntity : class, new()
    {
        void NotifyAllOfNew(TEntity entity);
        void NotifyAllOfUpdate(TEntity entity);
        void NotifyAllOfDelete(int id);

        void NotifyAllExceptCallerOfNew(TEntity entity);
        void NotifyAllExceptCallerOfUpdate(TEntity entity);
        void NotifyAllExceptCallerOfDelete(int id);
    }
}
