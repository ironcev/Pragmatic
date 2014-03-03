using System;
using SwissKnife.Diagnostics.Contracts;
using TinyDdd.Example.Model;

namespace TinyDdd.Example.Client.Desktop
{
    internal class UserViewModel
    {
        internal Guid Id { get; private set; }
        public string FullName { get; private set; }
        public string Email { get; private set; }

        internal UserViewModel(User user)
        {
            Argument.IsNotNull(user, "user");

            Id = user.Id;
            FullName = user.FullName;
            Email = user.Email;
        }
    }
}
