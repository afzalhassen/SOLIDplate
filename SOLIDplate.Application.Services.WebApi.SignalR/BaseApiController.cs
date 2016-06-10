using SOLIDplate.Application.Services.Clients.SignalR.Interfaces;
using SOLIDplate.Domain.Services.Interfaces;
using System.Net.Http;
using System.Web.Http;

namespace SOLIDplate.Application.Services.WebApi.SignalR
{
    public abstract class BaseApiController<TEntity, TDomainService, TEntityHubInterface, TEntityHubClient> : BaseApiController<TEntity, TDomainService>
        where TEntity : class, new()
        where TDomainService : IDomainService<TEntity>
        where TEntityHubClient : IEntityHubClient<TEntity, TEntityHubInterface>
    {
        protected readonly TEntityHubClient EntityHubClient;
        protected BaseApiController(TDomainService domainService, TEntityHubClient entityHubClient) : base(domainService)
        {
            EntityHubClient = entityHubClient;
        }

        [HttpPost]
        public new virtual HttpResponseMessage Post([FromBody]TEntity entity)
        {
            try
            {
                return base.Post(entity);
            }
            finally
            {
                EntityHubClient.NotifyAllOfNew(entity);
            }
        }

        [HttpPut]
        public new virtual HttpResponseMessage Put([FromBody]TEntity entity)
        {
            try
            {
                return base.Put(entity);
            }
            finally
            {
                EntityHubClient.NotifyAllOfUpdate(entity);
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
