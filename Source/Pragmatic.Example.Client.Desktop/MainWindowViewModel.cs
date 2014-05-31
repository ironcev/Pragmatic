using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Input;
using Pragmatic.Example.Client.Desktop.UICommands;
using Pragmatic.Example.Model;
using StructureMap;
using StructureMap.Pipeline;
using SwissKnife.Collections;

namespace Pragmatic.Example.Client.Desktop
{
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        public ICommand AddNewUserCommand { get; private set; }
        public ICommand GetAllUsersCommand { get; private set; }
        public ICommand DeleteUserCommand { get; private set; }

        private readonly ObservableCollection<UserViewModel> _users = new ObservableCollection<UserViewModel>();
        public ICollectionView Users { get; private set; }

        private UserViewModel _selectedUser;
        public UserViewModel SelectedUser
        {
            get { return _selectedUser; }
            set { _selectedUser = value; OnPropertyChanged("SelectedUser"); }
        }

        internal MainWindowViewModel()
        {
            CreateCommands();

            Users = CollectionViewSource.GetDefaultView(_users);
        }

        internal void SetSelectedUser()
        {
            SelectedUser = _users.Last();
        }

        internal void SetUsers(IEnumerable<User> users)
        {
            _users.Clear();
            _users.AddMany(users.Select(user => new UserViewModel(user)));
        }

        private void CreateCommands()
        {
            AddNewUserCommand = ObjectFactory.Container.GetInstance<AddNewUserUICommand>(new ExplicitArguments(new Dictionary<string, object> { { "mainWindowViewModel", this } }));
            GetAllUsersCommand = ObjectFactory.Container.GetInstance<GetAllUsersUICommand>(new ExplicitArguments(new Dictionary<string, object> { { "mainWindowViewModel", this } }));
            DeleteUserCommand = ObjectFactory.Container.GetInstance<DeleteUserUICommand>(new ExplicitArguments(new Dictionary<string, object> { { "mainWindowViewModel", this } }));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
