using Pragmatic.Interaction;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.Example.Model
{
    public abstract class BaseInteractionHandler
    {
        protected QueryExecutor QueryExecutor { get; private set; }
        protected UnitOfWork UnitOfWork { get; private set; }

        protected BaseInteractionHandler(QueryExecutor queryExecutor, UnitOfWork unitOfWork)
        {
            Argument.IsNotNull(queryExecutor, "queryExecutor");
            Argument.IsNotNull(unitOfWork, "unitOfWork");

            QueryExecutor = queryExecutor;
            UnitOfWork = unitOfWork;
        }
    }
}