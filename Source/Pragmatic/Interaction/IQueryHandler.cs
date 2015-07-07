namespace Pragmatic.Interaction
{
    public interface IQueryHandler<in TQuery, out TResult> where TQuery : class, IQuery
    {
        TResult Execute(TQuery query);
    }
}
