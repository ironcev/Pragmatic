using System;
using System.Collections.Generic;
using System.Linq;
using Pragmatic.Interaction;
using StructureMap;

namespace Pragmatic.StructureMap
{
    public class StructureMapQueryExecutor : QueryExecutor
    {
        protected override IEnumerable<object> GetQueryHandlers<TResult>(Type queryType)
        {
            System.Diagnostics.Debug.Assert(queryType != null);
            System.Diagnostics.Debug.Assert(typeof(IQuery).IsAssignableFrom(queryType));

            try
            {
                return ObjectFactory.GetAllInstances(typeof(IQueryHandler<,>).MakeGenericType(queryType, typeof(TResult))).Cast<object>();
            }
            catch (Exception e)
            {
                string additionalMessage = string.Format("An exception occured while resolving query handlers for the queries of type '{0}' and query results of type '{1}'.", queryType, typeof(TResult));
                LogException(additionalMessage, e);

                return Enumerable.Empty<object>();
            }            
        }
    }
}
