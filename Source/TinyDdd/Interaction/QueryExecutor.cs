using System;
using System.Collections.Generic;
using System.Linq;
using SwissKnife.Diagnostics.Contracts;

namespace TinyDdd.Interaction
{
    public abstract class QueryExecutor
    {
        public TResult Execute<TResult>(IQuery<TResult> query)
        {
            return ExecuteCore<TResult>(query);
        }

        public TResult Execute<TQuery, TResult>(TQuery query) where TQuery : IQuery // TODO-IG: What if we have hierarchy of queries? What is the expected behavior - polimorphic or not? (Same with the commands.)
        {
            return ExecuteCore<TResult>(query);
        }

        private TResult ExecuteCore<TResult>(IQuery query)
        {
            Argument.IsNotNull(query, "query");

            var queryHandlers = GetQueryHandlers(query.GetType(), typeof(TResult)).ToArray();

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

            object result;
            try
            {
                result = queryHandlers[0].Execute(query);
            }
            catch (Exception e)
            {
                string additionalMessage = string.Format("An exception occured while executing the query handler of type '{0}'.", queryHandlers[0].GetType());
                LogException(additionalMessage, e);

                throw new QueryExecutionException(additionalMessage, e);
            }

            try
            {
                return (TResult)result;
            }
            catch (InvalidCastException e)
            {
                string additionalMessage = string.Format("An exception occured while casting the result of the query handler of type '{1}'.{0}" +
                                                         "The returned result object cannot be cast into the expected result type.{0}" +
                                                         "The expected result type is '{2}'.{0}" +
                                                         "The returned result object type is '{3}'.",
                                                         Environment.NewLine,
                                                         queryHandlers[0].GetType(),
                                                         typeof(TResult),
                                                         result.GetType());
                LogException(additionalMessage, e);

                throw new QueryExecutionException(additionalMessage, e);
            }            
        }

        protected abstract IEnumerable<IQueryHandler> GetQueryHandlers(Type queryType, Type queryResultType);
        protected virtual void LogException(string additionalMessage, Exception exception) { }
    }
}
