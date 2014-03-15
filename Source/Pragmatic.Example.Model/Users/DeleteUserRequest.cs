using Pragmatic.Interaction;

namespace Pragmatic.Example.Model.Users
{
    public class DeleteUserRequest : Request
    {
        public User User { get; set; }
    }
}
