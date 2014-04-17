// TODO-IG: Add Intentionally Bad Code warning!

using System;
using System.Windows.Controls;
using System.Windows.Input;
using Pragmatic.Example.Client.Desktop.Dialogs;
using Pragmatic.Example.Model;
using Pragmatic.Example.Model.Users;
using Pragmatic.Interaction;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.Example.Client.Desktop.UICommands
{
    class AddNewUserUICommand : BaseUICommand, ICommand // TODO-IG: Replace with apporipriate classes from SwissKnife, once they are implemented.
    {
        private readonly MainWindowViewModel _mainWindowViewModel;

        public AddNewUserUICommand(MainWindowViewModel mainWindowViewModel, CommandExecutor commandExecutor, QueryExecutor queryExecutor, RequestExecutor requestExecutor)
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
            var userProfileDialog = new UserProfileDialog
               {
                   Title = "Edit Profile",
                   Buttons = new Button[] { } //TODO-VKY: This will be removed when NuGet fix "ModernDialog ShowDialog always return false"
               };

            // userProfileDialog.Buttons = new Button[] { userProfileDialog.OkButton, userProfileDialog.CancelButton };

            if (userProfileDialog.ShowDialog() != true) return;

            AddNewUserCommand addNewUserCommand = userProfileDialog.UserProfile;
            Response<User> response = CommandExecutor.Execute(addNewUserCommand);

            if (response.HasErrors)
                UserInteraction.ShowError("New user cannot be added.", response);

            _mainWindowViewModel.GetAllUsersCommand.Execute(true);
            _mainWindowViewModel.SetSelectedUser();
        }

        public event EventHandler CanExecuteChanged;
    }
}
