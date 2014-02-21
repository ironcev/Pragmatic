using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using TinyDdd.Interaction;

namespace TinyDdd.Tests.Unit.Interaction
{
    // ReSharper disable InconsistentNaming
    [TestFixture]
    public class CommandExecutorTests
    {
        [Test]
        public void Execute_MoreThanOneCommandHandlerDefined_ThrowsException()
        {
            var commandExecutor = new CommandExecutorWithMoreThanOneCommandHandlerDefined();
            var exception = Assert.Throws<NotSupportedException>(() => commandExecutor.Execute(new TestCommand()));
            Console.WriteLine(exception.Message);
            Assert.That(exception.Message.StartsWith("There are 2 command handlers defined for the commands of type"));
        }

        public class TestCommand : ICommand<Response> { } // This class must be public in order to use it in Moq mocks.
        private class CommandExecutorWithMoreThanOneCommandHandlerDefined : CommandExecutor
        {
            protected override IEnumerable<ICommandHandler> GetCommandHandlers(Type commandType)
            {
                yield return new Mock<ICommandHandler<TestCommand>>().Object;
                yield return new Mock<ICommandHandler<TestCommand>>().Object;
            }
        }

        [Test]
        public void Execute_CommandHandlerWithValidResponseType_ResponseReturned()
        {
            var response = new CommandExecutorWithCommandHandlerWithValidResponseType().Execute(new TestResponseCommand());
            Assert.That(response, Is.InstanceOf<TestResponse>());
        }

        [Test]
        public void Execute_CommandHandlerWithInvalidResponseType_ResponseReturned()
        {
            var commandExecutor = new CommandExecutorWithCommandHandlerWithInvalidResponseType();
            var exception = Assert.Throws<CommandExecutionException>(() => commandExecutor.Execute(new TestResponseCommand()));
            Console.WriteLine(exception.Message);
            Assert.That(exception.Message.StartsWith("An exception occured while converting the response of the command handler of type"));
        }

        private class TestResponse : Response<object> { }
        public class TestResponseCommand : ICommand<TestResponse> { } // This class must be public in order to use it in Moq mocks.
        private class CommandExecutorWithCommandHandlerWithValidResponseType : CommandExecutor
        {
            protected override IEnumerable<ICommandHandler> GetCommandHandlers(Type commandType)
            {
                var mock = new Mock<ICommandHandler<TestResponseCommand>>();
                mock.Setup(x => x.Execute(It.IsAny<TestResponseCommand>())).Returns(new TestResponse());
                yield return mock.Object;
            }
        }
        private class CommandExecutorWithCommandHandlerWithInvalidResponseType : CommandExecutor
        {
            protected override IEnumerable<ICommandHandler> GetCommandHandlers(Type commandType)
            {
                var mock = new Mock<ICommandHandler<TestResponseCommand>>();
                mock.Setup(x => x.Execute(It.IsAny<TestResponseCommand>())).Returns(new Response());
                yield return mock.Object;
            }
        }
    }
    // ReSharper restore InconsistentNaming
}
