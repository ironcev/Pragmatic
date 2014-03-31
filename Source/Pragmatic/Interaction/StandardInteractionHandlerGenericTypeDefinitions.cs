using System;
using Pragmatic.Interaction.StandardCommands;
using Pragmatic.Interaction.StandardRequests;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.Interaction
{
    public class StandardInteractionHandlerGenericTypeDefinitions
    {
        public Type GetByIdQueryHandler { get; private set; }
        public Type GetOneQueryHandler { get; private set; }
        public Type GetAllQueryHandler { get; private set; }
        public Type GetTotalCountQueryHandler { get; private set; }

        public Type CanDeleteEntityRequestHandler { get; private set; }

        public Type DeleteEntityCommandHandler { get; private set; }

        public StandardInteractionHandlerGenericTypeDefinitions(Type getByIdQueryHandler, Type getOneQueryHandler, Type getAllQueryHandler, Type getTotalCountQueryHandler)
            : this(getByIdQueryHandler, getOneQueryHandler, getAllQueryHandler, getTotalCountQueryHandler, typeof(CanDeleteEntityRequestHandler<>), typeof(DeleteEntityCommandHandler<>))
        { }

        public StandardInteractionHandlerGenericTypeDefinitions(Type getByIdQueryHandler, Type getOneQueryHandler, Type getAllQueryHandler, Type getTotalCountQueryHandler,
                                                                Type canDeleteEntityRequestHandler, Type deleteEntityCommandHandler)
        {
            Argument.IsNotNull(getByIdQueryHandler, "getByIdQueryHandler");
            Argument.IsNotNull(getOneQueryHandler, "getOneQueryHandler");
            Argument.IsNotNull(getAllQueryHandler, "getAllQueryHandler");
            Argument.IsNotNull(getTotalCountQueryHandler, "getTotalCountQueryHandler");
            Argument.IsNotNull(canDeleteEntityRequestHandler, "canDeleteEntityRequestHandler");
            Argument.IsNotNull(deleteEntityCommandHandler, "deleteEntityCommandHandler");
            // TODO-IG: Add preconditions that check if the provided types are of expected base generic types.

            GetByIdQueryHandler = getByIdQueryHandler;
            GetOneQueryHandler = getOneQueryHandler;
            GetAllQueryHandler = getAllQueryHandler;
            GetTotalCountQueryHandler = getTotalCountQueryHandler;

            CanDeleteEntityRequestHandler = canDeleteEntityRequestHandler;

            DeleteEntityCommandHandler = deleteEntityCommandHandler;
        }

        // TODO-IG: Add a helper method that will create the object by scanning assemblies and looking for the type definitions.
    }
}
