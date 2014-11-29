using System.Data.Entity;
using System.Linq;
using Pragmatic.Interaction;
using Pragmatic.Interaction.StandardQueries;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.EntityFramework.Interaction.StandardQueries
{
    public sealed class GetTotalCountQueryHandler<T> : BaseQueryHandler, IQueryHandler<GetTotalCountQuery<T>, int> where T : class
    {
        public GetTotalCountQueryHandler(DbContext dbContext) : base(dbContext)
        {
        }

        public int Execute(GetTotalCountQuery<T> query)
        {
            Argument.IsNotNull(query, "query");

            return query.Criteria.IsSome ? DbContext.Set<T>().Where(query.Criteria.Value).Count() : DbContext.Set<T>().Count();
        }
    }
}