using System;
using System.Windows;
using System.Windows.Input;
using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Controls;
using FirstFloor.ModernUI.Windows.Navigation;

namespace Pragmatic.Example.Client.Desktop.Pages
{
    /// <summary>
    /// Interaction logic for UserControl.xaml
    /// </summary>
    public partial class UsersControl : IContent
    {
        public UsersControl()
        {
            InitializeComponent();
        }

        // This is called when navigation to a content fragment begins. Fragment navigation happens when you navigate to a link uri containing a fragment (use the # character).
        public void OnFragmentNavigation(FragmentNavigationEventArgs e)
        {
            var ss = e.Fragment;
        }

        // This is called when the content is no longer the active content.
        public void OnNavigatedFrom(NavigationEventArgs e)
        {
           // NavigationCommands.GoToPage.Execute("/Pages/UsersControl.xaml", null);

        }

        // This is invoked when the content becomes the active content in a frame. This is a good time to initialize your content.
        public void OnNavigatedTo(NavigationEventArgs e)
        {

        }
        
        // This is invoked when the content is about to become inactive.
        public void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (e.Source != null) return;
            var vm = DataContext as MainWindowViewModel;
            if (vm != null) vm.AddNewUserCommand.Execute(true);
            e.Cancel = true; // For cancel navigation.
        }
    }
}
