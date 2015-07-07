using System.Data.Entity;
using System.Linq;
using Pragmatic.Example.Model;
using Pragmatic.Example.Model.Users;
using Pragmatic.Interaction;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.Example.EntityFramework
{
    public class GetUsersQueryHandler : BaseQueryHandler, IQueryHandler<GetUsersQuery, User[]>
    {
        public GetUsersQueryHandler(DbContext dbContext) : base(dbContext)
        {            
        }

        public User[] Execute(GetUsersQuery query)
        {
            Argument.IsNotNull(query, "query");

            IQueryable<User> userQuery = DbContext.Set<User>();

            string searchTerm = GetUsersQuery.NormalizeSeachTerm(query.SearchTerm);
            if (!string.IsNullOrEmpty(query.SearchTerm))
                userQuery = userQuery.Where(user => user.Email.Contains(searchTerm) || user.FirstName.Contains(searchTerm) || user.LastName.Contains(searchTerm));

            if (query.IsAdministrator != null)
                userQuery = userQuery.Where(user => user.IsAdministrator == query.IsAdministrator.Value);

            return userQuery.ToArray();
        }
    }
}
