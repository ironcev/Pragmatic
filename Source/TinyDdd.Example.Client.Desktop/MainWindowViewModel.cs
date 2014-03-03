using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using StructureMap;
using StructureMap.Pipeline;
using TinyDdd.Example.Client.Desktop.UICommands;
using TinyDdd.Example.Model;
using SwissKnife.Collections;

namespace TinyDdd.Example.Client.Desktop
{
    internal class MainWindowViewModel
    {
        public ICommand AddNewUserCommand { get; private set; }
        public ICommand GetAllUsersCommand { get; private set; }
        public ICommand DeleteUserCommand { get; private set; }

        private readonly ObservableCollection<UserViewModel> _users = new ObservableCollection<UserViewModel>();
        public ICollectionView Users { get; private set; }

        internal MainWindowViewModel()
        {
            CreateCommands();

            Users = CollectionViewSource.GetDefaultView(_users);
        }

        internal void SetUsers(IEnumerable<User> users)
        {
            _users.Clear();
            _users.AddMany(users.Select(user => new UserViewModel(user)));
        }

        private void CreateCommands()
        {
            AddNewUserCommand = ObjectFactory.GetInstance<AddNewUserUICommand>();
            GetAllUsersCommand = ObjectFactory.GetInstance<GetAllUsersUICommand>(new ExplicitArguments(new Dictionary<string, object>{ {"mainWindowViewModel", this}}));
            DeleteUserCommand = ObjectFactory.GetInstance<DeleteUserUICommand>(new ExplicitArguments(new Dictionary<string, object> { { "mainWindowViewModel", this } }));
        }
    }
}
