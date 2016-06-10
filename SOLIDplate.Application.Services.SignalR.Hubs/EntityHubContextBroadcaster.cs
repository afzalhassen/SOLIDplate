using Microsoft.AspNet.SignalR.Hubs;
using SOLIDplate.Application.Services.SignalR.Hubs.Interfaces;

namespace SOLIDplate.Application.Services.SignalR.Hubs
{
    public class EntityHubContextBroadcaster<TEntity> : IEntityHubContextBroadcaster<TEntity>
        where TEntity : class, new()
    {
        private readonly IHubConnectionContext<dynamic> _clients;

        public EntityHubContextBroadcaster(IHubConnectionContext<dynamic> clients)
        {
            _clients = clients;
        }

        public void NotifyAllOfNew(TEntity entity)
        {
            _clients.All.entityAdded(entity);
        }

        public void NotifyAllOfUpdate(TEntity entity)
        {
            _clients.All.entityUpdated(entity);
        }

        public void NotifyAllOfDelete(int id)
        {
            _clients.All.entityAdded(id);
        }

        public void NotifyAllExceptCallerOfNew(string connectionId, TEntity entity)
        {
            _clients.AllExcept(connectionId).entityAdded(entity);
        }

        public void NotifyAllExceptCallerOfUpdate(string connectionId, TEntity entity)
        {
            _clients.AllExcept(connectionId).entityUpdated(entity);
        }

        public void NotifyAllExceptCallerOfDelete(string connectionId, int id)
        {
            _clients.AllExcept(connectionId).entityAdded(id);
        }
    }
}