using System;
using System.Linq.Expressions;
using System.ServiceModel;

namespace SOLIDplate.Application.Services.Clients.Wcf.Interfaces
{
    public interface IClientChannelFactory<TServiceInterface>
    {
        /// <summary>
        /// Creates a new Channel from the factory.
        /// </summary>
        /// <returns>The newly created channel</returns>
        ICommunicationObject CreateNewProxy();

        /// <summary>
        /// Creates and automatically Opens a new Channel.
        /// </summary>
        /// <returns>The newly created and opened channel</returns>
        ICommunicationObject OpenNewProxy();

        /// <summary>
        /// Closes the Channel.
        /// Channel will be closed unless it has faulted, which in that case it will be Aborted
        /// </summary>
        /// <param name="proxy">
        /// The Channel to be closed.
        /// </param>
        void CloseProxy(ICommunicationObject proxy);

        /// <summary>
        /// Safely opens, invokes and closes a new Channel using the provided action expression.
        /// Also ensures that any TargetInvocationException is unwrapped and rethrown
        /// </summary>
        /// <param name="expression">
        /// Action expression to be invoked.
        /// </param>
        void CallProxy(Expression<Action<TServiceInterface>> expression);

        /// <summary>
        /// Safely opens, invokes and closes a new Channel using the provided action expression.
        /// Also ensures that any TargetInvocationException is unwrapped and rethrown
        /// </summary>
        /// <typeparam name="TResult">
        /// The type of the result.
        /// </typeparam>
        /// <param name="expression">
        /// Func expression to be invoked.
        /// </param>
        /// <returns>
        /// Instance of TResult
        /// </returns>
        TResult CallProxy<TResult>(Expression<Func<TServiceInterface, TResult>> expression);
    }
}
