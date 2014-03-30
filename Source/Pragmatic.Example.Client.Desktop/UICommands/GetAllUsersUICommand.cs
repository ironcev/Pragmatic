// TODO-IG: Add Intentionally Bad Code warning!

using System;
using System.Windows.Input;
using Pragmatic.Example.Model;
using Pragmatic.Interaction;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.Example.Client.Desktop.UICommands
{
    class GetAllUsersUICommand : BaseUICommand, ICommand // TODO-IG: Replace with appropriate classes from SwissKnife, once they are implemented.
    {
        private readonly MainWindowViewModel _mainWindowViewModel;

        public GetAllUsersUICommand(MainWindowViewModel mainWindowViewModel, CommandExecutor commandExecutor, QueryExecutor queryExecutor, RequestExecutor requestExecutor)
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
            // This query returns all users without any particular sorting.
            var users = QueryExecutor.GetAll<User>();

            // This query returns paginated result. The first page is returned with maximum 10 users. The users are sorted by their first name and then by email.
            //var users = QueryExecutor.GetAll(new OrderBy<User>(user => user.FirstName).ThenBy(user => user.Email, OrderByDirection.Descending), new Paging(1, 10));

            _mainWindowViewModel.SetUsers(users);
        }

        public event EventHandler CanExecuteChanged;
    }
}
