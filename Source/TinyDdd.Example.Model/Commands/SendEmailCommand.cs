using TinyDdd.Interaction;

namespace TinyDdd.Example.Model.Commands
{
    public class SendEmailCommand : Command
    {
        public string EmailAddress { get; set; }
    }
}
