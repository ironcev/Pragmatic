using System.Data.Entity;
using System.Linq;
using Pragmatic.Interaction;
using Pragmatic.Interaction.StandardQueries;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.EntityFramework.Interaction.StandardQueries
{
    public sealed class GetAllQueryHandler<T> : BaseQueryHandler, IQueryHandler<GetAllQuery<T>, IPagedEnumerable<T>> where T : class
    {
        public GetAllQueryHandler(DbContext dbContext) : base(dbContext) { }

        public IPagedEnumerable<T> Execute(GetAllQuery<T> query)
        {
            Argument.IsNotNull(query, "query");

            IQueryable<T> queryable = DbContext.Set<T>();

            queryable = queryable.OrderBy(query.OrderBy);

            if (query.Criteria.IsSome)
            {
                queryable = queryable.Where(query.Criteria.Value);
            }

            if (query.Paging.IsSome && !query.Paging.Value.IsNone)
            {
                queryable = queryable.Skip(query.Paging.Value.Skip).Take(query.Paging.Value.PageSize);
            }

            return queryable.ToPagedEnumerable(query.Paging);
        }
    }
}
