﻿using System.Collections.Generic;
using System.Linq;
using SwissKnife.Collections;
using SwissKnife.Diagnostics.Contracts;

namespace TinyDdd.Interaction
{
    public class Response<T> : Response
    {
        public T Result { get; private set; }

        public Response(T result)
        {
            Result = result;
        }
    }

    public class Response
    {
        private readonly IList<ResponseMessage> _responseMessages = new List<ResponseMessage>();

        public bool HasInformations
        {
            get
            {
                return HasMessagesOfType( MessageType.Information );
            }
        }

        public bool HasWarnings
        {
            get
            {
                return HasMessagesOfType( MessageType.Warning );
            }
        }

        public bool HasErrors
        {
            get
            {
                return HasMessagesOfType( MessageType.Error );
            }
        }

        public IEnumerable<ResponseMessage> Informations
        {
            get
            {
                return GetMessagesOfType( MessageType.Information );
            }
        }
        public IEnumerable<ResponseMessage> Warnings
        {
            get
            {
                return GetMessagesOfType( MessageType.Warning );
            }
        }
        public IEnumerable<ResponseMessage> Errors
        {
            get
            {
                return GetMessagesOfType( MessageType.Error );
            }
        }

        public void Add(ResponseMessage responseMessage)
        {
            #region Preconditions
            Argument.IsNotNull( responseMessage, "responseMessage" );
            #endregion

            _responseMessages.Add( responseMessage );
        }

        public void Add(Response response)
        {
            #region Preconditions
            Argument.IsNotNull( response, "response" );
            Argument.IsValid( response != this, string.Format( "{0} can not be added to itself.", typeof( Response ) ), "response" );
            #endregion

            _responseMessages.AddMany(response._responseMessages);
        }

        public void AddInformation(string message)
        {
            AddInformation(string.Empty, message);
        }

        public void AddInformation(string key, string message)
        {
            _responseMessages.Add( new ResponseMessage( MessageType.Information, key, message ) );
        }

        public void AddWarning(string message)
        {
            AddWarning(string.Empty, message);
        }

        public void AddWarning(string key, string message)
        {
            _responseMessages.Add( new ResponseMessage( MessageType.Warning, key, message ) );
        }

        public void AddError(string message)
        {
            AddError(string.Empty, message);
        }

        public void AddError(string key, string message)
        {
            _responseMessages.Add( new ResponseMessage( MessageType.Error, key, message ) );
        }

        public void AddErrors(IEnumerable<string> errors)
        {
            #region Preconditions
            Argument.IsNotNull( errors, "errors" );
            #endregion

            _responseMessages.AddMany( errors.Select( error => new ResponseMessage( MessageType.Error, string.Empty, error ) ) );
        }

        public void InsertError(string key, string message)
        {
            _responseMessages.Insert( 0, new ResponseMessage( MessageType.Error, key, message ) );
        }

        private bool HasMessagesOfType(MessageType messageType)
        {
            return _responseMessages.Any( x => x.MessageType == messageType );
        }

        private IEnumerable<ResponseMessage> GetMessagesOfType(MessageType messageType)
        {
            return _responseMessages.Where( x => x.MessageType == messageType ).ToList().AsReadOnly();
        }
    }
}
