using Pragmatic.Interaction;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.Example.Client.Desktop.UICommands
{
    class BaseUICommand
    {
        protected CommandExecutor CommandExecutor { get; private set; }
        protected QueryExecutor QueryExecutor { get; private set; }
        public RequestExecutor RequestExecutor { get; private set; }

        protected BaseUICommand(CommandExecutor commandExecutor, QueryExecutor queryExecutor, RequestExecutor requestExecutor)
        {
            Argument.IsNotNull(commandExecutor, "commandExecutor");
            Argument.IsNotNull(queryExecutor, "queryExecutor");
            Argument.IsNotNull(requestExecutor, "requestExecutor");

            CommandExecutor = commandExecutor;
            QueryExecutor = queryExecutor;
            RequestExecutor = requestExecutor;
        }
    }
}
