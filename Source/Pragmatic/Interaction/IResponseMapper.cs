namespace Pragmatic.Interaction
{
    public interface IResponseMapper
    {
        Response Map(Response originalResponse);
        Response<T> Map<T>(Response<T> originalResponse);
    }
}
