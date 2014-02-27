using NUnit.Framework;
using TinyDdd.Example.Model.HowToUseQueriesAndCommands.Commands;
using TinyDdd.Interaction;
using TinyDdd.StructureMap;

namespace TinyDdd.Example.Model
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
            _commandExecutor = new StructureMapCommandExecutor();
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
