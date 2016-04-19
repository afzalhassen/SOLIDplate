using SOLIDplate.Services.Clients.WebApi.Interfaces;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace SOLIDplate.Services.Clients.WebApi
{
	public abstract class WebApiHttpClient : HttpClient, IWebApiHttpClient
	{
		protected WebApiHttpClient(AuthenticationHeaderValue authenticationHeaderValue)
		{
			DefaultRequestHeaders.Authorization = authenticationHeaderValue;
		}

		protected WebApiHttpClient(Uri baseAddress)
		{
			BaseAddress = baseAddress;
		}

		protected WebApiHttpClient(Uri baseAddress, AuthenticationHeaderValue authenticationHeaderValue)
			: this(authenticationHeaderValue)
		{
			BaseAddress = baseAddress;
		}

		public new void Dispose()
		{
			Dispose(true);
		}
	}
}