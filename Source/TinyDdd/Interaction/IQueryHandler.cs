namespace TinyDdd.Interaction
{
    public interface IQueryHandler<in TQuery, out TResult> where TQuery : IQuery
    {
        TResult Execute(TQuery query);
    }
}
