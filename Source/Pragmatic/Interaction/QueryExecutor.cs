using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.Interaction
{
    public class QueryExecutor // TODO-IG: What if we have hierarchy of queries? What is the expected behavior - polymorphic or not? (Same with the hierarchy of commands.)
    {
        private readonly IInteractionHandlerResolver _interactionHandlerResolver;

        public QueryExecutor(IInteractionHandlerResolver interactionHandlerResolver)
        {
            Argument.IsNotNull(interactionHandlerResolver, "interactionHandlerResolver");

            _interactionHandlerResolver = interactionHandlerResolver;
        }


        public TResult Execute<TResult>(IQuery<TResult> query)
        {
            return ExecuteCore<TResult>(query);
        }

        public TResult Execute<TQuery, TResult>(TQuery query) where TQuery : IQuery 
        {
            return ExecuteCore<TResult>(query);
        }

        private TResult ExecuteCore<TResult>(IQuery query)
        {
            Argument.IsNotNull(query, "query");

            InteractionScope.BeginOrJoin();

            try
            {
                var queryHandlers = GetQueryHandlers<TResult>(query.GetType()).ToArray();

                if (queryHandlers.Length <= 0)
                    throw new InvalidOperationException(string.Format("There is no query handler defined for the queries of type '{0}' and query results of type '{1}'.", query.GetType(), typeof(TResult)));

                if (queryHandlers.Length > 1)
                    throw new NotSupportedException(string.Format("There are {1} query handlers defined for the queries of type '{2}' and results of type '{3}'.{0}" +
                                                                  "Having more than one query handler per query type and result type is not supported.{0}" +
                                                                  "The defined query handlers are:{0}{4}",
                                                                  Environment.NewLine,
                                                                  queryHandlers.Length,
                                                                  query.GetType(),
                                                                  typeof(TResult),
                                                                  queryHandlers.Aggregate(string.Empty, (output, queryHandler) => output + queryHandler.GetType() + Environment.NewLine)));

                return ExecuteQueryHandler<TResult>(queryHandlers[0], query);
            }
            finally
            {
                InteractionScope.End();
            }
        }

        private static TResult ExecuteQueryHandler<TResult>(object queryHandler, IQuery query) // TODO-IG: Remove duplicated code from here and from the CommandExecutor class.
        {
            try
            {
                var executeMethod = queryHandler.GetType().GetMethod("Execute", // TODO-IG: Replace with lambda expressions once when SwissKnife supports that.
                                        BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod,
                                        null, CallingConventions.HasThis,
                                        new[] { query.GetType() },
                                        null);
                return (TResult)executeMethod.Invoke(queryHandler, new object[] { query });
            }
            catch (Exception e)
            {
                string additionalMessage = string.Format("An exception occurred while executing the query handler of type '{0}'.", queryHandler.GetType());

                throw new QueryExecutionException(additionalMessage, e);
            }
        }

        protected IEnumerable<object> GetQueryHandlers<TResult>(Type queryType)
        {
            System.Diagnostics.Debug.Assert(queryType != null);
            System.Diagnostics.Debug.Assert(typeof(IQuery).IsAssignableFrom(queryType));

            try
            {
                return _interactionHandlerResolver.ResolveInteractionHandler(typeof(IQueryHandler<,>).MakeGenericType(queryType, typeof(TResult)));
            }
            catch (Exception e)
            {
                string message = string.Format("An exception occurred while resolving query handlers for the queries of type '{0}' and query results of type '{1}'.", queryType, typeof(TResult));

                throw new QueryExecutionException(message, e);
            }
        }
    }
}
