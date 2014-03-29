using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SwissKnife.Collections;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.Interaction
{
    public class Response<T> : Response
    {
        public T Result { get; private set; }

        public Response(T result)
        {
            Result = result;
        }

        public Response()
        {
            Result = default(T);
        }

        public Response(Response<T> originalResponse) : base(originalResponse)
        {
            Result = originalResponse.Result;
        }

        public static Response<T> From(Response response)
        {
            return From(response, default(T));
        }

        public static Response<T> From(Response response, T result)
        {
            Argument.IsNotNull(response, "response");

            Response<T> newResponse = new Response<T>(result);
            newResponse.Add(response);
            return newResponse;
        }
    }

    public class Response
    {
        private readonly IList<ResponseMessage> _responseMessages = new List<ResponseMessage>();

        public bool HasInformation { get { return HasMessagesOfType(MessageType.Information); } }
        public bool HasWarnings { get { return HasMessagesOfType(MessageType.Warning); } }
        public bool HasErrors { get { return HasMessagesOfType(MessageType.Error); } }
        public bool HasInformationOrWarnings { get { return HasInformation || HasWarnings; } }

        public IEnumerable<ResponseMessage> Successes { get { return GetMessagesOfType(MessageType.Success); } }
        public IEnumerable<ResponseMessage> Information { get { return GetMessagesOfType(MessageType.Information); } }
        public IEnumerable<ResponseMessage> Warnings { get { return GetMessagesOfType(MessageType.Warning); } }
        public IEnumerable<ResponseMessage> Errors { get { return GetMessagesOfType(MessageType.Error); } }
        public IEnumerable<ResponseMessage> AllMessages { get { return _responseMessages; } }

        public Response() { }

        public Response(Response originalResponse)
        {
            Argument.IsNotNull(originalResponse, "originalResponse");

            _responseMessages.AddMany(originalResponse._responseMessages);
        }

        public Response Add(ResponseMessage responseMessage)
        {
            Argument.IsNotNull(responseMessage, "responseMessage");

            _responseMessages.Add(responseMessage);

            return this;
        }

        public Response Add(Response response)
        {
            Argument.IsNotNull(response, "response");
            Argument.IsValid(response != this, string.Format("{0} can not be added to itself.", typeof(Response)), "response");

            _responseMessages.AddMany(response._responseMessages);

            return this;
        }

        public Response AddSuccess(string message)
        {
            AddInformation(message, string.Empty);

            return this;
        }

        public Response AddSuccess(string message, string key)
        {
            _responseMessages.Add(new ResponseMessage(MessageType.Success, message, key));

            return this;
        }

        public Response AddSuccess(Expression<Func<string>> resourceSelector)
        {
            Argument.IsNotNull(resourceSelector, "resourceSelector");

            AddSuccess(resourceSelector.Compile()(), ResponseMessageKeyBuilder.From(resourceSelector));

            return this;
        }

        public Response AddInformation(string message)
        {
            AddInformation(message, string.Empty);

            return this;
        }

        public Response AddInformation(string message, string key)
        {
            _responseMessages.Add(new ResponseMessage(MessageType.Information, message, key));

            return this;
        }

        public Response AddInformation(Expression<Func<string>> resourceSelector)
        {
            Argument.IsNotNull(resourceSelector, "resourceSelector");

            AddInformation(resourceSelector.Compile()(), ResponseMessageKeyBuilder.From(resourceSelector));

            return this;
        }

        public Response AddWarning(string message)
        {
            AddWarning(message, string.Empty);

            return this;
        }

        public Response AddWarning(string message, string key)
        {
            _responseMessages.Add(new ResponseMessage(MessageType.Warning, message, key));

            return this;
        }

        public Response AddWarning(Expression<Func<string>> resourceSelector)
        {
            Argument.IsNotNull(resourceSelector, "resourceSelector");

            AddWarning(resourceSelector.Compile()(), ResponseMessageKeyBuilder.From(resourceSelector));

            return this;
        }

        public Response AddError(string message)
        {
            AddError(message, string.Empty);

            return this;
        }

        public Response AddError(string message, string key)
        {
            _responseMessages.Add(new ResponseMessage(MessageType.Error, message, key));

            return this;
        }

        public Response AddError(Expression<Func<string>> resourceSelector)
        {
            Argument.IsNotNull(resourceSelector, "resourceSelector");

            AddError(resourceSelector.Compile()(), ResponseMessageKeyBuilder.From(resourceSelector));

            return this;
        }

        public Response AddErrors(IEnumerable<string> errors)
        {
            Argument.IsNotNull(errors, "errors");

            _responseMessages.AddMany(errors.Select(error => new ResponseMessage(MessageType.Error, error, string.Empty)));

            return this;
        }

        public Response InsertError(string message, string key)
        {
            _responseMessages.Insert(0, new ResponseMessage(MessageType.Error, message, key));

            return this;
        }

        private bool HasMessagesOfType(MessageType messageType)
        {
            return _responseMessages.Any(x => x.MessageType == messageType);
        }

        private IEnumerable<ResponseMessage> GetMessagesOfType(MessageType messageType)
        {
            return _responseMessages.Where(x => x.MessageType == messageType).ToList().AsReadOnly();
        }
    }
}

/*
 * DECISIONS:
 * Most likely the AddXYZ(resourceSelector) method is the one which will be mostly used.
 * Since it compiles the expression and analyzes it in the same time to obtain the key it is the slowest of all overridables.
 * We assume that there will be no performance issues because of that.
 */