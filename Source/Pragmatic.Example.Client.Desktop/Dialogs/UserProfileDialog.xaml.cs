// TODO-iG: This is really ugly how we transfer data here! Find some time to prettify the example.

using System.Windows;
using Pragmatic.Example.Model.Users;

namespace Pragmatic.Example.Client.Desktop.Dialogs
{
    public partial class UserProfileDialog // TODO-IG: Fix UI at resizing etc.
    {
        public UserProfileDialog()
        {
            InitializeComponent();

            _btnOk.Click += OnOkButtonClick;

            DataContext = new AddNewUserCommand();
        }

        public AddNewUserCommand UserProfile { get { return (AddNewUserCommand) DataContext; } }

        private void OnOkButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
