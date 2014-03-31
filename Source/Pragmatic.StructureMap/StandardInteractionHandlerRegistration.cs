using System;
using System.Linq;
using System.Reflection;
using Pragmatic.Interaction;
using Pragmatic.Interaction.StandardCommands;
using Pragmatic.Interaction.StandardQueries;
using Pragmatic.Interaction.StandardRequests;
using StructureMap.Configuration.DSL;
using SwissKnife;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.StructureMap
{
    public static class StandardInteractionHandlerRegistration
    {
        public static void RegisterStandardInteractionHandlersForEntities(Registry registry, StandardInteractionHandlerGenericTypeDefinitions standardInteractionHandlerGenericTypeDefinitions, params Assembly[] assembliesContainingEntities)
        {
            Argument.IsNotNull(registry, "registry");
            Argument.IsNotNull(standardInteractionHandlerGenericTypeDefinitions, "standardInteractionHandlerGenericTypeDefinitions");
            Argument.IsNotNull(assembliesContainingEntities, "assembliesContainingEntities");
            Argument.IsValid(assembliesContainingEntities.Length > 0,
                             "The array of assemblies that contain entities is empty. The array must have at least one assembly specified.",
                             "assembliesContainingEntities");

            var entityTypes = assembliesContainingEntities.SelectMany(assembly => assembly.GetTypes().Where(type => type != typeof(Entity) && typeof(Entity).IsAssignableFrom(type)));

            foreach (var entityType in entityTypes) // E.g. User.
            {
                // E.g. IQueryHandler<GetByIdQuery<User>, Option<User>>.
                ConnectInteractionHandlerToStandardInteractionForEntityType(registry, typeof(IQueryHandler<,>), typeof(GetByIdQuery<>), typeof(Option<>).MakeGenericType(entityType), standardInteractionHandlerGenericTypeDefinitions.GetByIdQueryHandler, entityType);
                // E.g. IQueryHandler<GetOneQuery<User>, Option<User>>.
                ConnectInteractionHandlerToStandardInteractionForEntityType(registry, typeof(IQueryHandler<,>), typeof(GetOneQuery<>), typeof(Option<>).MakeGenericType(entityType), standardInteractionHandlerGenericTypeDefinitions.GetOneQueryHandler, entityType);
                // E.g. IQueryHandler<GetAllQuery<User>, IPagedEnumerable<User>>.
                ConnectInteractionHandlerToStandardInteractionForEntityType(registry, typeof(IQueryHandler<,>), typeof(GetAllQuery<>), typeof(IPagedEnumerable<>).MakeGenericType(entityType), standardInteractionHandlerGenericTypeDefinitions.GetAllQueryHandler, entityType);
                // E.g. IQueryHandler<GetTotalCountQuery<User>, int>.
                ConnectInteractionHandlerToStandardInteractionForEntityType(registry, typeof(IQueryHandler<,>), typeof(GetTotalCountQuery<>), typeof(int), standardInteractionHandlerGenericTypeDefinitions.GetTotalCountQueryHandler, entityType);


                // E.g. IRequestHandler<CanDeleteEntityRequest<User>, Response<Option<User>>>.
                ConnectInteractionHandlerToStandardInteractionForEntityType(registry, typeof(IRequestHandler<,>), typeof(CanDeleteEntityRequest<>), typeof(Response<>).MakeGenericType(typeof(Option<>).MakeGenericType(entityType)), standardInteractionHandlerGenericTypeDefinitions.CanDeleteEntityRequestHandler, entityType);


                // E.g. ICommandHandler<DeleteEntityCommand<User>, Response>.
                ConnectInteractionHandlerToStandardInteractionForEntityType(registry, typeof(ICommandHandler<,>), typeof(DeleteEntityCommand<>), typeof(Response), standardInteractionHandlerGenericTypeDefinitions.DeleteEntityCommandHandler, entityType);
            }
        }

        // interactionHandlerOpenInterfaceType is e.g. IRequestHandler<,> or ICommandHandler<,> or IQueryHandler<,>.
        // resultType is concrete result type e.g. Response<Option<User>> or Response or IPagedEnumerable<User>.
        private static void ConnectInteractionHandlerToStandardInteractionForEntityType(Registry registry, Type interactionHandlerOpenInterfaceType,
                        Type standardInteractionGenericTypeDefinition, Type resultType,
                        Type interactionHandlerGenericTypeDefinition, Type entityType)
        {
            // E.g. CanDeleteEntityRequest<> -> CanDeleteEntityRequest<User> or
            //      DeleteEntityCommand<> -> DeleteEntityCommand<User> or
            //      GetAllQuery<> -> GetAllQuery<User>.
            Type requestType = standardInteractionGenericTypeDefinition.MakeGenericType(entityType);
            // E.g. IRequestHandler<CanDeleteEntityRequest<User>, Response<Option<User>>> or
            //      ICommandHandler<DeleteEntityCommand<User>, Response> or
            //      IQueryHandler<GetAllQuery<User>, IPagedEnumerable<User>>
            Type closedRequestHandlerInterfaceType = interactionHandlerOpenInterfaceType.MakeGenericType(requestType, resultType);
            // E.g. CanDeleteEntityRequestHandler<> -> CanDeleteEntityRequestHandler<User> or
            //      DeleteEntityCommandHandler<> -> DeleteEntityCommandHandler<User>
            //      GetAllQueryHandler<> -> GetAllQueryHandler<User> 
            Type closedRequestHandlerType = interactionHandlerGenericTypeDefinition.MakeGenericType(entityType);

            registry.For(closedRequestHandlerInterfaceType).Use(closedRequestHandlerType);
        }
    }
}
