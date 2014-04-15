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

            // Some units of work do not support deleting entities which are not loaded in the same interaction scope in which the deleting happens.
            // RavenDB is such example. On the other hand, NHibernate supports this.
            // In other words, if you try to use this line (see below):
            //     Response userDeletedResponse = CommandExecutor.DeleteEntity(userToDelete);
            // to delete an already loaded entity, this will work with NHibernate, but it will not work with RavenDB.
            // To make it work with RavenDB, we have to put both actions, fetching of the entity and its deletion in the same interaction scope.
            // You can comment out beginning and ending of the interaction scope in case you are:
            //   * Using NHibernate.
            //   * Using RavenDB, but instead of CommandExecutor.DeleteEntity(userToDelete) you use
            //     either CommandExecutor.DeleteEntity<User>(selectedUser.Value.Id) or CommandExecutor.DeleteEntity(typeof(User), selectedUser.Value.Id).
            InteractionScope.BeginOrJoin();

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
            Response userDeletedResponse = CommandExecutor.DeleteEntity(userToDelete);
            InteractionScope.End();
            // Alternatively, its type and id can be provided.
            //Response userDeletedResponse = CommandExecutor.DeleteEntity<User>(selectedUser.Value.Id);
            //Response userDeletedResponse = CommandExecutor.DeleteEntity(typeof(User), selectedUser.Value.Id);


            if (userDeletedResponse.HasErrors)
                UserInteraction.ShowError("The user cannot be deleted because of the following reasons:", userDeletedResponse);
            else
                UserInteraction.ShowInformation("Selected user successfully deleted.");

           _mainWindowViewModel.GetAllUsersCommand.Execute(true);
        }

        public event EventHandler CanExecuteChanged;
    }
}
