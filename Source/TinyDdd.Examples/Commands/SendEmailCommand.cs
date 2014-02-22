using TinyDdd.Interaction;

namespace TinyDdd.Examples.Commands
{
    public class SendEmailCommand : Command
    {
        public string EmailAddress { get; set; }
    }
}
