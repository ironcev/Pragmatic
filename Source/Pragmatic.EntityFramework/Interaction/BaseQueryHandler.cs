using System.Data.Entity;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.EntityFramework.Interaction
{
    public abstract class BaseQueryHandler
    {
        protected DbContext DbContext { get; private set; }

        protected BaseQueryHandler(DbContext dbContext)
        {
            Argument.IsNotNull(dbContext, "dbContext");

            DbContext = dbContext;
        }
    }
}