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
    public class StructureMapQueryExecutorTests
    {
        [Test]
        public void GetQueryHandlers_SingleQueryHandlerExists_ReturnsQueryHandler()
        {
            ObjectFactory.Initialize(x => x.AddRegistry(new StructureMapRegistry()));

            var queryHandlers = new StructureMapQueryExecutorWrapper().GetQueryHandlersWrapper<object>(typeof(TestQuery)).ToArray();
            Assert.That(queryHandlers.Length, Is.EqualTo(1));
            Assert.That(queryHandlers[0], Is.InstanceOf<TestQueryHandler>());
        }

        public sealed class TestQuery : IQuery<object> { }
        public sealed class TestQueryHandler : IQueryHandler<TestQuery, object>
        {
            public object Execute(TestQuery command) { return null; }
        }
        private class StructureMapQueryExecutorWrapper : StructureMapQueryExecutor
        {
            public IEnumerable<object> GetQueryHandlersWrapper<TResult>(Type queryType)
            {
                return GetQueryHandlers<TResult>(queryType);
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
