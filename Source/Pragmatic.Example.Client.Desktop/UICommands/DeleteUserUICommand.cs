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

            // Comment/uncomment these two lines to switch between deleting over generic method or over the method that accepts entity type.
            var response = RequestExecutor.CanDeleteEntity<User>(selectedUser.Value.Id);
            //var response = RequestExecutor.CanDeleteEntity(typeof(User), selectedUser.Value.Id);
            if (response.HasErrors)
            {
                UserInteraction.ShowError("The user cannot be deleted because of the following reasons:", response);
                return;
            }

            // If there are no errors, we know that we got the entity back.

            // Comment/uncomment these two lines to switch between deleting over generic method or over the method that accepts entity type.
            User userToDelete = response.Result.Value;
            //Entity userToDelete = response.Result.Value;

            if (response.HasInformationOrWarnings)
            {
                var deleteUser = UserInteraction.ShowResponse("Do you really want to delete the user?", response, MessageBoxButton.YesNo);
                if (deleteUser != MessageBoxResult.Yes) return;
            }

            // An entity can be deleted by providing the entity itself.
            // Since the compiler will properly infer the type, this line stays the same in switching between deleting over generic method or over the method that accepts base entity type.
            //CommandExecutor.DeleteEntity(userToDelete);
            // Alternatively, its type and id can be provided.
            //CommandExecutor.DeleteEntity<User>(selectedUser.Value.Id);
            CommandExecutor.DeleteEntity(typeof(User), selectedUser.Value.Id);


            UserInteraction.ShowInformation("Selected user successfully deleted.");

            //// TODO-IG: Refresh the list.
        }

        public event EventHandler CanExecuteChanged;
    }
}
