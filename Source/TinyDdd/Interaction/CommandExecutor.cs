using System;
using System.Collections.Generic;
using System.Linq;
using SwissKnife.Diagnostics.Contracts;

namespace TinyDdd.Interaction
{
    public abstract class CommandExecutor
    {
        public readonly string ExecuteCommandHandlerTechnicalErrorKey = typeof (CommandExecutor).FullName + "ExecuteCommandHandlerTechnicalErrorKey";

        public TResponse Execute<TResponse>(ICommand<TResponse> command) where TResponse : Response
        {
            Argument.IsNotNull(command, "command");

            var commandHandlers = GetCommandHandlers(command.GetType()).ToArray();

            if (commandHandlers.Length <= 0)
                throw new InvalidOperationException(string.Format("There is no command handler defined for the commands of type '{0}'.", command.GetType()));

            if (commandHandlers.Length > 1)
                throw new NotSupportedException(string.Format("There are {1} command handlers defined for the commands of type '{2}'.{0}" +
                                                              "Having more than one command handler per command type is not supported.{0}" +
                                                              "The defined command handlers are:{0}{3}",
                                                              Environment.NewLine,
                                                              commandHandlers.Length,
                                                              command.GetType(),                                                              
                                                              commandHandlers.Aggregate(string.Empty, (result, commandHandler) => result + commandHandler.GetType() + Environment.NewLine)));

            Response response;
            try
            {
                response = commandHandlers[0].Execute(command);
            }
            catch (Exception e)
            {
                string additionalMessage = string.Format("An exception occured while executing the command handler of type '{0}'.", commandHandlers[0].GetType());
                LogException(additionalMessage, e);

                throw new CommandExecutionException(additionalMessage, e);
            }

            try
            {
                return (TResponse) response;
            }
            catch (InvalidCastException e)
            {
                string additionalMessage = string.Format("An exception occured while converting the response of the command handler of type '{1}'.{0}" +
                                                         "The returned response type cannot be converted into the expected response type.{0}" +
                                                         "The expected response type is '{2}'.{0}" +
                                                         "The returned response type is '{3}'.",
                                                         Environment.NewLine,
                                                         commandHandlers[0].GetType(),
                                                         typeof(TResponse),
                                                         response.GetType());
                LogException(additionalMessage, e);

                throw new CommandExecutionException(additionalMessage, e);
            }
        }

        protected abstract IEnumerable<ICommandHandler> GetCommandHandlers(Type commandType);
        protected virtual void LogException(string additionalMessage, Exception exception) { }
    }
}
