// TODO-IG: Add Intentionally Bad Code warning!
using System;
using System.Windows.Input;
using SwissKnife.Diagnostics.Contracts;
using TinyDdd.Example.Model;
using TinyDdd.Interaction;
using ModelCommand = TinyDdd.Example.Model.Users;

namespace TinyDdd.Example.Client.Desktop.Commands
{
    class GetAllUsersCommand : BaseCommand, ICommand // TODO-IG: Replace with apporipriate classes from SwissKnife, once they are implemented.
    {
        private readonly MainWindowViewModel _mainWindowViewModel;

        public GetAllUsersCommand(MainWindowViewModel mainWindowViewModel, CommandExecutor commandExecutor, QueryExecutor queryExecutor)
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
            _mainWindowViewModel.SetUsers(QueryExecutor.GetAll<User>());
        }

        public event EventHandler CanExecuteChanged;
    }
}
