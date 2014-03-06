using TinyDdd.Interaction;

namespace TinyDdd.Example.Model.HowToUseQueriesAndCommands.Commands
{
    public sealed class SendEmailCommand : Command
    {
        public string EmailAddress { get; set; }
    }
}
