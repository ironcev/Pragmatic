namespace Pragmatic.Interaction
{
    public interface IEquatableQuery<in TQuery> : IQuery where TQuery : class, IQuery
    {
        bool IsEquivalentTo(TQuery query);
    }

    public interface IEquatableQuery<in TQuery, TResult> : IQuery<TResult> where TQuery : class, IQuery<TResult>
    {
        bool IsEquivalentTo(TQuery query);
    }
}
