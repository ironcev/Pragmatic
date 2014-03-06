using TinyDdd.Interaction;

namespace TinyDdd.Example.Model.HowToUseQueriesAndCommands.Commands
{
    public sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Response<User>>
    {
        public Response<User> Execute(CreateUserCommand command)
        {
            return new Response<User>(new User());
        }
    }
}
