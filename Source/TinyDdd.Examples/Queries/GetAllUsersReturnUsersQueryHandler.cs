using System.Collections.Generic;
using System.Linq;
using TinyDdd.Examples.Model;
using TinyDdd.Interaction;

namespace TinyDdd.Examples.Queries
{
    public class GetAllUsersReturnUsersQueryHandler : IQueryHandler<GetAllUsersQuery, IEnumerable<User>>
    {
        public object Execute(IQuery query)
        {
            return Enumerable.Empty<User>();
        }
    }
}
