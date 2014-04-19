using System;
using Pragmatic.Example.Model;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.Example.Client.Desktop
{
    internal class UserViewModel
    {
        internal Guid Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string FullName { get; private set; }
        public string Email { get; private set; }

        internal UserViewModel(User user)
        {
            Argument.IsNotNull(user, "user");

            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            FullName = user.FullName;
            Email = user.Email;
        }
    }
}
