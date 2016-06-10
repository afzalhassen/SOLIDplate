using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Practices.Unity;
using System;

namespace SOLIDplate.Application.Unity.SignalR
{
    public class SignalRUnityHubActivator : IHubActivator
    {
        private readonly IUnityContainer _container;

        public SignalRUnityHubActivator(IUnityContainer container)
        {
            _container = container;
        }

        public IHub Create(HubDescriptor descriptor)
        {
            if (descriptor == null)
            {
                throw new ArgumentNullException(nameof(descriptor));
            }

            if (descriptor.HubType == null)
            {
                return null;
            }

            var hub = _container.Resolve(descriptor.HubType) ?? Activator.CreateInstance(descriptor.HubType);
            return hub as IHub;
        }
    }
}