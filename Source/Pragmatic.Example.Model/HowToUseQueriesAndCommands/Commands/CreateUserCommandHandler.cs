using Pragmatic.Interaction;

namespace Pragmatic.Example.Model.HowToUseQueriesAndCommands.Commands
{
    public sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Response<User>>
    {
        public Response<User> Execute(CreateUserCommand command)
        {
            return new Response<User>(new User());
        }
    }
}
