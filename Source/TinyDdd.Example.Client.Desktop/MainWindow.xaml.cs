namespace TinyDdd.Example.Client.Desktop
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            DataContext = new MainWindowViewModel();
        }
    }
}
