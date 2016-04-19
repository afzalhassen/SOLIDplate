using SOLIDplate.Domain.Services.Interfaces;
using SOLIDplate.Services.Clients.SignalR.Interfaces;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SOLIDplate.Services.WebApi
{
	public abstract class BaseApiController<TEntity, TDomainService, TEntityHubInterface, TEntityHubClient> : ApiController
		where TEntity : class, new()
		where TDomainService : IDomainService<TEntity>
		where TEntityHubClient : IEntityHubClient<TEntity, TEntityHubInterface>
	{
		protected TDomainService DomainService { get; }
		protected TEntityHubClient EntityHubClient { get; }

		protected BaseApiController(TDomainService domainService, TEntityHubClient entityHubClient)
		{
			DomainService = domainService;
			EntityHubClient = entityHubClient;
		}

		[HttpGet]
		public HttpResponseMessage Get()
		{
			try
			{
				return Request.CreateResponse(HttpStatusCode.OK, DomainService.Get());
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
				return Request.CreateResponse(HttpStatusCode.OK, DomainService.Get(id));
			}
			catch (Exception e)
			{
				return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
			}
		}

		[HttpGet]
		public HttpResponseMessage ExecuteQuery([FromUri]int queryId)
		{
			try
			{
				return Request.CreateResponse(HttpStatusCode.OK, DomainService.ExecuteQuery(queryId));
			}
			catch (Exception e)
			{
				return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
			}
		}

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
			finally
			{
				EntityHubClient.NotifyAllOfNew(entity);
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
			finally
			{
				EntityHubClient.NotifyAllOfUpdate(entity);
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
			finally
			{
				EntityHubClient.NotifyAllOfDelete(id);
			}
		}
	}
}
