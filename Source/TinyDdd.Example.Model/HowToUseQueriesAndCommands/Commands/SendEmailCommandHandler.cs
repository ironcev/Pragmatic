using TinyDdd.Interaction;

namespace TinyDdd.Example.Model.HowToUseQueriesAndCommands.Commands
{
    public class SendEmailCommandHandler : ICommandHandler<SendEmailCommand, Response>
    {
        public Response Execute(SendEmailCommand command)
        {
            return new Response();
        }
    }
}
