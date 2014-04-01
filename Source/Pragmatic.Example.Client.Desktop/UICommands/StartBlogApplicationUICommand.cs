using System;
using System.Windows.Input;
using Pragmatic.Interaction;

namespace Pragmatic.Example.Client.Desktop.UICommands
{
    class StartBlogApplicationUICommand: BaseUICommand, ICommand // TODO-IG: Replace with apporipriate classes from SwissKnife, once they are implemented.
    {
        public StartBlogApplicationUICommand(CommandExecutor commandExecutor, QueryExecutor queryExecutor, RequestExecutor requestExecutor)
            : base(commandExecutor, queryExecutor, requestExecutor)
        {
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            //var dialog = new BlogApplicationControl();
            //if (dialog.ShowDialog() != true) return;
        }

        public event EventHandler CanExecuteChanged;
    }
}
