// TODO-IG: Add Intentionally Bad Code warning!

using System;
using System.Windows;
using System.Windows.Input;
using Pragmatic.Example.Model;
using Pragmatic.Example.Model.Users;
using Pragmatic.Interaction;
using SwissKnife;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.Example.Client.Desktop.UICommands
{
    class DeleteUserUICommand : BaseUICommand, ICommand // TODO-IG: Replace with apporipriate classes from SwissKnife, once they are implemented.
    {
        private readonly MainWindowViewModel _mainWindowViewModel; // TODO-IG: Should we have some bae class for such comamnds as well?

        public DeleteUserUICommand(MainWindowViewModel mainWindowViewModel, CommandExecutor commandExecutor, QueryExecutor queryExecutor)
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
            // WARNING:
            // In a real application we would expect that the delete command loads the User and executes business logic
            // that validates if the User can be deleted at all.
            // Te purpose of thi example is to show that interaction scope can begin and end in the high level client code as well.
            InteractionScope.BeginOrJoin();

            Option<UserViewModel> selectedUser = _mainWindowViewModel.Users.CurrentItem as UserViewModel;
            if (selectedUser.IsNone) return;

            Option<User> user = QueryExecutor.GetById<User>(selectedUser.Value.Id);
            if (user.IsNone) return;

            var deleteUser = UserInteraction.ShowQuestion(string.Format("Do you really want to delete the user {0}?", user.Value.FullName), MessageBoxButton.YesNo);
            if (deleteUser != MessageBoxResult.Yes) return;

            Response response = CommandExecutor.Execute(new DeleteUserCommand {User = user.Value});

            if (response.HasErrors)
                UserInteraction.ShowError("The user cannot be deleted.", response);
            else
                UserInteraction.ShowInformation("Selected user succesfully deleted.");

            // TODO-IG: Refresh the list.

            InteractionScope.End();
        }

        public event EventHandler CanExecuteChanged;
    }
}
