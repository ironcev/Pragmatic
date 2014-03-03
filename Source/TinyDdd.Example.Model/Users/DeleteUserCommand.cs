using TinyDdd.Interaction;

namespace TinyDdd.Example.Model.Users
{
    public sealed class DeleteUserCommand : Command
    {
        public User User { get; set; }
    }
}
