using FluentValidation;
using Raven.Client;
using Raven.Client.Document;
using StructureMap;
using StructureMap.Configuration.DSL;
using TinyDdd.Example.Model;
using TinyDdd.Interaction;
using TinyDdd.Raven.Interaction.StandardQueries;
using TinyDdd.StructureMap;

namespace TinyDdd.Example.Client.Desktop
{
    internal class ContainerRegistry : Registry
    {
        private ContainerRegistry()
        {
            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();

                scan.AssemblyContainingType<UnitOfWork>();
                scan.AssemblyContainingType<User>();

                scan.ConnectImplementationsToTypesClosing(typeof(ICommandHandler<,>));
                scan.ConnectImplementationsToTypesClosing(typeof(IQueryHandler<,>));
                scan.ConnectImplementationsToTypesClosing(typeof(IValidator<>));
            });

            For<IResponseMapper>().Use(new InvariantResponseMapper());
            For<CommandExecutor>().Use<StructureMapCommandExecutor>();
            For<QueryExecutor>().Use<StructureMapQueryExecutor>();

            // Register query handlers for standard queries.
            QueryHandlerGenericTypeDefinitions definitions = new QueryHandlerGenericTypeDefinitions
                (
                    typeof(GetByIdQueryHandler<>),
                    typeof(GetOneQueryHandler<>),
                    typeof(GetAllQueryHandler<>)
                );
            this.ConnectQueryHandlerImplementationsToStandardQueriesForDerivedTypesOf(definitions, typeof(Entity), typeof(User).Assembly);

            IncludeRegistry<RavenContainerRegistry>();
        }

        public static void Initialize()
        {
            ObjectFactory.Initialize(x => x.AddRegistry(new ContainerRegistry()));
        }
    }

    public class RavenContainerRegistry : Registry
    {
        public RavenContainerRegistry()
        {
            For<UnitOfWork>()
                .LifecycleIs(new InteractionScopeLifecycle())
                .Use<Raven.UnitOfWork>();

            For<IDocumentSession>()
                .LifecycleIs(new InteractionScopeLifecycle())
                .Use(() => ObjectFactory.GetInstance<IDocumentStore>().OpenSession());


            For<IAsyncDocumentSession>()
                .LifecycleIs(new InteractionScopeLifecycle())
                .Use(() => ObjectFactory.GetInstance<IDocumentStore>().OpenAsyncSession());

            For<IDocumentStore>()
                .Singleton()
                .Use(() =>
                {
                    var documentStore = new DocumentStore { ConnectionStringName = "RavenConnStr" };

                    return documentStore.Initialize();
                });
        }
    }
}