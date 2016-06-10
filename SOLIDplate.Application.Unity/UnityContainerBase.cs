using Microsoft.Practices.Unity;
using System;

namespace SOLIDplate.Application.Unity
{
    /// <summary>
    /// Generic Bootstrapper to build and configure Unity Containers
    /// </summary>
    public abstract class UnityContainerBase
    {
        /// <summary>
        /// Builds and configures a new Unity container.
        /// </summary>
        /// <returns>A configured Unity container</returns>
        public IUnityContainer Resolve()
        {
            var unityContainer = new UnityContainer();
            try
            {
                RegisterDependencies(unityContainer);
            }
            catch (Exception)
            {
                unityContainer.Dispose();
                throw;
            }

            return unityContainer;
        }

        /// <summary>
        /// Configures types with an existing Unity container by registering extensions, services etc
        /// </summary>
        /// <param name="unityContainer">
        /// The container .
        /// </param>
        protected abstract void RegisterDependencies(IUnityContainer unityContainer);
    }
}
