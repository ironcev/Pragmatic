using System.Collections.Generic;
using System.Linq;
using TinyDdd.Examples.Dto;
using TinyDdd.Interaction;

namespace TinyDdd.Examples.Queries
{
    public class GetAllUsersReturnUserDtosQueryHandler : IQueryHandler<GetAllUsersQuery, IEnumerable<UserDto>>
    {
        public object Execute(IQuery query)
        {
            return Enumerable.Empty<UserDto>();
        }
    }
}
