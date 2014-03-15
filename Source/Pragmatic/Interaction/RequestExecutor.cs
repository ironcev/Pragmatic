using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.Interaction
{
    public class RequestExecutor // TODO-IG: There is too much code duplication between the executors. Refector that.
    {
        private readonly IInteractionHandlerResolver _interactionHandlerResolver;

        public RequestExecutor(IInteractionHandlerResolver interactionHandlerResolver)
        {
            Argument.IsNotNull(interactionHandlerResolver, "interactionHandlerResolver");

            _interactionHandlerResolver = interactionHandlerResolver;
        }

        public TResponse Execute<TResponse>(IRequest<TResponse> request) where TResponse : Response
        {
            Argument.IsNotNull(request, "request");

            InteractionScope.BeginOrJoin();

            try
            {
                var requestHandlers = GetRequestHandlers<TResponse>(request.GetType()).ToArray();

                if (requestHandlers.Length <= 0)
                    throw new InvalidOperationException(string.Format("There is no request handler defined for the requests of type '{0}'.", request.GetType()));

                if (requestHandlers.Length > 1)
                    throw new NotSupportedException(string.Format("There are {1} request handlers defined for the requests of type '{2}'.{0}" + // TODO-IG: Introduce ExceptionBuilder class to avoid code polution.
                                                                  "Having more than one request handler per request type is not supported.{0}" +
                                                                  "The defined request handlers are:{0}{3}",
                                                                  Environment.NewLine,
                                                                  requestHandlers.Length,
                                                                  request.GetType(),                                                              
                                                                  requestHandlers.Aggregate(string.Empty, (output, requestHandler) => output + requestHandler.GetType() + Environment.NewLine)));

                return ExecuteRequestHandler(requestHandlers[0], request);
            }
            finally
            {
                InteractionScope.End();
            }
        }

        private static TResponse ExecuteRequestHandler<TResponse>(object requestHandler, IRequest<TResponse> request) where TResponse : Response
        {
            try
            {
                var executeMethod = requestHandler.GetType().GetMethod("Execute", // TODO-IG: Replace with lambda expressions once when SwissKnife supports that.
                                        BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod,
                                        null, CallingConventions.HasThis,
                                        new[] { request.GetType() },
                                        null);
                return (TResponse)executeMethod.Invoke(requestHandler, new object[] { request });
            }
            catch (Exception e)
            {
                string message = string.Format("An exception occured while executing the request handler of type '{0}'.", requestHandler.GetType());

                throw new RequestExecutionException(message, e);
            }
        }

        private IEnumerable<object> GetRequestHandlers<TResponse>(Type requestType) where TResponse : Response
        {
            System.Diagnostics.Debug.Assert(requestType != null);
            System.Diagnostics.Debug.Assert(typeof(IRequest<TResponse>).IsAssignableFrom(requestType));

            try
            {
                return _interactionHandlerResolver.ResolveInteractionHandler(typeof(IRequestHandler<,>).MakeGenericType(requestType, typeof(TResponse)));
            }
            catch (Exception e)
            {
                string message = string.Format("An exception occured while resolving request handlers for the requests of type '{0}'.", requestType);

                throw new RequestExecutionException(message, e);
            } 
        }
    }
}
