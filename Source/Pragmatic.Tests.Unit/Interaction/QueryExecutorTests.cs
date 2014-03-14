using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Pragmatic.Interaction;

namespace Pragmatic.Tests.Unit.Interaction
{
    // ReSharper disable InconsistentNaming
    [TestFixture]
    public class QueryExecutorTests
    {
        [Test]
        public void Execute_MoreThanOneQueryHandlerDefined_ThrowsException()
        {
            var queryExecutor = new QueryExecutor(new InteractionHandlerResolverWithMoreThanOneQueryHandlerDefined());
            var exception = Assert.Throws<NotSupportedException>(() => queryExecutor.Execute(new TestQuery()));
            Console.WriteLine(exception.Message);
            Assert.That(exception.Message.StartsWith("There are 2 query handlers defined for the queries of type"));
        }

        public sealed class TestQuery : IQuery<object> { } // This class must be public in order to use it in Moq mocks.
        private class InteractionHandlerResolverWithMoreThanOneQueryHandlerDefined : IInteractionHandlerResolver
        {
            public IEnumerable<object> ResolveInteractionHandler(Type interactionHandlerType)
            {
                yield return new Mock<IQueryHandler<TestQuery, object>>().Object;
                yield return new Mock<IQueryHandler<TestQuery, object>>().Object;
            }
        }

        [Test]
        public void Execute_QueryHandlerRegistered_ResultReturned()
        {
            var response = new QueryExecutor(new InteractionHandlerResolverWithQueryHandlerRegistered()).Execute(new TestResultQuery());
            Assert.That(response, Is.InstanceOf<TestResult>());
        }
        public class TestResult { } // This class must be public in order to use it in Moq mocks.
        public sealed class TestResultQuery : IQuery<TestResult> { } // This class must be public in order to use it in Moq mocks.
        private class InteractionHandlerResolverWithQueryHandlerRegistered : IInteractionHandlerResolver
        {
            public IEnumerable<object> ResolveInteractionHandler(Type interactionHandlerType)
            {
                var mock = new Mock<IQueryHandler<TestResultQuery, TestResult>>();
                mock.Setup(x => x.Execute(It.IsAny<TestResultQuery>())).Returns(new TestResult());
                yield return mock.Object;
            }
        }
    }
    // ReSharper restore InconsistentNaming
}
