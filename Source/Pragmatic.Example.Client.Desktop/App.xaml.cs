using System.Windows;

namespace Pragmatic.Example.Client.Desktop
{
    public partial class App
    {
        public App()
        {
            Startup += OnStartup;
        }

        private static void OnStartup(object sender, StartupEventArgs e)
        {
            ContainerRegistry.Initialize();
        }
    }
}
