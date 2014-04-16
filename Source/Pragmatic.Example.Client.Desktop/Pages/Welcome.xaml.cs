using System.Windows;
using FirstFloor.ModernUI.Windows;
using FirstFloor.ModernUI.Windows.Navigation;

namespace Pragmatic.Example.Client.Desktop.Pages
{
    /// <summary>
    /// Interaction logic for Welcome.xaml
    /// </summary>
    public partial class Welcome : IContent
    {
        public Welcome()
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
            if (e.Source != null) return;
            var parentWindow = Window.GetWindow(this);
            if (parentWindow != null) parentWindow.Close();
        }
    }
}
