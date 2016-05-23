using SOLIDplate.Services.Clients.Wcf.Interfaces;
using System;
using System.Linq.Expressions;
using System.Reflection;
using System.ServiceModel;

namespace SOLIDplate.Services.Clients.Wcf
{
    public class ClientChannelFactory<TServiceInterface> : IClientChannelFactory<TServiceInterface>
    {
        private readonly string _endpointConfigurationName;
        private readonly object _lockObj;
        private volatile ChannelFactory<TServiceInterface> _domainIntegrationServiceChannelFactory;

        public ClientChannelFactory(string endpointConfigurationName)
        {
            _lockObj = new object();
            _endpointConfigurationName = endpointConfigurationName;
        }

        public ICommunicationObject CreateNewProxy()
        {
            return SetupFactory().CreateChannel() as ICommunicationObject;
        }

        public ICommunicationObject OpenNewProxy()
        {
            var proxy = CreateNewProxy();
            proxy.Open();
            return proxy;
        }

        public void CloseProxy(ICommunicationObject proxy)
        {
            if (proxy == null)
            {
                return;
            }
            if (proxy.State == CommunicationState.Faulted)
            {
                proxy.Abort();
            }
            else
            {
                proxy.Close();
            }
        }

        public void CallProxy(Expression<Action<TServiceInterface>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            var proxy = default(ICommunicationObject);

            try
            {
                proxy = OpenNewProxy();
                using (new OperationContextScope((IClientChannel)proxy))
                {
                    expression.Compile().DynamicInvoke(proxy);
                }
            }
            catch (TargetInvocationException e)
            {
                HandleTargetInvocationException(e);
                throw;
            }
            finally
            {
                CloseProxy(proxy);
            }
        }

        public TResult CallProxy<TResult>(Expression<Func<TServiceInterface, TResult>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            var proxy = default(ICommunicationObject);

            try
            {
                proxy = OpenNewProxy();

                using (new OperationContextScope((IClientChannel)proxy))
                {
                    return (TResult)expression.Compile().DynamicInvoke(proxy);
                }
            }
            catch (TargetInvocationException e)
            {
                HandleTargetInvocationException(e);
                throw;
            }
            finally
            {
                CloseProxy(proxy);
            }
        }

        private ChannelFactory<TServiceInterface> SetupFactory()
        {
            if (_domainIntegrationServiceChannelFactory != null)
            {
                return _domainIntegrationServiceChannelFactory;
            }
            lock (_lockObj)
            {
                if (_domainIntegrationServiceChannelFactory == null)
                {
                    _domainIntegrationServiceChannelFactory = new ChannelFactory<TServiceInterface>(_endpointConfigurationName);
                }
            }

            return _domainIntegrationServiceChannelFactory;
        }

        private static void HandleTargetInvocationException(TargetInvocationException e)
        {
            if (e.InnerException != null)
            {
                throw e.InnerException;
            }
        }
    }
}