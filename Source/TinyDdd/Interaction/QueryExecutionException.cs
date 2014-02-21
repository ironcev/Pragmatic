using System;
using System.Runtime.Serialization;

namespace TinyDdd.Interaction
{
    [Serializable]
    public class QueryExecutionException : Exception
    {
        public QueryExecutionException()
        {            
        }

        public QueryExecutionException(string message) : base(message)
        {
        }

        public QueryExecutionException(string message, Exception inner) : base(message, inner)
        {            
        }

        protected QueryExecutionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {            
        }
    }
}
