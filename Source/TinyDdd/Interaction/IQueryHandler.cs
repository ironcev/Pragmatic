namespace TinyDdd.Interaction
{
    // ReSharper disable UnusedTypeParameter
    public interface IQueryHandler<in TQuery, out TResult> : IQueryHandler where TQuery : IQuery
    // ReSharper restore UnusedTypeParameter
    {
    }

    public interface IQueryHandler
    {
        object Execute(IQuery query);
    }
}
