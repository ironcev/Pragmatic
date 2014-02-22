using TinyDdd.Examples.Model;
using TinyDdd.Interaction;

namespace TinyDdd.Examples.Queries
{
    public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, User>
    {
        public object Execute(IQuery query)
        {
            return new User();
        }
    }
}
