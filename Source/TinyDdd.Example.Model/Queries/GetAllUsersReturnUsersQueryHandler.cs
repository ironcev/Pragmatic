using System.Collections.Generic;
using System.Linq;
using TinyDdd.Interaction;

namespace TinyDdd.Example.Model.Queries
{
    public class GetAllUsersReturnUsersQueryHandler : IQueryHandler<GetAllUsersQuery, IEnumerable<User>>
    {
        public IEnumerable<User> Execute(GetAllUsersQuery query)
        {
            return Enumerable.Empty<User>();
        }
    }
}
