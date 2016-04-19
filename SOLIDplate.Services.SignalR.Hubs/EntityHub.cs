using SOLIDplate.Services.SignalR.Hubs.Interfaces;
using Microsoft.AspNet.SignalR;

namespace SOLIDplate.Services.SignalR.Hubs
{
	public abstract class EntityHub<TEntity, TEntityHub> : Hub, IEntityHub<TEntity>
		where TEntity : class, new()
		where TEntityHub : IEntityHub<TEntity>
	{
		protected readonly IEntityHubContextBroadcaster<TEntity> Broadcaster;

		protected EntityHub(IEntityHubContextBroadcaster<TEntity> broadcaster)
		{
			Broadcaster = broadcaster;
		}

		public abstract void NotifyAllOfNew(TEntity entity);
		public abstract void NotifyAllOfUpdate(TEntity entity);
		public abstract void NotifyAllOfDelete(int id);

		public abstract void NotifyAllExceptCallerOfNew(TEntity entity);
		public abstract void NotifyAllExceptCallerOfUpdate(TEntity entity);
		public abstract void NotifyAllExceptCallerOfDelete(int id);
	}
}