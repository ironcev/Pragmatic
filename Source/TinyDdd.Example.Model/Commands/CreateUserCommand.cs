using TinyDdd.Interaction;

namespace TinyDdd.Example.Model.Commands
{
    public class CreateUserCommand : ICommand<Response<User>>
    {
        public string FirstName { get; set; }
    }
}
