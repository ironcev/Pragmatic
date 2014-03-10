namespace Pragmatic.Interaction
{
    public interface ICommandHandler<in TCommand, out TResponse> where TCommand : ICommand<TResponse> where TResponse : Response
    {
        TResponse Execute(TCommand command);
    }
}