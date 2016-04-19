using SOLIDplate.Domain.Integration.Services.Interfaces;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SOLIDplate.Services.WebApi
{
	public abstract class BaseIntegrationApiController<TDomainIntegrationService, TEntity, TKeyType> : ApiController
		where TDomainIntegrationService : IDomainIntegrationService<TEntity, TKeyType>
		where TEntity : class, new()
	{
		protected TDomainIntegrationService DomainIntegrationService { get; }

		protected BaseIntegrationApiController(TDomainIntegrationService domainIntegrationService)
		{
			DomainIntegrationService = domainIntegrationService;
		}

		[HttpGet]
		public HttpResponseMessage Get()
		{
			try
			{
				return Request.CreateResponse(HttpStatusCode.OK, DomainIntegrationService.Get());
			}
			catch (Exception e)
			{
				return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
			}
		}

		[HttpGet]
		public HttpResponseMessage Get(TKeyType key)
		{
			try
			{
				return Request.CreateResponse(HttpStatusCode.OK, DomainIntegrationService.Get(key));
			}
			catch (Exception e)
			{
				return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
			}
		}
	}
}