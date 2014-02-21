namespace TinyDdd.Interaction
{
    public interface ICommandHandler<in TCommand> : ICommandHandler where TCommand : ICommand
    {
        Response Execute(TCommand command);
    }

    public interface ICommandHandler
    {
        Response Execute(ICommand command);
    }
}