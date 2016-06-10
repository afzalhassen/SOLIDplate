using SOLIDplate.Application.Services.Clients.Wcf.Interfaces;
using SOLIDplate.Domain.Integration.Services.Interfaces;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading;

namespace SOLIDplate.Domain.Integration.Services
{
    public abstract class DomainIntegrationService<TEntity, TKeyType, TServiceClientChannelFactoryInterface, TServiceInterface> : IDomainIntegrationService<TEntity, TKeyType>
        where TEntity : class, new()
        where TServiceClientChannelFactoryInterface : IClientChannelFactory<TServiceInterface>
    {
        protected readonly TServiceClientChannelFactoryInterface ServiceClientChannelFactoryInterface;
        protected string EntityTypeName => typeof(TEntity).Name;

        protected WindowsImpersonationContext CurrentWindowsImpersonationContext => ((WindowsIdentity)Thread.CurrentPrincipal.Identity).Impersonate();

        protected DomainIntegrationService(TServiceClientChannelFactoryInterface serviceClientChannelFactoryInterface)
        {
            ServiceClientChannelFactoryInterface = serviceClientChannelFactoryInterface;
        }

        public abstract IEnumerable<TEntity> Get();
        public abstract TEntity Get(TKeyType key);
    }
}
