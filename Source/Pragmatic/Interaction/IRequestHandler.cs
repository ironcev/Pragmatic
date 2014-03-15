namespace Pragmatic.Interaction
{
    public interface IRequestHandler<in TRequest, out TResponse> where TRequest : IRequest<TResponse> where TResponse : Response
    {
        TResponse Execute(TRequest request);
    }
}