using Pragmatic.Interaction;

namespace Pragmatic.Example.Model.HowToUseQueriesAndCommands.Commands
{
    public sealed class SendEmailCommand : Command
    {
        public string EmailAddress { get; set; }
    }
}
