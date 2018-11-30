using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using Unity;

namespace SOLIDplate.Application.Unity.SignalR
{
    public class SignalRUnityDependencyResolver : DefaultDependencyResolver
    {
        private readonly IUnityContainer _unityContainer;

        public SignalRUnityDependencyResolver(IUnityContainer unityContainer)
        {
            _unityContainer = unityContainer;
        }

        public override object GetService(Type serviceType)
        {
            return _unityContainer.IsRegistered(serviceType) ?
                       _unityContainer.Resolve(serviceType) :
                       base.GetService(serviceType);
        }

        public override IEnumerable<object> GetServices(Type serviceType)
        {
            return _unityContainer.IsRegistered(serviceType) ?
                       _unityContainer.ResolveAll(serviceType) :
                       base.GetServices(serviceType);
        }
    }
}