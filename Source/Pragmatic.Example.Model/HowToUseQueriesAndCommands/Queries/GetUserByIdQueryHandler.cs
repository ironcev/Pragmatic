using Pragmatic.Interaction;

namespace Pragmatic.Example.Model.HowToUseQueriesAndCommands.Queries
{
    public sealed class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, User>
    {
        public User Execute(GetUserByIdQuery query)
        {
            return new User();
        }
    }
}
