using TinyDdd.Interaction;

namespace TinyDdd.Examples.Commands
{
    public class SendEmailCommandHandler : ICommandHandler<SendEmailCommand>
    {
        public Response Execute(ICommand command)
        {
            return new Response();
        }
    }
}
