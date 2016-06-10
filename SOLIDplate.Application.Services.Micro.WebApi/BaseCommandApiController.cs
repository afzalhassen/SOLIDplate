using SOLIDplate.Domain.Command.Services.Interfaces;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SOLIDplate.Application.Services.Micro.WebApi
{
    public abstract class BaseCommandApiController<TCommandEntity, TDomainCommandService> : ApiController
        where TCommandEntity : class, new()
        where TDomainCommandService : IDomainService<TCommandEntity>
    {
        protected readonly TDomainCommandService DomainCommandService;

        protected BaseCommandApiController(TDomainCommandService domainCommandService)
        {
            DomainCommandService = domainCommandService;
        }

        [HttpPost]
        public virtual HttpResponseMessage Post([FromBody]TCommandEntity commandEntity)
        {
            try
            {
                DomainCommandService.Add(commandEntity);
                return Request.CreateResponse(HttpStatusCode.Created, commandEntity);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut]
        public virtual HttpResponseMessage Put([FromBody]TCommandEntity commandEntity)
        {
            try
            {
                DomainCommandService.Update(commandEntity);
                return Request.CreateResponse(HttpStatusCode.OK, commandEntity);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpDelete]
        public virtual HttpResponseMessage Delete([FromUri]int id)
        {
            try
            {
                DomainCommandService.Delete(id);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
