using SwissKnife.Diagnostics.Contracts;
using TinyDdd.Interaction;

namespace TinyDdd.Example.Client.Desktop.Commands
{
    class BaseCommand
    {
        protected CommandExecutor CommandExecutor { get; private set; }

        protected BaseCommand(CommandExecutor commandExecutor)
        {
            Argument.IsNotNull(commandExecutor, "commandExecutor");

            CommandExecutor = commandExecutor;
        }
    }
}
