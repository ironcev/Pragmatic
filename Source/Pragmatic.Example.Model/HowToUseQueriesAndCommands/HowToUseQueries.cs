using System;
using System.Collections.Generic;
using NUnit.Framework;
using Pragmatic.Example.Model.HowToUseQueriesAndCommands.Dtos;
using Pragmatic.Example.Model.HowToUseQueriesAndCommands.Queries;
using Pragmatic.Interaction;
using Pragmatic.StructureMap;

namespace Pragmatic.Example.Model.HowToUseQueriesAndCommands
{
    // ReSharper disable InconsistentNaming
    [TestFixture]
    class HowToUseQueries
    {
        private QueryExecutor _queryExecutor;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            Initialization.Initialize();
            _queryExecutor = new QueryExecutor(new StructureMapInteractionHandlerResolver());
        }

        [Test]
        public void Get_User_By_Id()
        {
            var getUserByIdQuery = new GetUserByIdQuery
            {
                Id = Guid.NewGuid()
            };

            User user = _queryExecutor.Execute(getUserByIdQuery);

            Assert.That(user, Is.Not.Null);
        }

        [Test]
        public void Get_All_Users()
        {
            var getAllUsersQuery = new GetAllUsersQuery();

            IEnumerable<User> user = _queryExecutor.Execute<GetAllUsersQuery, IEnumerable<User>>(getAllUsersQuery);

            Assert.That(user, Is.Not.Null);
        }

        [Test]
        public void Get_All_UserDtos()
        {
            var getAllUsersQuery = new GetAllUsersQuery();

            IEnumerable<UserDto> user = _queryExecutor.Execute<GetAllUsersQuery, IEnumerable<UserDto>>(getAllUsersQuery);

            Assert.That(user, Is.Not.Null);
        }
    }
    // ReSharper restore InconsistentNaming
}
