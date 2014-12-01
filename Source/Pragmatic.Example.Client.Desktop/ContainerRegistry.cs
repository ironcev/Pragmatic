using System;
using System.Data.Entity;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentValidation;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using Pragmatic.Example.Client.Desktop.Persistency.EntityFramework;
using Pragmatic.Example.Client.Desktop.Persistency.NHibernate;
using Pragmatic.Example.Model;
using Pragmatic.Interaction;
using Pragmatic.Interaction.EntityDeletion;
using Pragmatic.Interaction.StandardQueries;
using Pragmatic.StructureMap;
using Raven.Client;
using Raven.Client.Document;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;
using SwissKnife;
using RavenStandardQueries = Pragmatic.Raven.Interaction.StandardQueries;
using NHibernateStandardQueries = Pragmatic.NHibernate.Interaction.StandardQueries;
using EntityFrameworkStandardQueries = Pragmatic.EntityFramework.Interaction.StandardQueries;


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
                For<IQueryHandler<GetByIdQuery, Option<object>>>().Singleton().Use<RavenStandardQueries.GetByIdQueryHandler>();
                standardInteractionHandlerGenericTypeDefinitions = new StandardInteractionHandlerGenericTypeDefinitions
                    (
                    typeof(RavenStandardQueries.GetByIdQueryHandler<>),
                    typeof(RavenStandardQueries.GetOneQueryHandler<>),
                    typeof(RavenStandardQueries.GetAllQueryHandler<>),
                    typeof(RavenStandardQueries.GetTotalCountQueryHandler<>)
                    );

                IncludeRegistry<RavenContainerRegistry>();
            }
            else if (UnitOfWorkFactory.DefaultUnitOfWorkType == typeof(NHibernate.UnitOfWork))
            {
                For<IQueryHandler<GetByIdQuery, Option<object>>>().Singleton().Use<NHibernateStandardQueries.GetByIdQueryHandler>();
                standardInteractionHandlerGenericTypeDefinitions = new StandardInteractionHandlerGenericTypeDefinitions
                (
                    typeof(NHibernateStandardQueries.GetByIdQueryHandler<>),
                    typeof(NHibernateStandardQueries.GetOneQueryHandler<>),
                    typeof(NHibernateStandardQueries.GetAllQueryHandler<>),
                    typeof(NHibernateStandardQueries.GetTotalCountQueryHandler<>)
                );

                IncludeRegistry<NHibernateContainerRegistry>();
            }
            else if (UnitOfWorkFactory.DefaultUnitOfWorkType == typeof (EntityFramework.UnitOfWork))
            {
                For<IQueryHandler<GetByIdQuery, Option<object>>>().Singleton().Use<EntityFrameworkStandardQueries.GetByIdQueryHandler>();
                standardInteractionHandlerGenericTypeDefinitions = new StandardInteractionHandlerGenericTypeDefinitions
                (
                    typeof(EntityFrameworkStandardQueries.GetByIdQueryHandler<>),
                    typeof(EntityFrameworkStandardQueries.GetOneQueryHandler<>),
                    typeof(EntityFrameworkStandardQueries.GetAllQueryHandler<>),
                    typeof(EntityFrameworkStandardQueries.GetTotalCountQueryHandler<>)
                );

                IncludeRegistry<EntityFrameworkContainerRegistry>();
            }
            else throw new InvalidOperationException(string.Format("The type '{0}' is not supported as the default unit of work.", UnitOfWorkFactory.DefaultUnitOfWorkType));


            StandardInteractionHandlerRegistration.RegisterStandardInteractionHandlersForEntities(this, standardInteractionHandlerGenericTypeDefinitions);

            For<UnitOfWork>()
                .LifecycleIs(new InteractionScopeLifecycle())
                .Use(() => UnitOfWorkFactory.CreateUnitOfWork());
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

            IDocumentStore documentStore = new DocumentStore { ConnectionStringName = "RavenConnectionString" }.Initialize();
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

    public class EntityFrameworkContainerRegistry : Registry
    {
        public EntityFrameworkContainerRegistry()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ExampleDbContext>());

            For<DbContext>()
                .LifecycleIs(new InteractionScopeLifecycle())
                .Use<ExampleDbContext>();
        }
    }
}