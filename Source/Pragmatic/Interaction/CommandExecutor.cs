using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.Interaction
{
    public abstract class CommandExecutor
    {
        public TResponse Execute<TResponse>(ICommand<TResponse> command) where TResponse : Response
        {
            Argument.IsNotNull(command, "command");

            InteractionScope.BeginOrJoin();

            var commandHandlers = GetCommandHandlers<TResponse>(command.GetType()).ToArray();

            if (commandHandlers.Length <= 0)
                throw new InvalidOperationException(string.Format("There is no command handler defined for the commands of type '{0}'.", command.GetType()));

            if (commandHandlers.Length > 1)
                throw new NotSupportedException(string.Format("There are {1} command handlers defined for the commands of type '{2}'.{0}" + // TODO-IG: Introduce ExceptionBuilder class to avoid code polution.
                                                              "Having more than one command handler per command type is not supported.{0}" +
                                                              "The defined command handlers are:{0}{3}",
                                                              Environment.NewLine,
                                                              commandHandlers.Length,
                                                              command.GetType(),                                                              
                                                              commandHandlers.Aggregate(string.Empty, (output, commandHandler) => output + commandHandler.GetType() + Environment.NewLine)));

            try
            {
                return ExecuteCommandHandler(commandHandlers[0], command);
            }
            catch (Exception e)
            {
                string additionalMessage = string.Format("An exception occured while executing the command handler of type '{0}'.", commandHandlers[0].GetType());
                LogException(additionalMessage, e);

                throw new CommandExecutionException(additionalMessage, e);
            }
            finally
            {
                InteractionScope.End();
            }
        }

        private static TResponse ExecuteCommandHandler<TResponse>(object commandHandler, ICommand<TResponse> command) where TResponse : Response
        {
            var executeMethod = commandHandler.GetType().GetMethod("Execute", // TODO-IG: Replace with labda expressions once when SwissKnife supports that.
                                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod,
                                    null, CallingConventions.HasThis,
                                    new[] { command.GetType() },
                                    null);
            return (TResponse)executeMethod.Invoke(commandHandler, new object[] {command});
        }

        protected abstract IEnumerable<object> GetCommandHandlers<TResponse>(Type commandType) where TResponse : Response;
        protected virtual void LogException(string additionalMessage, Exception exception) { }
    }
}
