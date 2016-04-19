using SOLIDplate.Common.Portable;
using SOLIDplate.Services.Clients.SignalR.Interfaces;
using Microsoft.AspNet.SignalR.Client;
using System.Threading.Tasks;

namespace SOLIDplate.Services.Clients.SignalR
{
	public abstract class EntityHubClient<TEntity, TEntityHubInterface> : IEntityHubClient<TEntity, TEntityHubInterface>
		where TEntity : class, new()
	{
		protected readonly HubConnection HubConnection;
		protected readonly IHubProxy HubProxy;

		protected EntityHubClient(IEntityHubClientConstructorParameter<TEntityHubInterface> entityHubClientConstructorParameter)
		{
			HubConnection = entityHubClientConstructorParameter.HubConnection;
			var hubNameDerived = typeof(TEntityHubInterface).Name.Substring(1);
			var hubNameFormatted = hubNameDerived.ToCamelCase();
			HubProxy = HubConnection.CreateHubProxy(hubNameFormatted);
			HubConnection.Start().Wait();
		}

		public virtual Task NotifyAllOfNew(TEntity entity)
		{
			return HubProxy.Invoke("NotifyAllOfNew", entity);
		}

		public virtual Task NotifyAllOfUpdate(TEntity entity)
		{
			return HubProxy.Invoke("NotifyAllOfUpdate", entity);
		}

		public virtual Task NotifyAllOfDelete(int id)
		{
			return HubProxy.Invoke("NotifyAllOfDelete", id);
		}

		public virtual Task NotifyAllExceptCallerOfNew(TEntity entity)
		{
			return HubProxy.Invoke("NotifyAllExceptCallerOfNew", entity);
		}

		public virtual Task NotifyAllExceptCallerOfUpdate(TEntity entity)
		{
			return HubProxy.Invoke("NotifyAllExceptCallerOfUpdate", entity);
		}

		public virtual Task NotifyAllExceptCallerOfDelete(int id)
		{
			return HubProxy.Invoke("NotifyAllExceptCallerOfDelete", id);
		}
	}
}