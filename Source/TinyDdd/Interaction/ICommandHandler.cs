/*
 * NOTE: See the architectural decisions at the end of the file "ICommand.cs".
 */
namespace TinyDdd.Interaction
{
    // ReSharper disable UnusedTypeParameter
    public interface ICommandHandler<in TCommand> : ICommandHandler where TCommand : ICommand
    // ReSharper restore UnusedTypeParameter
    {
    }

    public interface ICommandHandler
    {
        Response Execute(ICommand command);
    }
}