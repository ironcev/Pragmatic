using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using StructureMap.Configuration.DSL;
using SwissKnife;
using SwissKnife.Diagnostics.Contracts;
using TinyDdd.Interaction;
using TinyDdd.Interaction.StandardQueries;

namespace TinyDdd.StructureMap
{
    public class QueryHandlerGenericTypeDefinitions // TODO-IG: Get rid of this class and replace it by scanning the assemblies.
    {
        public Type GetByIdDefinition { get; private set; }
        public Type GetOneDefinition { get; private set; }
        public Type GetAllDefinition { get; private set; }

        public QueryHandlerGenericTypeDefinitions(Type getByIdDefinition, Type getOneDefinition, Type getAllDefinition)
        {
            Argument.IsNotNull(getByIdDefinition, "GetByIdDefinition");
            Argument.IsNotNull(getOneDefinition, "GetOneDefinition");
            Argument.IsNotNull(getAllDefinition, "GetAllDefinition");
            // TODO-IG: Add other preconditions if appropriaete

            GetByIdDefinition = getByIdDefinition;
            GetOneDefinition = getOneDefinition;
            GetAllDefinition = getAllDefinition;
        }
    }

    public static class RegistryExtensions // TODO-IG: Move this to TinyDdd once when we move from subclassing to dependency injection.
    {
        public static void ConnectQueryHandlerImplementationsToStandardQueriesForDerivedTypesOf(this Registry registry, QueryHandlerGenericTypeDefinitions queryHandlerGenericTypeDefinitions, Type baseType, params Assembly[] assembliesContainingDerivedTypes)
        {
            Argument.IsNotNull(registry, "registry");
            Argument.IsNotNull(queryHandlerGenericTypeDefinitions, "queryHandlerGenericTypeDefinitions");
            Argument.IsNotNull(baseType, "baseType");
            Argument.IsValid(!baseType.IsSealed, string.Format("Base type must not be sealed. The base type is: '{0}'.", baseType), "baseType");
            Argument.IsNotNull(assembliesContainingDerivedTypes, "assembliesContainingDerivedTypes");

            var typesDerivedFromBaseType = assembliesContainingDerivedTypes
                                           .SelectMany(assembly => assembly.GetTypes().Where(type => type != baseType && baseType.IsAssignableFrom(type)));

            foreach (var derivedType in typesDerivedFromBaseType)
            {
                ConnectQueryHandlerToStandardQueryForQueriedType(registry, typeof(GetByIdQuery<>), typeof(Option<>), queryHandlerGenericTypeDefinitions.GetByIdDefinition, derivedType);
                ConnectQueryHandlerToStandardQueryForQueriedType(registry, typeof(GetOneQuery<>), typeof(Option<>), queryHandlerGenericTypeDefinitions.GetOneDefinition, derivedType);
                ConnectQueryHandlerToStandardQueryForQueriedType(registry, typeof(GetAllQuery<>), typeof(IEnumerable<>), queryHandlerGenericTypeDefinitions.GetAllDefinition, derivedType);
            }                
        }

        // TODO-IG: Remove the standardQueryResulType and deduce it from the standardQueryGenericTypeDefinition. Try to see if other arguments can be reduced as well.
        private static void ConnectQueryHandlerToStandardQueryForQueriedType(Registry registry,
            Type standardQueryGenericTypeDefinition, Type standardQueryResultTypeGenericTypeDefinition,
            Type queryHandlerGenericTypeDefinition, Type queriedType)
        {
            Type openQueryHandlerInterfaceType = typeof(IQueryHandler<,>); // IQueryHandler<>.
            Type queryType = standardQueryGenericTypeDefinition.MakeGenericType(queriedType); // e.g. GetByIdQuery<> -> GetByIdQuery<User>.
            Type resultType = standardQueryResultTypeGenericTypeDefinition.MakeGenericType(queriedType); // e.g. Option<> -> Option<User>.

            Type closedQueryHandlerInterfaceType = openQueryHandlerInterfaceType.MakeGenericType(queryType, resultType); // IQueryHandler<GetByIdQuery<User>, Option<User>>.
            Type closedQueryHandlerType = queryHandlerGenericTypeDefinition.MakeGenericType(queriedType); // e.g. GetByIdQueryHandler<> -> GetByIdQueryHandler<User>.

            registry.For(closedQueryHandlerInterfaceType).Use(closedQueryHandlerType);
        }
    }
}
