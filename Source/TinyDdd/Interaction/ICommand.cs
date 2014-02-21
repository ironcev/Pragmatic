/*
 * NOTE: See the architectural decisions at the end of this file.
 */
namespace TinyDdd.Interaction
{    
    public abstract class Command : ICommand<Response> { }

    // ReSharper disable UnusedTypeParameter
    public interface ICommand<in TResponse> : ICommand where TResponse : Response, new()
    // ReSharper restore UnusedTypeParameter
    {
    }

    public interface ICommand { }
}

/*
 * DECISIONS:
 * In theory, commands should be fire-and-forget and should not return any kind of response.
 * In the pratice we alway want some response to be returned, e.g. the new entity that was just created or similar.
 * Even in that case, the command shouldn't bother itself about what kind of response will be returned.
 * That should be the responsebility of the handler. In particular, different handlers could return different results for the same command.
 * Again, in practice, this is not the case. There is always a single type of response for the command.
 * In most of the cases it will be the plain Response object.
 * 
 * In addition, we want to be able to call the executor in the simplest manner possible, by writing:
 *     executor.Execute(someCommand);
 * Still we would like to have the proper response type at the output of the Execute() method.
 * This could be, of course, done in the following manner:
 *     ConcreteResponse response = executor.Execute<ConcreteResponse>(someCommand);
 * In order to simplify the calling code as much as possible we, introduced the parameterized version of ICommand so that the response type can be inferred by the compiler:
 *     ConcreteResponse response = executor.Execute(someCommand);
 * 
 * Abstract Command class is intended as base class for commands that simply return the plain Response object.
 */
