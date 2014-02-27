using SwissKnife.Diagnostics.Contracts;
using TinyDdd.Example.Model;

namespace TinyDdd.Example.Client.Desktop
{
    internal class UserViewModel
    {
        public string FullName { get; private set; }
        public string Email { get; private set; }

        internal UserViewModel(User user)
        {
            Argument.IsNotNull(user, "user");

            FullName = user.FullName;
            Email = user.Email;
        }
    }
}
