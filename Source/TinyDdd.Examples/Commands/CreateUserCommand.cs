using TinyDdd.Examples.Model;
using TinyDdd.Interaction;

namespace TinyDdd.Examples.Commands
{
    class CreateUserCommand : ICommand<Response<User>>
    {
        public string FirstName { get; set; }
    }
}
