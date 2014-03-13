// TODO-IG: Add Intentionally Bad Code warning!

using System;
using System.Windows.Input;
using Pragmatic.Example.Model;
using Pragmatic.Interaction;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.Example.Client.Desktop.UICommands
{
    class GetAllUsersUICommand : BaseUICommand, ICommand // TODO-IG: Replace with apporipriate classes from SwissKnife, once they are implemented.
    {
        private readonly MainWindowViewModel _mainWindowViewModel;

        public GetAllUsersUICommand(MainWindowViewModel mainWindowViewModel, CommandExecutor commandExecutor, QueryExecutor queryExecutor)
            : base(commandExecutor, queryExecutor)
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
            //_mainWindowViewModel.SetUsers(QueryExecutor.GetAll<User>());
            _mainWindowViewModel.SetUsers(QueryExecutor.GetAll(new OrderBy<User>(user => user.FirstName).ThenBy(user => user.Email, OrderByDirection.Descending), new Paging(1, 2)));
        }

        public event EventHandler CanExecuteChanged;
    }
}
