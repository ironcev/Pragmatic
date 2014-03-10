using Pragmatic.Interaction;

namespace Pragmatic.Example.Model.HowToUseQueriesAndCommands.Commands
{
    public sealed class SendEmailCommandHandler : ICommandHandler<SendEmailCommand, Response>
    {
        public Response Execute(SendEmailCommand command)
        {
            return new Response();
        }
    }
}
