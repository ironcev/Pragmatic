using System.Data.Entity;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.Example.EntityFramework
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
