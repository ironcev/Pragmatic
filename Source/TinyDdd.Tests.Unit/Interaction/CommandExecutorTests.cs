using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using TinyDdd.Interaction;

namespace TinyDdd.Tests.Unit.Interaction
{
    [TestFixture]
    public class CommandExecutorTests
    {
        [Test]
        public void Execute_MoreThanOneCommandHandlerDefined_ThrowsException()
        {
            var commandExecutor = new CommandExecutorWithMoreThanOneCommandHandlerDefined();
            var exception = Assert.Throws<NotSupportedException>(() => commandExecutor.Execute<DummyCommand, Response>(new DummyCommand()));
            Assert.That(exception.Message.StartsWith("There are 2 command handlers defined for the commands of type"));
        }

        public class DummyCommand : ICommand { }

        private class CommandExecutorWithMoreThanOneCommandHandlerDefined : CommandExecutor
        {
            protected override IEnumerable<ICommandHandler<TCommand, TResponse>> GetCommandHandlers<TCommand, TResponse>()
            {
                yield return new Mock<ICommandHandler<TCommand, TResponse>>().Object;
                yield return new Mock<ICommandHandler<TCommand, TResponse>>().Object;
            }
        }
    }
}
