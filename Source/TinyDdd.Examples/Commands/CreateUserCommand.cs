using TinyDdd.Examples.Model;
using TinyDdd.Interaction;

namespace TinyDdd.Examples.Commands
{
    public class CreateUserCommand : ICommand<Response<User>>
    {
        public string FirstName { get; set; }
    }
}
