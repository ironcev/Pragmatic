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
    public class StructureMapQueryExecutorTests
    {
        [Test]
        public void GetQueryHandlers_SingleQueryHandlerExists_ReturnsQueryHandler()
        {
            ObjectFactory.Initialize(x => x.AddRegistry(new StructureMapRegistry()));

            var queryHandlers = new StructureMapQueryExecutorWrapper().GetQueryHandlersWrapper(typeof(TestQuery), typeof(object)).ToArray();
            Assert.That(queryHandlers.Length, Is.EqualTo(1));
            Assert.That(queryHandlers[0], Is.InstanceOf<TestQueryHandler>());
        }

        public class TestQuery : IQuery<object> { }
        public class TestQueryHandler : IQueryHandler<TestQuery, object>
        {
            public object Execute(IQuery command) { return null; }
        }
        private class StructureMapQueryExecutorWrapper : StructureMapQueryExecutor
        {
            public IEnumerable<IQueryHandler> GetQueryHandlersWrapper(Type queryType, Type queryResultType)
            {
                return GetQueryHandlers(queryType, queryResultType);
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
                    scan.ConnectImplementationsToTypesClosing(typeof(IQueryHandler<,>));
                });
            }
        }
    }
    // ReSharper restore InconsistentNaming
}
