using System;
using System.Collections.Generic;
using System.Linq;
using StructureMap;
using TinyDdd.Interaction;

namespace TinyDdd.StructureMap
{
    public class StructureMapQueryExecutor : QueryExecutor
    {
        protected override IEnumerable<IQueryHandler> GetQueryHandlers(Type queryType, Type queryResultType)
        {
            System.Diagnostics.Debug.Assert(queryType != null);
            System.Diagnostics.Debug.Assert(typeof(IQuery).IsAssignableFrom(queryType));
            System.Diagnostics.Debug.Assert(queryResultType != null);

            try
            {
                return ObjectFactory.GetAllInstances(typeof(IQueryHandler<,>).MakeGenericType(queryType, queryResultType)).Cast<IQueryHandler>();
            }
            catch (Exception e)
            {
                string additionalMessage = string.Format("An exception occured while resolving query handlers for the queries of type '{0}' and query results of type '{1}'.", queryType, queryResultType);
                LogException(additionalMessage, e);

                return Enumerable.Empty<IQueryHandler>();
            }            
        }
    }
}
