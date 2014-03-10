/*
 * NOTE: See the architectural decisions at the end of this file.
 */
namespace Pragmatic.Interaction
{
    // ReSharper disable UnusedTypeParameter
    public interface IQuery<TResult> : IQuery
    // ReSharper restore UnusedTypeParameter
    {
    }

    public interface IQuery
    {
    }
}
/*
 * DECISIONS:
 * See the decisions at the end of the "ICommand.cs" file.
 *
 * The parameterized version of the IQuery interface is here just as convenience to support query execution in
 * which the result type is inferred. E.g:
 *   executor.Execute(someQuery);
 * Use this interface only if you are really sure that the query definition will not have more than one result type.
 */
