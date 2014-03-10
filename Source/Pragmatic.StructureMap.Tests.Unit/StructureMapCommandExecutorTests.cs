using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Pragmatic.Interaction;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace Pragmatic.StructureMap.Tests.Unit
{
    // ReSharper disable InconsistentNaming
    [TestFixture]
    public class StructureMapCommandExecutorTests
    {
        [Test]
        public void GetCommandHandlers_SingleCommandHandlerExists_ReturnsCommandHandler()
        {
            ObjectFactory.Initialize(x => x.AddRegistry(new StructureMapRegistry()));

            var commandHandlers = new StructureMapCommandExecutorWrapper().GetCommandHandlersWrapper<Response>(typeof(TestCommand)).ToArray();
            Assert.That(commandHandlers.Length, Is.EqualTo(1));
            Assert.That(commandHandlers[0], Is.InstanceOf<TestCommandHandler>());
        }

        public sealed class TestCommand : ICommand<Response> { }
        public sealed class TestCommandHandler : ICommandHandler<TestCommand, Response>
        {
            public Response Execute(TestCommand command) { return null; }
        }
        private class StructureMapCommandExecutorWrapper : StructureMapCommandExecutor
        {
            public IEnumerable<object> GetCommandHandlersWrapper<TResponse>(Type commandType) where TResponse : Response
            {
                return GetCommandHandlers<TResponse>(commandType);
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
                    scan.ConnectImplementationsToTypesClosing(typeof(ICommandHandler<,>));
                });
            }
        }
    }
    // ReSharper restore InconsistentNaming
}
