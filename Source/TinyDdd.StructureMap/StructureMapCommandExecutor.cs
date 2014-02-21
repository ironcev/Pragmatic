using System;
using System.Collections.Generic;
using System.Linq;
using StructureMap;
using TinyDdd.Interaction;

namespace TinyDdd.StructureMap
{
    public class StructureMapCommandExecutor : CommandExecutor
    {
        protected override IEnumerable<ICommandHandler> GetCommandHandlers(Type commandType)
        {
            System.Diagnostics.Debug.Assert(commandType != null);

            try
            {
                return ObjectFactory.GetAllInstances(typeof(ICommandHandler<>).MakeGenericType(commandType)).Cast<ICommandHandler>();
            }
            catch (Exception e)
            {
                string additionalMessage = string.Format("An exception occured while resolving command handlers for the commands of type '{0}'.", commandType);
                LogException(additionalMessage, e);

                return Enumerable.Empty<ICommandHandler>();
            }            
        }
    }
}
