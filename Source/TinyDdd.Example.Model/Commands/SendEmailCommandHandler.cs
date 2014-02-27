using TinyDdd.Interaction;

namespace TinyDdd.Example.Model.Commands
{
    public class SendEmailCommandHandler : ICommandHandler<SendEmailCommand, Response>
    {
        public Response Execute(SendEmailCommand command)
        {
            return new Response();
        }
    }
}
