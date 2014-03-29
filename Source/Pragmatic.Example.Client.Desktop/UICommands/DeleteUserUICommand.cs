// TODO-IG: Add Intentionally Bad Code warning!

using System;
using System.Windows;
using System.Windows.Input;
using Pragmatic.Example.Model;
using Pragmatic.Interaction;
using SwissKnife;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.Example.Client.Desktop.UICommands
{
    class DeleteUserUICommand : BaseUICommand, ICommand // TODO-IG: Replace with appropriate classes from SwissKnife, once they are implemented.
    {
        private readonly MainWindowViewModel _mainWindowViewModel; // TODO-IG: Should we have some base class for such commands as well?

        public DeleteUserUICommand(MainWindowViewModel mainWindowViewModel, CommandExecutor commandExecutor, QueryExecutor queryExecutor, RequestExecutor requestExecutor)
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
            Option<UserViewModel> selectedUser = _mainWindowViewModel.Users.CurrentItem as UserViewModel;
            if (selectedUser.IsNone) return;

            var response = RequestExecutor.CanDeleteEntity<User>(selectedUser.Value.Id);
            if (response.HasErrors)
            {
                UserInteraction.ShowError("The user cannot be deleted because of the following reasons:", response);
                return;
            }

            // If there are no errors, we know that we got the entity back.
            User userToDelete = response.Result.Value;

            if (response.HasInformationOrWarnings)
            {
                var deleteUser = UserInteraction.ShowResponse(string.Format("Do you really want to delete the user {0}?", userToDelete.FullName), response, MessageBoxButton.YesNo);
                if (deleteUser != MessageBoxResult.Yes) return;
            }


            CommandExecutor.DeleteEntity(userToDelete);

            UserInteraction.ShowInformation("Selected user successfully deleted.");

            //// TODO-IG: Refresh the list.
        }

        public event EventHandler CanExecuteChanged;
    }
}
