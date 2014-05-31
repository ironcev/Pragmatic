// TODO-IG: Add Intentionally Bad Code warning!

using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Pragmatic.Example.Model;
using Pragmatic.Interaction;
using SwissKnife;
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

            // This query is (unfortunately) here because so far there are neither unit tests nor integration tests in Pragmatic and this is the only way to "test"
            // if non-generic version of the GetByIdQuery works.
            //if (users.Any())
            //{
            //    Option<object> firstUser = QueryExecutor.GetById(typeof (User), users.First().Id);
            //    MessageBox.Show(((User) firstUser.Value).FullName);
            //}
                
            _mainWindowViewModel.SetUsers(users);
        }

        public event EventHandler CanExecuteChanged;
    }
}
