using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Pragmatic.Interaction.Caching;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.Interaction
{
    public class QueryExecutor // TODO-IG: What if we have hierarchy of queries? What is the expected behavior - polymorphic or not? (Same with the hierarchy of commands.)
    {
        private readonly IInteractionHandlerResolver _interactionHandlerResolver;
        private readonly IQueryResultCacheResolver _queryResultCacheResolver;

        public QueryExecutor(IInteractionHandlerResolver interactionHandlerResolver, IQueryResultCacheResolver queryResultCacheResolver)
        {
            Argument.IsNotNull(interactionHandlerResolver, "interactionHandlerResolver");
            Argument.IsNotNull(queryResultCacheResolver, "queryResultCacheResolver");

            _interactionHandlerResolver = interactionHandlerResolver;
            _queryResultCacheResolver = queryResultCacheResolver;
        }


        public TResult Execute<TResult>(IQuery<TResult> query)
        {
            return ExecuteCore<TResult>(query);
        }

        public TResult Execute<TQuery, TResult>(TQuery query) where TQuery : class, IQuery
        {
            return ExecuteCore<TResult>(query);
        }

        private TResult ExecuteCore<TResult>(IQuery query)
        {
            Argument.IsNotNull(query, "query");

            InteractionScope.BeginOrJoin();

            try
            {
                // See if there is a caching defined for this query type.
                var queryResultCashes = GetQueryResultCashes<TResult>(query.GetType()).ToArray();
                if (queryResultCashes.Length > 1)
                    throw new NotSupportedException(string.Format("There are {1} query result caches defined for the queries of type '{2}' and results of type '{3}'.{0}" +
                                                                  "Having more than one query result cache type per query type and result type is not supported.{0}" +
                                                                  "The defined query result caches are:{0}{4}",
                                                                  System.Environment.NewLine,
                                                                  queryResultCashes.Length,
                                                                  query.GetType(),
                                                                  typeof(TResult),
                                                                  queryResultCashes.Aggregate(string.Empty, (output, cache) => output + cache.GetType() + System.Environment.NewLine)));

                var queryResultCache = (IQueryResultCache<TResult>)queryResultCashes.FirstOrDefault();

                // If yes and there are values, return the values from the cache.
                if (queryResultCache != null && queryResultCache.HasCachedResultFor(query))
                    return GetCachedResultFor(queryResultCache, query);

                // If no, execute the query.
                var queryHandlers = GetQueryHandlers<TResult>(query.GetType()).ToArray();

                if (queryHandlers.Length <= 0)
                    throw new InvalidOperationException(string.Format("There is no query handler defined for the queries of type '{0}' and query results of type '{1}'.", query.GetType(), typeof(TResult)));

                if (queryHandlers.Length > 1)
                    throw new NotSupportedException(string.Format("There are {1} query handlers defined for the queries of type '{2}' and results of type '{3}'.{0}" +
                                                                  "Having more than one query handler per query type and result type is not supported.{0}" +
                                                                  "The defined query handlers are:{0}{4}",
                                                                  System.Environment.NewLine,
                                                                  queryHandlers.Length,
                                                                  query.GetType(),
                                                                  typeof(TResult),
                                                                  queryHandlers.Aggregate(string.Empty, (output, queryHandler) => output + queryHandler.GetType() + System.Environment.NewLine)));

                var result = ExecuteQueryHandler<TResult>(queryHandlers[0], query);

                // If the cashing is defined, cache the result before returning it.
                if (queryResultCache != null)
                    CacheResultFor(queryResultCache, query, result);

                return result;
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

        private static TResult GetCachedResultFor<TResult>(IQueryResultCache<TResult> queryResultCache, IQuery query)
        {
            try
            {
                return queryResultCache.GetCachedResultFor(query);
            }
            catch (Exception e)
            {
                string additionalMessage = string.Format("An exception occurred while getting the cashed query result from the query result cache of type '{0}'.", queryResultCache.GetType());

                throw new QueryExecutionException(additionalMessage, e);
            }
        }

        private static void CacheResultFor<TResult>(IQueryResultCache<TResult> queryResultCache, IQuery query, TResult result)
        {
            try
            {
                queryResultCache.CacheResultFor(query, result);
            }
            catch (Exception e)
            {
                string additionalMessage = string.Format("An exception occurred while cashing the query result in the query result cache of type '{0}'.", queryResultCache.GetType());

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

        protected IEnumerable<object> GetQueryResultCashes<TResult>(Type queryType)
        {
            System.Diagnostics.Debug.Assert(queryType != null);
            System.Diagnostics.Debug.Assert(typeof(IQuery).IsAssignableFrom(queryType));

            return _queryResultCacheResolver.ResolveQueryResultCache(typeof(IQueryResultCache<,>).MakeGenericType(queryType, typeof(TResult)));
        }
    }
}
