using System.Collections.Generic;
using System.Linq;
using Pragmatic.Interaction;

namespace Pragmatic.Example.Model.HowToUseQueriesAndCommands.Queries
{
    public sealed class GetAllUsersReturnUsersQueryHandler : IQueryHandler<GetAllUsersQuery, IEnumerable<User>>
    {
        public IEnumerable<User> Execute(GetAllUsersQuery query)
        {
            return Enumerable.Empty<User>();
        }
    }
}
