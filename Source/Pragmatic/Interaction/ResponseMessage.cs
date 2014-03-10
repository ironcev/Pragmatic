using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.Interaction
{
    public enum MessageType
    {
        Success,
        Information,
        Warning,
        Error
    }

    public class ResponseMessage // TODO-IG: Mark it as DeepImmutable once when this attribute is available in SwissKnife.
    {
        public MessageType MessageType { get; private set; }

        public string Key { get; private set; }

        public string Message { get; private set; }

        public bool HasKey { get { return !string.IsNullOrWhiteSpace(Key); } }

        public ResponseMessage(MessageType messageType, string message) : this(messageType, message, string.Empty) { }

        public ResponseMessage(MessageType messageType, string message, string key)
        {
            Argument.IsNotNullOrWhitespace(message, "message");
            Argument.IsNotNull(key, "key");

            MessageType = messageType;
            Key = key;
            Message = message;
        }

        public override string ToString()
        {
            return string.Format("[{0}] {1} [{2}]", MessageType, Message, Key);
        }
    }
}
