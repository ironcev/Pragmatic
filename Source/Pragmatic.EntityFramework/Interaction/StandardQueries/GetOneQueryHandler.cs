using System.Data.Entity;
using System.Linq;
using Pragmatic.Interaction;
using Pragmatic.Interaction.StandardQueries;
using SwissKnife;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.EntityFramework.Interaction.StandardQueries
{
    public sealed class GetOneQueryHandler<T> : BaseQueryHandler, IQueryHandler<GetOneQuery<T>, Option<T>> where T : class
    {
        public GetOneQueryHandler(DbContext dbContext) : base(dbContext)
        {
        }

        public Option<T> Execute(GetOneQuery<T> query)
        {
            Argument.IsNotNull(query, "query");

            IQueryable<T> queryable = DbContext.Set<T>();

            return queryable.OrderBy(query.OrderBy).Where(query.Criteria).SingleOrDefault();
        }
    }
}