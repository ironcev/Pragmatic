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

        public sealed class TestCommand : ICommand<Response> { } // This class must be public in order to use it in Moq mocks.
        private class CommandExecutorWithMoreThanOneCommandHandlerDefined : CommandExecutor
        {
            protected override IEnumerable<object> GetCommandHandlers<TResponse>(Type commandType)
            {
                yield return new Mock<ICommandHandler<TestCommand, Response>>().Object;
                yield return new Mock<ICommandHandler<TestCommand, Response>>().Object;
            }
        }

        [Test]
        public void Execute_CommandHandlerWithValidResponseType_ResponseReturned()
        {
            var response = new CommandExecutorWithCommandHandlerWithValidResponseType().Execute(new TestResponseCommand());
            Assert.That(response, Is.InstanceOf<TestResponse>());
        }

        public class TestResponse : Response<object> { }
        public sealed class TestResponseCommand : ICommand<TestResponse> { } // This class must be public in order to use it in Moq mocks.
        private class CommandExecutorWithCommandHandlerWithValidResponseType : CommandExecutor
        {
            protected override IEnumerable<object> GetCommandHandlers<TResponse>(Type commandType)
            {
                var mock = new Mock<ICommandHandler<TestResponseCommand, TestResponse>>();
                mock.Setup(x => x.Execute(It.IsAny<TestResponseCommand>())).Returns(new TestResponse());
                yield return mock.Object;
            }
        }
    }
    // ReSharper restore InconsistentNaming
}
