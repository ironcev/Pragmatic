using SwissKnife.Diagnostics.Contracts;
using TinyDdd.Interaction;

namespace TinyDdd.Example.Client.Desktop.Commands
{
    class BaseCommand
    {
        protected CommandExecutor CommandExecutor { get; private set; }
        protected QueryExecutor QueryExecutor { get; private set; }

        protected BaseCommand(CommandExecutor commandExecutor, QueryExecutor queryExecutor)
        {
            Argument.IsNotNull(commandExecutor, "commandExecutor");
            Argument.IsNotNull(queryExecutor, "queryExecutor");

            CommandExecutor = commandExecutor;
            QueryExecutor = queryExecutor;
        }
    }
}
