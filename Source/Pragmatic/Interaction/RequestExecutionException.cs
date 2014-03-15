using System;
using System.Runtime.Serialization;

namespace Pragmatic.Interaction
{
    [Serializable]
    public class RequestExecutionException : Exception
    {
        public RequestExecutionException()
        {            
        }

        public RequestExecutionException(string message) : base(message)
        {
        }

        public RequestExecutionException(string message, Exception inner) : base(message, inner)
        {            
        }

        protected RequestExecutionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {            
        }
    }
}
