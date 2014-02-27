using TinyDdd.Interaction;

namespace TinyDdd.Example.Model.HowToUseQueriesAndCommands.Commands
{
    public class SendEmailCommand : Command
    {
        public string EmailAddress { get; set; }
    }
}
