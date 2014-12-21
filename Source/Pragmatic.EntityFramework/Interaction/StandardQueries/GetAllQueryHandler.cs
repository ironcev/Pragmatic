using System.Data.Entity;
using System.Linq;
using Pragmatic.Interaction;
using Pragmatic.Interaction.StandardQueries;
using SwissKnife;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.EntityFramework.Interaction.StandardQueries
{
    public sealed class GetAllQueryHandler<T> : BaseQueryHandler, IQueryHandler<GetAllQuery<T>, IPagedEnumerable<T>> where T : class
    {
        public GetAllQueryHandler(DbContext dbContext) : base(dbContext) { }

        public IPagedEnumerable<T> Execute(GetAllQuery<T> query)
        {
            Argument.IsNotNull(query, "query");
            Argument.IsValid(!((query.OrderBy.IsNone || (query.OrderBy.IsSome && !query.OrderBy.Value.OrderByItems.Any())) &&
                              (query.Paging.IsSome && !query.Paging.Value.IsNone)), 
                             string.Format("The pagination is specified ({0}) in the query, but the ordering ({1}) is not. " +
                                           "Entity Framework supports pagination only if the ordering is specified. " +
                                           "Make sure that the ordering ({1}) is specified every time when a paginated result is requested.",
                                           Identifier.ToString(() => query.Paging),
                                           Identifier.ToString(() => query.OrderBy)),
                             "query");

            IQueryable<T> queryable = DbContext.Set<T>();

            queryable = queryable.OrderBy(query.OrderBy);

            if (query.Criteria.IsSome)
            {
                queryable = queryable.Where(query.Criteria.Value);
            }

            return queryable.ToPagedEnumerable(query.Paging);
        }
    }
}
