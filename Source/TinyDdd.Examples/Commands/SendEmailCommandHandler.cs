using TinyDdd.Interaction;

namespace TinyDdd.Examples.Commands
{
    public class SendEmailCommandHandler : ICommandHandler<SendEmailCommand, Response>
    {
        public Response Execute(SendEmailCommand command)
        {
            return new Response();
        }
    }
}
