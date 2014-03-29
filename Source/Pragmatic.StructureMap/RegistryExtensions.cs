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
    public class QueryHandlerGenericTypeDefinitions // TODO-IG: Get rid of this class and replace it by scanning the assemblies.
    {
        public Type GetByIdDefinition { get; private set; }
        public Type GetOneDefinition { get; private set; }
        public Type GetAllDefinition { get; private set; }
        public Type GetTotalCountDefinition { get; private set; }

        public QueryHandlerGenericTypeDefinitions(Type getByIdDefinition, Type getOneDefinition, Type getAllDefinition, Type getTotalCountDefinition)
        {
            Argument.IsNotNull(getByIdDefinition, "getByIdDefinition");
            Argument.IsNotNull(getOneDefinition, "getOneDefinition");
            Argument.IsNotNull(getAllDefinition, "getAllDefinition");
            Argument.IsNotNull(getTotalCountDefinition, "getTotalCountDefinition");
            // TODO-IG: Add other preconditions if appropriate.

            GetByIdDefinition = getByIdDefinition;
            GetOneDefinition = getOneDefinition;
            GetAllDefinition = getAllDefinition;
            GetTotalCountDefinition = getTotalCountDefinition;
        }
    }

    public class RequestHandlerGenericTypeDefinitions // TODO-IG: Get rid of this class and replace it by scanning the assemblies.
    {
        public Type CanDeleteEntityDefinition { get; private set; }

        public RequestHandlerGenericTypeDefinitions(Type canDeleteEntityDefinition)
        {
            Argument.IsNotNull(canDeleteEntityDefinition, "canDeleteEntityDefinition");
            // TODO-IG: Add other preconditions if appropriate.

            CanDeleteEntityDefinition = canDeleteEntityDefinition;
        }
    }

    public class CommandHandlerGenericTypeDefinitions // TODO-IG: Get rid of this class and replace it by scanning the assemblies.
    {
        public Type DeleteEntityDefinition { get; private set; }

        public CommandHandlerGenericTypeDefinitions(Type deleteEntityDefinition)
        {
            Argument.IsNotNull(deleteEntityDefinition, "deleteEntityDefinition");
            // TODO-IG: Add other preconditions if appropriate.

            DeleteEntityDefinition = deleteEntityDefinition;
        }
    }

    public static class RegistryExtensions // TODO-IG: Move this to Pragmatic once when we move from sub classing to dependency injection.
    {
        public static void ConnectQueryHandlerImplementationsToStandardQueriesForDerivedTypesOf(this Registry registry, QueryHandlerGenericTypeDefinitions queryHandlerGenericTypeDefinitions, Type baseType, params Assembly[] assembliesContainingDerivedTypes)
        {
            Argument.IsNotNull(registry, "registry");
            Argument.IsNotNull(queryHandlerGenericTypeDefinitions, "requestHandlerGenericTypeDefinitions");
            Argument.IsNotNull(baseType, "baseType");
            Argument.IsValid(!baseType.IsSealed, string.Format("Base type must not be sealed. The base type is: '{0}'.", baseType), "baseType");
            Argument.IsNotNull(assembliesContainingDerivedTypes, "assembliesContainingDerivedTypes");

            var typesDerivedFromBaseType = assembliesContainingDerivedTypes
                                           .SelectMany(assembly => assembly.GetTypes().Where(type => type != baseType && baseType.IsAssignableFrom(type)));

            foreach (var derivedType in typesDerivedFromBaseType)
            {
                ConnectQueryHandlerToStandardQueryForQueriedType(registry, typeof(GetByIdQuery<>), typeof(Option<>), queryHandlerGenericTypeDefinitions.GetByIdDefinition, derivedType);
                ConnectQueryHandlerToStandardQueryForQueriedType(registry, typeof(GetOneQuery<>), typeof(Option<>), queryHandlerGenericTypeDefinitions.GetOneDefinition, derivedType);
                ConnectQueryHandlerToStandardQueryForQueriedType(registry, typeof(GetAllQuery<>), typeof(IPagedEnumerable<>), queryHandlerGenericTypeDefinitions.GetAllDefinition, derivedType);
                ConnectQueryHandlerToStandardQueryForQueriedType(registry, typeof(GetTotalCountQuery<>), typeof(int), queryHandlerGenericTypeDefinitions.GetTotalCountDefinition, derivedType);
            }
        }

        // TODO-IG: Remove code duplication in the below methods. Combine all three methods into one to avoid triple scanning of assemblies.

        // TODO-IG: Remove the standardQueryResulType and deduce it from the standardQueryGenericTypeDefinition. Try to see if other arguments can be reduced as well.
        private static void ConnectQueryHandlerToStandardQueryForQueriedType(Registry registry,
            Type standardQueryGenericTypeDefinition, Type standardQueryResultTypeGenericTypeDefinition,
            Type queryHandlerGenericTypeDefinition, Type queriedType)
        {
            Type openQueryHandlerInterfaceType = typeof(IQueryHandler<,>); // IQueryHandler<>.
            Type queryType = standardQueryGenericTypeDefinition.MakeGenericType(queriedType); // e.g. GetByIdQuery<> -> GetByIdQuery<User>.
            Type resultType = standardQueryResultTypeGenericTypeDefinition.IsGenericTypeDefinition ? standardQueryResultTypeGenericTypeDefinition.MakeGenericType(queriedType) : standardQueryResultTypeGenericTypeDefinition; // e.g. Option<> -> Option<User>.

            Type closedQueryHandlerInterfaceType = openQueryHandlerInterfaceType.MakeGenericType(queryType, resultType); // IQueryHandler<GetByIdQuery<User>, Option<User>>.
            Type closedQueryHandlerType = queryHandlerGenericTypeDefinition.MakeGenericType(queriedType); // e.g. GetByIdQueryHandler<> -> GetByIdQueryHandler<User>.

            registry.For(closedQueryHandlerInterfaceType).Use(closedQueryHandlerType);
        }

        public static void ConnectRequestHandlerImplementationsToStandardRequestsForDerivedTypesOf(this Registry registry, RequestHandlerGenericTypeDefinitions requestHandlerGenericTypeDefinitions, Type baseType, params Assembly[] assembliesContainingDerivedTypes)
        {
            Argument.IsNotNull(registry, "registry");
            Argument.IsNotNull(requestHandlerGenericTypeDefinitions, "requestHandlerGenericTypeDefinitions");
            Argument.IsNotNull(baseType, "baseType");
            Argument.IsValid(!baseType.IsSealed, string.Format("Base type must not be sealed. The base type is: '{0}'.", baseType), "baseType");
            Argument.IsNotNull(assembliesContainingDerivedTypes, "assembliesContainingDerivedTypes");

            var typesDerivedFromBaseType = assembliesContainingDerivedTypes
                                           .SelectMany(assembly => assembly.GetTypes().Where(type => type != baseType && baseType.IsAssignableFrom(type)));

            foreach (var derivedType in typesDerivedFromBaseType)
            {
                ConnectRequestHandlerToStandardRequestForEntityType(registry, typeof(CanDeleteEntityRequest<>), typeof(Response<>).MakeGenericType(typeof(Option<>).MakeGenericType(derivedType)), requestHandlerGenericTypeDefinitions.CanDeleteEntityDefinition, derivedType);
            }
        }

        private static void ConnectRequestHandlerToStandardRequestForEntityType(Registry registry,
            Type standardRequestGenericTypeDefinition, Type standardRequestResultTypeGenericTypeDefinition,
            Type requestHandlerGenericTypeDefinition, Type entityType)
        {
            Type openRequestHandlerInterfaceType = typeof(IRequestHandler<,>); // IRequestHandler<>.
            Type requestType = standardRequestGenericTypeDefinition.MakeGenericType(entityType); // e.g. CanDeleteEntityRequest<> -> CanDeleteEntityRequest<User>.
            Type resultType = standardRequestResultTypeGenericTypeDefinition.IsGenericTypeDefinition ? standardRequestResultTypeGenericTypeDefinition.MakeGenericType(entityType) : standardRequestResultTypeGenericTypeDefinition; // e.g. Response<Option<>> -> Response<Option<User>>.

            Type closedRequestHandlerInterfaceType = openRequestHandlerInterfaceType.MakeGenericType(requestType, resultType); // IRequestHandler<CanDeleteEntityRequest<User>, Response<Option<User>>>.
            Type closedRequestHandlerType = requestHandlerGenericTypeDefinition.MakeGenericType(entityType); // e.g. CanDeleteEntityRequestHandler<> -> CanDeleteEntityRequestHandler<User>.

            registry.For(closedRequestHandlerInterfaceType).Use(closedRequestHandlerType);
        }

        public static void ConnectCommandHandlerImplementationsToStandardCommandsForDerivedTypesOf(this Registry registry, CommandHandlerGenericTypeDefinitions commandHandlerGenericTypeDefinitions, Type baseType, params Assembly[] assembliesContainingDerivedTypes)
        {
            Argument.IsNotNull(registry, "registry");
            Argument.IsNotNull(commandHandlerGenericTypeDefinitions, "commandHandlerGenericTypeDefinitions");
            Argument.IsNotNull(baseType, "baseType");
            Argument.IsValid(!baseType.IsSealed, string.Format("Base type must not be sealed. The base type is: '{0}'.", baseType), "baseType");
            Argument.IsNotNull(assembliesContainingDerivedTypes, "assembliesContainingDerivedTypes");

            var typesDerivedFromBaseType = assembliesContainingDerivedTypes
                                           .SelectMany(assembly => assembly.GetTypes().Where(type => type != baseType && baseType.IsAssignableFrom(type)));

            foreach (var derivedType in typesDerivedFromBaseType)
            {
                ConnectCommandHandlerToStandardRequestForEntityType(registry, typeof(DeleteEntityCommand<>), typeof(Response), commandHandlerGenericTypeDefinitions.DeleteEntityDefinition, derivedType);
            }
        }

        private static void ConnectCommandHandlerToStandardRequestForEntityType(Registry registry,
            Type standardCommandGenericTypeDefinition, Type standardCommandResultTypeGenericTypeDefinition,
            Type commandHandlerGenericTypeDefinition, Type entityType)
        {
            Type openCommandHandlerInterfaceType = typeof(ICommandHandler<,>); // ICommandHandler<>.
            Type commandType = standardCommandGenericTypeDefinition.MakeGenericType(entityType); // e.g. DeleteEntityCommand<> -> DeleteEntityCommand<User>.
            Type resultType = standardCommandResultTypeGenericTypeDefinition.IsGenericTypeDefinition ? standardCommandResultTypeGenericTypeDefinition.MakeGenericType(entityType) : standardCommandResultTypeGenericTypeDefinition; // e.g. Response<Option<>> -> Response<Option<User>> or Response -> Response.

            Type closedCommandHandlerInterfaceType = openCommandHandlerInterfaceType.MakeGenericType(commandType, resultType); // ICommandHandler<DeleteEntityCommand<User>, Response>.
            Type closedCommandHandlerType = commandHandlerGenericTypeDefinition.MakeGenericType(entityType); // e.g. DeleteEntityCommandHandler<> -> DeleteEntityCommandHandler<User>.

            registry.For(closedCommandHandlerInterfaceType).Use(closedCommandHandlerType);
        }
    }
}
