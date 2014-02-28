// TODO-iG: This is really ugly how we transfer data here! Find some time to prittify the example.
using System.Windows;
using ModelCommand = TinyDdd.Example.Model.Users;

namespace TinyDdd.Example.Client.Desktop.Dialogs
{
    public partial class UserProfileDialog // TODO-IG: Fix UI at resizing etc.
    {
        public UserProfileDialog()
        {
            InitializeComponent();

            _btnOk.Click += OnOkButtonClick;

            DataContext = new ModelCommand.AddNewUserCommand();
        }

        public ModelCommand.AddNewUserCommand UserProfile { get { return (ModelCommand.AddNewUserCommand) DataContext; } }

        private void OnOkButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
