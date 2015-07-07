using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Navigation;

namespace Pragmatic.Example.Client.Desktop.Pages
{
    public partial class UsersControl : IContent
    {
        public UsersControl()
        {
            InitializeComponent();
        }

        public void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
        }

        public void OnNavigatedFrom(NavigationEventArgs e)
        {
        }

        public void OnNavigatedTo(NavigationEventArgs e)
        {

        }
        
        public void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (e.Source.ToString() == "CreateNewUser")
            {
                var vm = DataContext as MainWindowViewModel;
                if (vm != null) vm.AddNewUserCommand.Execute(true);
                e.Cancel = true;
            }
        }
    }
}
