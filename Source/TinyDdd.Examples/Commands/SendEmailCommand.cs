using TinyDdd.Interaction;

namespace TinyDdd.Examples.Commands
{
    class SendEmailCommand : Command
    {
        public string EmailAddress { get; set; }
    }
}
