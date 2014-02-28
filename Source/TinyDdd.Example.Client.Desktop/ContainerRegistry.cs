using System.Collections.Generic;
using FluentValidation;
using Raven.Client;
using Raven.Client.Document;
using StructureMap;
using StructureMap.Configuration.DSL;
using SwissKnife;
using TinyDdd.Example.Model;
using TinyDdd.Interaction;
using TinyDdd.Interaction.StandardQueries;
using TinyDdd.Raven;
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

            // TODO-IG: How to tell to StructureMap to do the mapping below automatically?
            For<IQueryHandler<GetByIdQuery<User>, Option<User>>>().Use<GetByIdQueryHandler<User>>();
            For<IQueryHandler<GetOneQuery<User>, Option<User>>>().Use<GetOneQueryHandler<User>>();
            For<IQueryHandler<GetAllQuery<User>, IEnumerable<User>>>().Use<GetAllQueryHandler<User>>();

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
            For<IUnitOfWork>().Use<UnitOfWork>();

            For<IDocumentSession>()
                .HybridHttpOrThreadLocalScoped()
                .Use(() => ObjectFactory.GetInstance<IDocumentStore>().OpenSession());

            For<IAsyncDocumentSession>()
                .HybridHttpOrThreadLocalScoped()
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