using System.Collections.Generic;
using System.Linq;
using Pragmatic.Example.Model.HowToUseQueriesAndCommands.Dtos;
using Pragmatic.Interaction;

namespace Pragmatic.Example.Model.HowToUseQueriesAndCommands.Queries
{
    public sealed class GetAllUsersReturnUserDtosQueryHandler : IQueryHandler<GetAllUsersQuery, IEnumerable<UserDto>>
    {
        public IEnumerable<UserDto> Execute(GetAllUsersQuery query)
        {
            return Enumerable.Empty<UserDto>();
        }
    }
}
