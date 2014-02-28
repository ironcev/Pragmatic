using System;
using TinyDdd.Interaction;

namespace TinyDdd.Example.Model.HowToUseQueriesAndCommands.Queries
{
    public class GetUserByIdQuery : IQuery<User>
    {
        public Guid Id { get; set; }
    }
}
