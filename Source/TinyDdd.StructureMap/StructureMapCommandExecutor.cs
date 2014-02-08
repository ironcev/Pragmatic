using System;
using System.Collections.Generic;
using System.Linq;
using StructureMap;
using TinyDdd.Interaction;

namespace TinyDdd.StructureMap
{
    public class StructureMapCommandExecutor : CommandExecutor
    {
        protected override IEnumerable<ICommandHandler<TCommand, TResponse>> GetCommandHandlers<TCommand, TResponse>()
        {
            try
            {
                return ObjectFactory.GetAllInstances<ICommandHandler<TCommand, TResponse>>();
            }
            catch (Exception e)
            {
                string additionalMessage = string.Format("An exception occured while resolving command handlers for the commands of type '{0}'.", typeof(TCommand).FullName);
                LogException(additionalMessage, e);
                return Enumerable.Empty<ICommandHandler<TCommand, TResponse>>();
            }            
        }
    }
}
