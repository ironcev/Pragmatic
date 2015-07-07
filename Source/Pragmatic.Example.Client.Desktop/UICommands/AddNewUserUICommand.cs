// WARNING: 

using System;
using System.Windows.Controls;
using System.Windows.Input;
using Pragmatic.Example.Client.Desktop.Dialogs;
using Pragmatic.Example.Model;
using Pragmatic.Example.Model.Users;
using Pragmatic.Interaction;
using Pragmatic.Interaction.Caching;
using StructureMap;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.Example.Client.Desktop.UICommands
{
    class AddNewUserUICommand : BaseUICommand, ICommand // TODO-IG: Replace with appropriate classes from SwissKnife, once they are implemented.
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
                   Buttons = new Button[] { } //TODO-VKY: This will be removed when NuGet fix "ModernDialog ShowDialog always return false".
               };
           
            if (_mainWindowViewModel.SelectedUser != null)
            {
                userProfileDialog.UserProfile.FirstName = _mainWindowViewModel.SelectedUser.FirstName;
                userProfileDialog.UserProfile.LastName = _mainWindowViewModel.SelectedUser.LastName;
                userProfileDialog.UserProfile.Email = _mainWindowViewModel.SelectedUser.Email;
                userProfileDialog.Title = "Edit Profile" + "-" + _mainWindowViewModel.SelectedUser.FirstName;
            }

            // userProfileDialog.Buttons = new Button[] { userProfileDialog.OkButton, userProfileDialog.CancelButton };

            if (userProfileDialog.ShowDialog() != true) return;

            AddNewUserCommand addNewUserCommand = userProfileDialog.UserProfile;
            Response<User> response = CommandExecutor.Execute(addNewUserCommand);

            if (response.HasErrors)
                UserInteraction.ShowError("New user cannot be added.", response);
            else
            {
                var cache = ObjectFactory.Container.GetInstance<IQueryResultCache<GetUsersQuery, User[]>>();
                cache.InvalidateCacheFor(query => true); // Just a "test" since we don't have any other real tests :-(
                cache.InvalidatCacheForAllQueries();

                _mainWindowViewModel.GetAllUsersCommand.Execute(true);
                _mainWindowViewModel.SetSelectedUser();
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}
