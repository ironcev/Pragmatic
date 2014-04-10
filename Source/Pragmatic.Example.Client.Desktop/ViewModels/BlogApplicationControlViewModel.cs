using System;
using Pragmatic.Example.Model;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.Example.Client.Desktop
{
    internal class BlogApplicationControlViewModel
    {
        internal BlogApplicationControlViewModel(User user)
        {
            Argument.IsNotNull(user, "user");
        }
    }
}
