using System;
using System.Collections.Generic;
using System.Linq;
using SwissKnife.Diagnostics.Contracts;

namespace TinyDdd.Interaction
{
    public abstract class CommandExecutor
    {
        public readonly string ExecuteCommandHandlerTechnicalErrorKey = typeof (CommandExecutor).FullName + "ExecuteCommandHandlerTechnicalErrorKey";

        public TResponse Execute<TCommand, TResponse>(TCommand command) where TCommand : ICommand where TResponse : Response, new()
        {
            Argument.IsNotNull(command, "command");

            var commandHandlers = GetCommandHandlers<TCommand, TResponse>().ToArray();

            if (commandHandlers.Length <= 0)
                throw new InvalidOperationException(string.Format("There is no command handler defined for the commands of type '{0}'.", typeof(TCommand)));

            if (commandHandlers.Length > 1)
                throw new NotSupportedException(string.Format("There are {0} command handlers defined for the commands of type '{1}'. " +
                                                              "Having more than one command handler per command type is not supported. " +
                                                              "The defined command handlers are:{2}{3}",
                                                              commandHandlers.Length,
                                                              typeof(TCommand),
                                                              Environment.NewLine,
                                                              commandHandlers.Aggregate(string.Empty, (result, commandHandler) => result + commandHandler.GetType().FullName + Environment.NewLine)));

            try
            {
                return commandHandlers[0].Execute(command);
            }
            catch (Exception e)
            {
                string additionalMessage = string.Format("An exception occured while executing the command handler of type '{0}'.", commandHandlers[0].GetType().FullName);
                LogException(additionalMessage, e);

                return CreateTechnicalErroResponse<TResponse>(additionalMessage);
            }
        }

        private TResponse CreateTechnicalErroResponse<TResponse>(string additionalMessage) where TResponse : Response, new()
        {
            TResponse technicalErroResponse = new TResponse();
            technicalErroResponse.AddTechnicalError(ExecuteCommandHandlerTechnicalErrorKey, additionalMessage);
            return technicalErroResponse;
        }

        protected abstract IEnumerable<ICommandHandler<TCommand, TResponse>> GetCommandHandlers<TCommand, TResponse>() where TCommand : ICommand where TResponse : Response;
        protected virtual void LogException(string additionalMessage, Exception exception) { }
    }
}
