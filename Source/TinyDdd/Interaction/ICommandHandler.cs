namespace TinyDdd.Interaction
{
    public interface ICommandHandler<in TCommand, out TResponse> where TCommand : ICommand where TResponse : Response
    {
        TResponse Execute(TCommand command);
    }
}
