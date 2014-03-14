using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentValidation;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using Pragmatic.Example.Client.Desktop.NHibernateMappings;
using Pragmatic.Example.Model;
using Pragmatic.Interaction;
using Pragmatic.Raven.Interaction.StandardQueries;
using Pragmatic.StructureMap;
using Raven.Client;
using Raven.Client.Document;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace Pragmatic.Example.Client.Desktop
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
            For<IInteractionHandlerResolver>().Use(new StructureMapInteractionHandlerResolver());

            // Register query handlers for standard queries.
            QueryHandlerGenericTypeDefinitions definitions;
            if (UnitOfWorkFactory.DefaultUnitOfWorkType == typeof(Raven.UnitOfWork))
            {
                definitions = new QueryHandlerGenericTypeDefinitions
                    (
                    typeof(GetByIdQueryHandler<>),
                    typeof(GetOneQueryHandler<>),
                    typeof(GetAllQueryHandler<>),
                    typeof(GetTotalCountQueryHandler<>)
                    );
            }
            else
            {
                definitions = new QueryHandlerGenericTypeDefinitions
                (
                    typeof(NHibernate.Interaction.StandardQueries.GetByIdQueryHandler<>),
                    typeof(NHibernate.Interaction.StandardQueries.GetOneQueryHandler<>),
                    typeof(NHibernate.Interaction.StandardQueries.GetAllQueryHandler<>),
                    typeof(NHibernate.Interaction.StandardQueries.GetTotalCountQueryHandler<>)
                );
            }

            this.ConnectQueryHandlerImplementationsToStandardQueriesForDerivedTypesOf(definitions, typeof(Entity), typeof(User).Assembly);

            For<UnitOfWork>()
                .LifecycleIs(new InteractionScopeLifecycle())
                .Use(UnitOfWorkFactory.CreateUnitOfWork);

            IncludeRegistry<RavenContainerRegistry>();
            IncludeRegistry<NHibernateContainerRegistry>();
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

    public class NHibernateContainerRegistry : Registry
    {
        public NHibernateContainerRegistry()
        {
            For<ISession>()
                .LifecycleIs(new InteractionScopeLifecycle())
                .Use(() => ObjectFactory.GetInstance<ISessionFactory>().OpenSession());

            For<ISessionFactory>()
                .Singleton()
                .Use(() =>
                {
                    var configuration = Fluently.Configure()
                        .Database(SQLiteConfiguration.Standard.UsingFile("Pragmatic.db"))
                        .Mappings(mappings =>
                            mappings.FluentMappings.AddFromAssembly(System.Reflection.Assembly.GetAssembly(typeof(UserMap)))
                        );

                    SchemaExport schema = new SchemaExport(configuration.BuildConfiguration());
                    schema.Execute(false, true, false);

                    return configuration.BuildSessionFactory();
                });
        }

    }
}