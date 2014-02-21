using SwissKnife.Diagnostics.Contracts;

namespace TinyDdd.Interaction
{
    public enum MessageType
    {
        Information,
        Warning,
        Error,
        TechnicalError // TODO-IG: Do we need this? No.
    }

    public class ResponseMessage
    {
        public MessageType MessageType { get; private set; }

        public string Key { get; private set; }

        public string Message { get; private set; }

        public ResponseMessage(MessageType messageType, string message) : this(messageType, string.Empty, message) { }

        public ResponseMessage(MessageType messageType, string key, string message)
        {
            Argument.IsNotNull(key, "key");
            Argument.IsNotNullOrWhitespace(message, "message");

            MessageType = messageType;
            Key = key;
            Message = message;
        }
    }
}
