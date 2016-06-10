using Microsoft.Practices.Unity;
using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace SOLIDplate.Application.Unity.WebApi
{
    public class WebApiUnityControllerActivator : IHttpControllerActivator
    {
        private readonly IUnityContainer _container;

        public WebApiUnityControllerActivator(IUnityContainer container)
        {
            _container = container;
        }

        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            var controller = (IHttpController)_container.Resolve(controllerType);
            return controller;
        }
    }
}