// TODO-IG: Add Intentionally Bad Code warning!

using System;
using System.Windows.Input;
using Pragmatic.Example.Client.Desktop.Dialogs;
using Pragmatic.Example.Model;
using Pragmatic.Example.Model.Users;
using Pragmatic.Interaction;

namespace Pragmatic.Example.Client.Desktop.UICommands
{
    class AddNewUserUICommand : BaseUICommand, ICommand // TODO-IG: Replace with apporipriate classes from SwissKnife, once they are implemented.
    {
        public AddNewUserUICommand(CommandExecutor commandExecutor, QueryExecutor queryExecutor) : base(commandExecutor, queryExecutor)
        {
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            UserProfileDialog dialog = new UserProfileDialog();
            if (dialog.ShowDialog() != true) return;

            AddNewUserCommand addNewUserCommand = dialog.UserProfile;
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
