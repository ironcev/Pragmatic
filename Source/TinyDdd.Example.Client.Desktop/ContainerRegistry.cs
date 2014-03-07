using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentValidation;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using Raven.Client;
using Raven.Client.Document;
using StructureMap;
using StructureMap.Configuration.DSL;
using TinyDdd.Example.Model;
using TinyDdd.Interaction;
using TinyDdd.StructureMap;

namespace TinyDdd.Example.Client.Desktop
{
    internal class ContainerRegistry : Registry
    {
        private ContainerRegistry()
        {
            Scan( scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();

                scan.AssemblyContainingType<UnitOfWork>();
                scan.AssemblyContainingType<User>();

                scan.ConnectImplementationsToTypesClosing( typeof( ICommandHandler<,> ) );
                scan.ConnectImplementationsToTypesClosing( typeof( IQueryHandler<,> ) );
                scan.ConnectImplementationsToTypesClosing( typeof( IValidator<> ) );
            } );

            For<IResponseMapper>().Use( new InvariantResponseMapper() );
            For<CommandExecutor>().Use<StructureMapCommandExecutor>();
            For<QueryExecutor>().Use<StructureMapQueryExecutor>();

            // Register query handlers for standard queries.
            QueryHandlerGenericTypeDefinitions definitions;
            if (UnitOfWorkFactory.DefaultUnitOfWorkType == typeof (Raven.UnitOfWork))
            {
                definitions = new QueryHandlerGenericTypeDefinitions
                    (
                    typeof (Raven.Interaction.StandardQueries.GetByIdQueryHandler<>),
                    typeof (Raven.Interaction.StandardQueries.GetOneQueryHandler<>),
                    typeof (Raven.Interaction.StandardQueries.GetAllQueryHandler<>)
                    );
            }
            else
            {
                definitions = new QueryHandlerGenericTypeDefinitions
                (
                    typeof( NHibernate.Interaction.StandardQueries.GetByIdQueryHandler<> ),
                    typeof( NHibernate.Interaction.StandardQueries.GetOneQueryHandler<> ),
                    typeof( NHibernate.Interaction.StandardQueries.GetAllQueryHandler<> )
                );
            }
            
            this.ConnectQueryHandlerImplementationsToStandardQueriesForDerivedTypesOf( definitions, typeof( Entity ), typeof( User ).Assembly );

            For<UnitOfWork>()
                .LifecycleIs( new InteractionScopeLifecycle() )
                .Use( UnitOfWorkFactory.CreateUnitOfWork );

            IncludeRegistry<RavenContainerRegistry>();
            IncludeRegistry<NHibernateContainerRegistry>();
        }

        public static void Initialize()
        {
            ObjectFactory.Initialize( x => x.AddRegistry( new ContainerRegistry() ) );
        }
    }

    public class RavenContainerRegistry : Registry
    {
        public RavenContainerRegistry()
        {
            For<IDocumentSession>()
                .LifecycleIs( new InteractionScopeLifecycle() )
                .Use( () => ObjectFactory.GetInstance<IDocumentStore>().OpenSession() );


            For<IAsyncDocumentSession>()
                .LifecycleIs( new InteractionScopeLifecycle() )
                .Use( () => ObjectFactory.GetInstance<IDocumentStore>().OpenAsyncSession() );

            For<IDocumentStore>()
                .Singleton()
                .Use( () =>
                {
                    var documentStore = new DocumentStore { ConnectionStringName = "RavenConnStr" };

                    return documentStore.Initialize();
                } );
        }
    }

    public class NHibernateContainerRegistry : Registry
    {
        public NHibernateContainerRegistry()
        {
            For<ISession>()
                .LifecycleIs( new InteractionScopeLifecycle() )
                .Use( () => ObjectFactory.GetInstance<ISessionFactory>().OpenSession() );

            For<ISessionFactory>()
                .Singleton()
                .Use(() =>
                {
                    var configuration = Fluently.Configure()
                        .Database(SQLiteConfiguration.Standard.UsingFile("TinyDdd.db"))
                        .Mappings(mappings =>
                            mappings.FluentMappings.AddFromAssembly(System.Reflection.Assembly.GetAssembly(typeof (NHibernateMappings.UserMap)))
                        );

                    SchemaExport schema = new SchemaExport(configuration.BuildConfiguration());
                    schema.Execute(false, true, false);

                    return configuration.BuildSessionFactory();
                });
        }

    }
}