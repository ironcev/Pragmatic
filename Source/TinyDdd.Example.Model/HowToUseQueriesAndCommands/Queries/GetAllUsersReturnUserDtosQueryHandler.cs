using System.Collections.Generic;
using System.Linq;
using TinyDdd.Example.Model.HowToUseQueriesAndCommands.Dtos;
using TinyDdd.Interaction;

namespace TinyDdd.Example.Model.HowToUseQueriesAndCommands.Queries
{
    public class GetAllUsersReturnUserDtosQueryHandler : IQueryHandler<GetAllUsersQuery, IEnumerable<UserDto>>
    {
        public IEnumerable<UserDto> Execute(GetAllUsersQuery query)
        {
            return Enumerable.Empty<UserDto>();
        }
    }
}
