using System.Linq;
using NUnit.Framework;
using Pragmatic.Interaction;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace Pragmatic.StructureMap.Tests.Unit
{
    // ReSharper disable InconsistentNaming
    [TestFixture]
    public class StructureMapInteractionHandlerResolverTests
    {
        [Test]
        public void ResolveInteractionHandler_SingleQueryHandlerExists_ReturnsQueryHandler()
        {
            ObjectFactory.Initialize(x => x.AddRegistry(new StructureMapRegistry()));

            var queryHandlers = new StructureMapInteractionHandlerResolver().ResolveInteractionHandler(typeof(IQueryHandler<TestQuery, object>)).ToArray();
            Assert.That(queryHandlers.Length, Is.EqualTo(1));
            Assert.That(queryHandlers[0], Is.InstanceOf<TestQueryHandler>());
        }

        public sealed class TestQuery : IQuery<object> { }
        public sealed class TestQueryHandler : IQueryHandler<TestQuery, object>
        {
            public object Execute(TestQuery command) { return null; }
        }

        [Test]
        public void GetCommandHandlers_SingleCommandHandlerExists_ReturnsCommandHandler()
        {
            ObjectFactory.Initialize(x => x.AddRegistry(new StructureMapRegistry()));

            var commandHandlers = new StructureMapInteractionHandlerResolver().ResolveInteractionHandler(typeof(ICommandHandler<TestCommand, Response>)).ToArray();
            Assert.That(commandHandlers.Length, Is.EqualTo(1));
            Assert.That(commandHandlers[0], Is.InstanceOf<TestCommandHandler>());
        }

        public sealed class TestCommand : ICommand<Response> { }
        public sealed class TestCommandHandler : ICommandHandler<TestCommand, Response>
        {
            public Response Execute(TestCommand command) { return null; }
        }

        private class StructureMapRegistry : Registry
        {
            internal StructureMapRegistry()
            {
                Scan(scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                    scan.ConnectImplementationsToTypesClosing(typeof(IQueryHandler<,>));
                    scan.ConnectImplementationsToTypesClosing(typeof(ICommandHandler<,>));
                });
            }
        }
    }
    // ReSharper restore InconsistentNaming
}
