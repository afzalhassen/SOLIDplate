using SOLIDplate.Application.Services.Clients.SignalR.Interfaces;
using SOLIDplate.Domain.Command.Services.Interfaces;
using System.Net.Http;
using System.Web.Http;

namespace SOLIDplate.Application.Services.Micro.WebApi.SignalR
{
    public abstract class BaseCommandApiController<TCommandEntity, TDomainCommandService, TEntityHubInterface, TEntityHubClient> : BaseCommandApiController<TCommandEntity, TDomainCommandService>
        where TCommandEntity : class, new()
        where TDomainCommandService : IDomainService<TCommandEntity>
        where TEntityHubClient : IEntityHubClient<TCommandEntity, TEntityHubInterface>
    {
        protected readonly TEntityHubClient EntityHubClient;

        protected BaseCommandApiController(TDomainCommandService domainCommandService, TEntityHubClient entityHubClient)
            : base(domainCommandService)
        {
            EntityHubClient = entityHubClient;
        }

        [HttpPost]
        public new virtual HttpResponseMessage Post([FromBody]TCommandEntity commandEntity)
        {
            try
            {
                return base.Post(commandEntity);
            }
            finally
            {
                EntityHubClient.NotifyAllOfNew(commandEntity);
            }
        }

        [HttpPut]
        public new virtual HttpResponseMessage Put([FromBody]TCommandEntity commandEntity)
        {
            try
            {
                return base.Put(commandEntity);
            }
            finally
            {
                EntityHubClient.NotifyAllOfUpdate(commandEntity);
            }
        }

        [HttpDelete]
        public new virtual HttpResponseMessage Delete([FromUri]int id)
        {
            try
            {
                return base.Delete(id);
            }
            finally
            {
                EntityHubClient.NotifyAllOfDelete(id);
            }
        }
    }
}