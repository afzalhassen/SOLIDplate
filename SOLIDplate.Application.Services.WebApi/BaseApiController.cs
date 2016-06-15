using SOLIDplate.Domain.Services.Interfaces;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SOLIDplate.Application.Services.WebApi
{
    public abstract class BaseApiController<TEntity, TDomainService> : ApiController
        where TEntity : class, new()
        where TDomainService : IDomainService<TEntity>
    {
        protected readonly TDomainService DomainService;
        protected BaseApiController(TDomainService domainService)
        {
            DomainService = domainService;
        }


        [HttpGet]
        public HttpResponseMessage Get()
        {
            try
            {
                var entities = DomainService.Get();
                return Request.CreateResponse(HttpStatusCode.OK, entities);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet]
        public HttpResponseMessage Get([FromUri]int id)
        {
            try
            {
                var entity = DomainService.Get(id);
                return Request.CreateResponse(HttpStatusCode.OK, entity);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpGet]
        public abstract HttpResponseMessage ExecuteQuery([FromUri] int queryId);
        //{
        //    try
        //    {
        //        var queryEntities = DomainService.ExecuteQuery(queryId);
        //        return Request.CreateResponse(HttpStatusCode.OK, queryEntities);
        //    }
        //    catch (Exception e)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
        //    }
        //}

        [HttpPost]
        public virtual HttpResponseMessage Post([FromBody]TEntity entity)
        {
            try
            {
                DomainService.Add(entity);
                return Request.CreateResponse(HttpStatusCode.Created, entity);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }

        [HttpPut]
        public virtual HttpResponseMessage Put([FromBody]TEntity entity)
        {
            try
            {
                DomainService.Update(entity);
                return Request.CreateResponse(HttpStatusCode.OK, entity);
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
                DomainService.Delete(id);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
            }
        }
    }
}
