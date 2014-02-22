using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SwissKnife.Diagnostics.Contracts;

namespace TinyDdd.Interaction // TODO-IG: In general, move from extinsibility by subclassing to dependency injection.
{
    public abstract class QueryExecutor // TODO-IG: What if we have hierarchy of queries? What is the expected behavior - polimorphic or not? (Same with the commands.)
    {
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

            try
            {
                return ExecuteQueryHandler<TResult>(queryHandlers[0], query);
            }
            catch (Exception e)
            {
                string additionalMessage = string.Format("An exception occured while executing the query handler of type '{0}'.", queryHandlers[0].GetType());
                LogException(additionalMessage, e);

                throw new QueryExecutionException(additionalMessage, e);
            }
        }

        private static TResult ExecuteQueryHandler<TResult>(object queryHandler, IQuery query) // TODO-IG: Remove duplicated code from here and from the CommandExecutor class.
        {
            var executeMethod = queryHandler.GetType().GetMethod("Execute", // TODO-IG: Replace with labda expressions once when SwissKnife supports that.
                                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod,
                                    null, CallingConventions.HasThis,
                                    new[] { query.GetType() },
                                    null);
            return (TResult)executeMethod.Invoke(queryHandler, new object[] { query });
        }

        protected abstract IEnumerable<object> GetQueryHandlers<TResult>(Type queryType);
        protected virtual void LogException(string additionalMessage, Exception exception) { }
    }
}
