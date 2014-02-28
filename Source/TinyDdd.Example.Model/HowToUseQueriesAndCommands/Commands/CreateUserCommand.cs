using TinyDdd.Interaction;

namespace TinyDdd.Example.Model.HowToUseQueriesAndCommands.Commands
{
    public class CreateUserCommand : ICommand<Response<User>>
    {
        public string FirstName { get; set; }
    }
}
