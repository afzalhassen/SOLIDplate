using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using System.Net;

namespace SOLIDplate.Application.Services.Clients.SignalR.Interfaces
{
    public interface IEntityHubClientConstructorParameter<TEntityHubInterface>
    {
        ICredentials Credentials { get; set; }
        HubConnection HubConnection { get; set; }
        JsonSerializer JsonSerializer { get; set; }
    }
}