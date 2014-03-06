using TinyDdd.Interaction;

namespace TinyDdd.Example.Model.HowToUseQueriesAndCommands.Queries
{
    public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, User>
    {
        public User Execute(GetUserByIdQuery query)
        {
            return new User();
        }
    }
}
