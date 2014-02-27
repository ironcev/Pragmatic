// TODO-IG: Add Intentionally Bad Code warning!
using System;
using System.Windows.Input;
using TinyDdd.Example.Client.Desktop.Dialogs;
using TinyDdd.Example.Model;
using TinyDdd.Interaction;
using ModelCommand = TinyDdd.Example.Model.Users;

namespace TinyDdd.Example.Client.Desktop.Commands
{
    class AddNewUserCommand : BaseCommand, ICommand // TODO-IG: Replace with apporipriate classes from SwissKnife, once they are implemented.
    {
        public AddNewUserCommand(CommandExecutor commandExecutor, QueryExecutor queryExecutor) : base(commandExecutor, queryExecutor)
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

            ModelCommand.AddNewUserCommand addNewUserCommand = dialog.UserProfile;
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
