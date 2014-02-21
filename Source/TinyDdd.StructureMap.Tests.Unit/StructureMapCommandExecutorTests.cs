using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using StructureMap;
using StructureMap.Configuration.DSL;
using TinyDdd.Interaction;

namespace TinyDdd.StructureMap.Tests.Unit
{
    // ReSharper disable InconsistentNaming
    [TestFixture]
    public class StructureMapCommandExecutorTests
    {
        [Test]
        public void GetCommandHandlers_CommandHandlerExists_ReturnsCommandHandler()
        {
            ObjectFactory.Initialize(x => x.AddRegistry(new StructureMapRegistry()));

            var commandHandlers = new StructureMapCommandExecutorWrapper().GetCommandHandlersWrapper(typeof(TestCommand)).ToArray();
            Assert.That(commandHandlers.Length, Is.EqualTo(1));
            Assert.That(commandHandlers[0], Is.InstanceOf<TestCommandHandler>());
        }

        public class TestCommand : ICommand<Response> { }
        public class TestCommandHandler : ICommandHandler<TestCommand>
        {
            public Response Execute(TestCommand command) { return null; }
            public Response Execute(ICommand command) { return null; }
        }
        private class StructureMapCommandExecutorWrapper : StructureMapCommandExecutor
        {
            public IEnumerable<ICommandHandler> GetCommandHandlersWrapper(Type commandType)
            {
                return GetCommandHandlers(commandType);
            }
        }

        private class StructureMapRegistry : Registry
        {
            internal StructureMapRegistry()
            {
                Scan(scan =>
                {
                    scan.TheCallingAssembly();
                    scan.WithDefaultConventions();
                    scan.ConnectImplementationsToTypesClosing(typeof(ICommandHandler<>));
                });
            }
        }
    }
    // ReSharper restore InconsistentNaming
}
