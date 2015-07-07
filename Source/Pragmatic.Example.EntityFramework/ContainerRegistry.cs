using System.Data.Entity;
using Pragmatic.EntityFramework.Interaction.StandardQueries;
using Pragmatic.Interaction;
using Pragmatic.Interaction.EntityDeletion;
using Pragmatic.StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace Pragmatic.Example.EntityFramework
{
    public class ContainerRegistry : Registry
    {
        public ContainerRegistry()
        {
            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();

                scan.ConnectImplementationsToTypesClosing(typeof (IQueryHandler<,>));
                scan.ConnectImplementationsToTypesClosing(typeof (ICommandHandler<,>));
                scan.ConnectImplementationsToTypesClosing(typeof(EntityDeleter<>));
            });

            For<UnitOfWork>()
                .LifecycleIs(new InteractionScopeLifecycle())
                .Use<Pragmatic.EntityFramework.UnitOfWork>();

            For<DbContext>()
                .LifecycleIs(new InteractionScopeLifecycle())
                .Use(() => new ExampleDbContext());

            var standardInteractionHandlerGenericTypeDefinitions = new StandardInteractionHandlerGenericTypeDefinitions
                (
                    typeof(GetByIdQueryHandler<>),
                    typeof(GetOneQueryHandler<>),
                    typeof(GetAllQueryHandler<>),
                    typeof(GetTotalCountQueryHandler<>)
                );

            StandardInteractionHandlerRegistration.RegisterStandardInteractionHandlersForEntities(this, standardInteractionHandlerGenericTypeDefinitions);

            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ExampleDbContext>());
        }
    }
}