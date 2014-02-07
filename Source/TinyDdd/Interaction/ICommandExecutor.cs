namespace TinyDdd.Interaction
{
    public interface ICommandExecutor
    {
        TResponse Execute<TCommand, TResponse>(TCommand command) where TCommand : ICommand where TResponse : Response;
    }
}
