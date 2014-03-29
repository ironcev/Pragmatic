using Pragmatic.Interaction;

namespace Pragmatic.Example.Model.Users
{
    public sealed class AddNewUserCommand : ICommand<Response<User>>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsAdministrator { get; set; }
    }
}
