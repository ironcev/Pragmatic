using Pragmatic.Interaction;

namespace Pragmatic.Example.Model.Users
{
    public sealed class DeleteUserCommand : Command
    {
        public User User { get; set; }
    }
}
