using SwissKnife.Diagnostics.Contracts;
using TinyDdd.Interaction;

namespace TinyDdd.Example.Model
{
    public abstract class BaseCommandHandler
    {
        protected QueryExecutor QueryExecutor { get; private set; }
        protected IUnitOfWork UnitOfWork { get; private set; }

        protected BaseCommandHandler(QueryExecutor queryExecutor, IUnitOfWork unitOfWork)
        {
            Argument.IsNotNull(queryExecutor, "queryExecutor");
            Argument.IsNotNull(unitOfWork, "unitOfWork");

            QueryExecutor = queryExecutor;
            UnitOfWork = unitOfWork;
        }
    }
}