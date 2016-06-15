using SOLIDplate.Domain.Query.Services.Interfaces;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SOLIDplate.Application.Services.Micro.WebApi
{
    public abstract class BaseQueryApiController<TQueryEntity, TDomainQueryService> : ApiController
        where TQueryEntity : class, new()
        where TDomainQueryService : IDomainService<TQueryEntity>
    {
        protected TDomainQueryService DomainQueryService { get; }

        protected BaseQueryApiController(TDomainQueryService domainQueryService)
        {
            DomainQueryService = domainQueryService;
        }

        [HttpGet]
        public HttpResponseMessage Get()
        {
            try
            {
                var queryEntities = DomainQueryService.Get();
                return Request.CreateResponse(HttpStatusCode.OK, queryEntities);
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
                var queryEntity = DomainQueryService.Get(id);
                return Request.CreateResponse(HttpStatusCode.OK, queryEntity);
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
        //        var queryEntities = DomainQueryService.ExecuteQuery(queryId);
        //        return Request.CreateResponse(HttpStatusCode.OK, queryEntities);
        //    }
        //    catch (Exception e)
        //    {
        //        return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
        //    }
        //}
    }
}