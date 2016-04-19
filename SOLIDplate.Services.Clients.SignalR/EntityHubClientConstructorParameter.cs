using SOLIDplate.Services.Clients.SignalR.Interfaces;
using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using System.Net;

namespace SOLIDplate.Services.Clients.SignalR
{
	public class EntityHubClientConstructorParameter<TEntityHubInterface> : IEntityHubClientConstructorParameter<TEntityHubInterface>
	{
		public EntityHubClientConstructorParameter(HubConnection hubConnection, ICredentials credentials, JsonSerializer jsonSerializer)
		{
			Credentials = credentials;
			HubConnection = hubConnection;
			JsonSerializer = jsonSerializer;
		}
		public ICredentials Credentials { get; set; }
		public HubConnection HubConnection { get; set; }
		public JsonSerializer JsonSerializer { get; set; }
	}
}
