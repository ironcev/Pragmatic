namespace Pragmatic.Interaction
{
    public class InvariantResponseMapper : IResponseMapper
    {
        public Response Map(Response originalResponse)
        {
            // There is no need to check the precondition. They will be immediately checked in the constructor.
            return new Response(originalResponse);
        }
    }
}
