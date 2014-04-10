using System;
using System.Data.Entity;
using System.Linq;
using Pragmatic.Interaction;
using Pragmatic.Interaction.StandardQueries;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.EntityFramework.Interaction.StandardQueries
{
    public sealed class GetAllQueryHandlerHandler<T> : BaseQueryHandler, IQueryHandler<GetAllQuery<T>, IPagedEnumerable<T>> where T : class
    {
        public GetAllQueryHandlerHandler(DbContext dbContext) : base(dbContext)
        {
        }

        public IPagedEnumerable<T> Execute(GetAllQuery<T> query)
        {
            Argument.IsNotNull(query, "query");

            IQueryable<T> queryable = DbContext.Set<T>();

            queryable = queryable.OrderBy(query.OrderBy);

            if (query.Criteria.IsSome)
            {
                queryable = queryable.Where(query.Criteria.Value);
            }

            int total = queryable.Count();

            if (query.Paging.IsSome && !query.Paging.Value.IsNone)
            {
                queryable = queryable.Skip(query.Paging.Value.Skip).Take(query.Paging.Value.PageSize);
            }
            
            return new PagedList<T>(queryable, 0, Int32.MaxValue, total);
        }
    }
}
