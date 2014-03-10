using System;
using System.Collections.Generic;
using System.Linq;
using Pragmatic.Interaction;
using StructureMap;

namespace Pragmatic.StructureMap
{
    public class StructureMapCommandExecutor : CommandExecutor
    {
        protected override IEnumerable<object> GetCommandHandlers<TResponse>(Type commandType)
        {
            System.Diagnostics.Debug.Assert(commandType != null);
            System.Diagnostics.Debug.Assert(typeof(ICommand<TResponse>).IsAssignableFrom(commandType));

            try
            {
                return ObjectFactory.GetAllInstances(typeof(ICommandHandler<,>).MakeGenericType(commandType, typeof(TResponse))).Cast<object>();
            }
            catch (Exception e)
            {
                string additionalMessage = string.Format("An exception occured while resolving command handlers for the commands of type '{0}'.", commandType);
                LogException(additionalMessage, e);

                return Enumerable.Empty<object>();
            }            
        }
    }
}
