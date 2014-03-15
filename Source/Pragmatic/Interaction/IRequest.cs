namespace Pragmatic.Interaction
{    
    public abstract class Request : IRequest<Response> { }

    // ReSharper disable UnusedTypeParameter
    public interface IRequest<in TResponse> where TResponse : Response
    // ReSharper restore UnusedTypeParameter
    {
    }
}