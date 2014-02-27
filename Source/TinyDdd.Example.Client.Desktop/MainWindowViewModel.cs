using System.Windows.Input;
using StructureMap;
using TinyDdd.Example.Client.Desktop.Commands;

namespace TinyDdd.Example.Client.Desktop
{
    class MainWindowViewModel
    {
        public ICommand AddNewUserCommand { get; private set; }

        public MainWindowViewModel()
        {
            CreateCommands();
        }

        private void CreateCommands()
        {
            AddNewUserCommand = ObjectFactory.GetInstance<AddNewUserCommand>();
        }
    }
}
