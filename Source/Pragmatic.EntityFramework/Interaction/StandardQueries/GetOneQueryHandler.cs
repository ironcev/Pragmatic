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
        public GetOneQueryHandler(DbContext dbContext) : base(dbContext) { }

        public Option<T> Execute(GetOneQuery<T> query)
        {
            Argument.IsNotNull(query, "query");

            IQueryable<T> queryable = DbContext.Set<T>();

            // TODO-HH: Single or First here? What if there's a several entities by this criteria?
            // TODO-IG: Good point. Current semantic is to return the first one! Therefore the query should be renamed to GetFirst.
            //          GetOne can remain, but it should throw exception if there are more than one. Or we can remove it completely.
            //return queryable.OrderBy(query.OrderBy).Where(query.Criteria).SingleOrDefault();
            return queryable.OrderByWithExpressionTransform(query.OrderBy).Where(query.Criteria).FirstOrDefault();
        }
    }
}