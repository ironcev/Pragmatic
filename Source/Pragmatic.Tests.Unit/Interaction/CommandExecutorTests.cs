using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using Pragmatic.Interaction;

namespace Pragmatic.Tests.Unit.Interaction
{
    // ReSharper disable InconsistentNaming
    [TestFixture]
    public class CommandExecutorTests
    {
        [Test]
        public void Execute_MoreThanOneCommandHandlerDefined_ThrowsException()
        {
            var commandExecutor = new CommandExecutor(new InteractionHandlerResolverWithMoreThanOneCommandHandlerDefined());
            var exception = Assert.Throws<NotSupportedException>(() => commandExecutor.Execute(new TestCommand()));
            Console.WriteLine(exception.Message);
            Assert.That(exception.Message.StartsWith("There are 2 command handlers defined for the commands of type"));
        }

        public sealed class TestCommand : ICommand<Response> { } // This class must be public in order to use it in Moq mocks.
        private class InteractionHandlerResolverWithMoreThanOneCommandHandlerDefined : IInteractionHandlerResolver
        {
            public IEnumerable<object> ResolveInteractionHandler(Type interactionHandlerType)
            {
                yield return new Mock<ICommandHandler<TestCommand, Response>>().Object;
                yield return new Mock<ICommandHandler<TestCommand, Response>>().Object;
            }
        }

        [Test]
        public void Execute_CommandHandlerWithValidResponseType_ResponseReturned()
        {
            var response = new CommandExecutor(new InteractionHandlerResolverWithCommandHandlerWithValidResponseType()).Execute(new TestResponseCommand());
            Assert.That(response, Is.InstanceOf<TestResponse>());
        }

        public class TestResponse : Response<object> { }
        public sealed class TestResponseCommand : ICommand<TestResponse> { } // This class must be public in order to use it in Moq mocks.
        private class InteractionHandlerResolverWithCommandHandlerWithValidResponseType : IInteractionHandlerResolver
        {
            public IEnumerable<object> ResolveInteractionHandler(Type interactionHandlerType)
            {
                var mock = new Mock<ICommandHandler<TestResponseCommand, TestResponse>>();
                mock.Setup(x => x.Execute(It.IsAny<TestResponseCommand>())).Returns(new TestResponse());
                yield return mock.Object;
            }
        }
    }
    // ReSharper restore InconsistentNaming
}
