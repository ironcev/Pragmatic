using TinyDdd.Examples.Model;
using TinyDdd.Interaction;

namespace TinyDdd.Examples.Commands
{
    public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand>
    {
        public Response Execute(ICommand command)
        {
            return new Response<User>(new User());
        }
    }
}
