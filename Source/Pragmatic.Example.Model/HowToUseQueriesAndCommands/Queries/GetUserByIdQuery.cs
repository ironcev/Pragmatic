using System;
using Pragmatic.Interaction;

namespace Pragmatic.Example.Model.HowToUseQueriesAndCommands.Queries
{
    public sealed class GetUserByIdQuery : IQuery<User>
    {
        public Guid Id { get; set; }
    }
}
