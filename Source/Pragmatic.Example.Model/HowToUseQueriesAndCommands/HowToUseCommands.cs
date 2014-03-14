using NUnit.Framework;
using Pragmatic.Example.Model.HowToUseQueriesAndCommands.Commands;
using Pragmatic.Interaction;
using Pragmatic.StructureMap;

namespace Pragmatic.Example.Model.HowToUseQueriesAndCommands
{
    // ReSharper disable InconsistentNaming
    [TestFixture]
    class HowToUseCommands
    {
        private CommandExecutor _commandExecutor;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            Initialization.Initialize();
            _commandExecutor = new CommandExecutor(new StructureMapInteractionHandlerResolver());
        }

        [Test]
        public void Create_User()
        {
            var createUserCommand = new CreateUserCommand
            {
                FirstName = "Jon"
            };

            Response<User> newUser = _commandExecutor.Execute(createUserCommand);

            Assert.That(newUser, Is.Not.Null);
            Assert.That(newUser.Result, Is.Not.Null);
        }

        [Test]
        public void Send_Email()
        {
            var sendEmailCommand = new SendEmailCommand
            {
                EmailAddress = "jon@jondoe.com"
            };

            Response response = _commandExecutor.Execute(sendEmailCommand);

            Assert.That(response, Is.Not.Null);
        }
    }
    // ReSharper restore InconsistentNaming
}
