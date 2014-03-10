using Pragmatic.Interaction;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.Example.Client.Desktop.UICommands
{
    class BaseUICommand
    {
        protected CommandExecutor CommandExecutor { get; private set; }
        protected QueryExecutor QueryExecutor { get; private set; }

        protected BaseUICommand(CommandExecutor commandExecutor, QueryExecutor queryExecutor)
        {
            Argument.IsNotNull(commandExecutor, "commandExecutor");
            Argument.IsNotNull(queryExecutor, "queryExecutor");

            CommandExecutor = commandExecutor;
            QueryExecutor = queryExecutor;
        }
    }
}
