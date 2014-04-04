using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentValidation;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using Pragmatic.Example.Client.Desktop.NHibernateMappings;
using Pragmatic.Example.Model;
using Pragmatic.Interaction;
using Pragmatic.Interaction.EntityDeletion;
using Pragmatic.Raven.Interaction.StandardQueries;
using Pragmatic.StructureMap;
using Raven.Client;
using Raven.Client.Document;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

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

                scan.AssemblyContainingType<Entity>();
                scan.AssemblyContainingType<User>();

                scan.ConnectImplementationsToTypesClosing(typeof(ICommandHandler<,>));
                scan.ConnectImplementationsToTypesClosing(typeof(IQueryHandler<,>));
                scan.ConnectImplementationsToTypesClosing(typeof(IRequestHandler<,>));
                scan.ConnectImplementationsToTypesClosing(typeof(IValidator<>));
                scan.ConnectImplementationsToTypesClosing(typeof(EntityDeleter<>));
            });

            For<IResponseMapper>().Use(new InvariantResponseMapper());

            var structureMapInteractionObjectResolver = new StructureMapInteractionObjectResolver();
            For<IInteractionHandlerResolver>().Use(structureMapInteractionObjectResolver);
            For<IEntityDeleterResolver>().Use(structureMapInteractionObjectResolver);

            // Register query handlers for standard queries. WARNING: Intentionally Bad Code!
            StandardInteractionHandlerGenericTypeDefinitions standardInteractionHandlerGenericTypeDefinitions;
            if (UnitOfWorkFactory.DefaultUnitOfWorkType == typeof(Raven.UnitOfWork))
            {
                standardInteractionHandlerGenericTypeDefinitions = new StandardInteractionHandlerGenericTypeDefinitions
                    (
                    typeof(GetByIdQueryHandler<>),
                    typeof(GetOneQueryHandler<>),
                    typeof(GetAllQueryHandler<>),
                    typeof(GetTotalCountQueryHandler<>)
                    );
            }
            else
            {
                standardInteractionHandlerGenericTypeDefinitions = new StandardInteractionHandlerGenericTypeDefinitions
                (
                    typeof(NHibernate.Interaction.StandardQueries.GetByIdQueryHandler<>),
                    typeof(NHibernate.Interaction.StandardQueries.GetOneQueryHandler<>),
                    typeof(NHibernate.Interaction.StandardQueries.GetAllQueryHandler<>),
                    typeof(NHibernate.Interaction.StandardQueries.GetTotalCountQueryHandler<>)
                );
            }

            StandardInteractionHandlerRegistration.RegisterStandardInteractionHandlersForEntities(this, standardInteractionHandlerGenericTypeDefinitions, typeof(User).Assembly);

            For<UnitOfWork>()
                .LifecycleIs(new InteractionScopeLifecycle())
                .Use(() => UnitOfWorkFactory.CreateUnitOfWork());

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

            IDocumentStore documentStore = new DocumentStore {ConnectionStringName = "RavenConnStr"}.Initialize();
            For<IDocumentStore>()
                .Singleton()
                .Use(documentStore);
        }
    }

    public class NHibernateContainerRegistry : Registry
    {
        public NHibernateContainerRegistry()
        {
            For<ISession>()
                .LifecycleIs(new InteractionScopeLifecycle())
                .Use(() => ObjectFactory.GetInstance<ISessionFactory>().OpenSession());

            var configuration = Fluently.Configure()
                                        .Database(SQLiteConfiguration.Standard.UsingFile("Pragmatic.db"))
                                        .Mappings(mappings => mappings.FluentMappings.AddFromAssembly(System.Reflection.Assembly.GetAssembly(typeof(UserMap))));

            SchemaExport schema = new SchemaExport(configuration.BuildConfiguration());
            schema.Execute(false, true, false);

            ISessionFactory sessionFactory = configuration.BuildSessionFactory();

            For<ISessionFactory>()
                .Singleton()
                .Use(sessionFactory);
        }
    }
}