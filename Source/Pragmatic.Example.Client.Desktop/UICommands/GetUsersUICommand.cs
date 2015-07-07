// !!! WARNING: Intentionally Bad Code !!!
using System;
using System.Windows.Input;
using Pragmatic.Example.Model.Users;
using Pragmatic.Interaction;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.Example.Client.Desktop.UICommands
{
    class GetUsersUICommand : BaseUICommand, ICommand // TODO-IG: Replace with appropriate classes from SwissKnife, once they are implemented.
    {
        private readonly MainWindowViewModel _mainWindowViewModel;

        public GetUsersUICommand(MainWindowViewModel mainWindowViewModel, CommandExecutor commandExecutor, QueryExecutor queryExecutor, RequestExecutor requestExecutor)
            : base(commandExecutor, queryExecutor, requestExecutor)
        {
            Argument.IsNotNull(mainWindowViewModel, "mainWindowViewModel");

            _mainWindowViewModel = mainWindowViewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var users = QueryExecutor.Execute(new GetUsersQuery { SearchTerm = (string)parameter });
                
            _mainWindowViewModel.SetUsers(users);
        }

        public event EventHandler CanExecuteChanged;
    }
}
