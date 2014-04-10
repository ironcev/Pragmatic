// TODO-IG: Add Intentionally Bad Code warning!

using System;
using System.Windows.Controls;
using System.Windows.Input;
using Pragmatic.Example.Client.Desktop.Dialogs;
using Pragmatic.Example.Model;
using Pragmatic.Example.Model.Users;
using Pragmatic.Interaction;

namespace Pragmatic.Example.Client.Desktop.UICommands
{
    class AddNewUserUICommand : BaseUICommand, ICommand // TODO-IG: Replace with apporipriate classes from SwissKnife, once they are implemented.
    {
        public AddNewUserUICommand(CommandExecutor commandExecutor, QueryExecutor queryExecutor, RequestExecutor requestExecutor)
            : base(commandExecutor, queryExecutor, requestExecutor)
        {
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
            else
                UserInteraction.ShowInformation("New user succesfully added.");

            // TODO-IG: Display the user.
        }

        public event EventHandler CanExecuteChanged;
    }
}
