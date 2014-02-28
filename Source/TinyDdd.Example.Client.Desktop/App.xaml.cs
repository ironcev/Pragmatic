using System.Windows;

namespace TinyDdd.Example.Client.Desktop
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
