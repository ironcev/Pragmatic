using System;
using NUnit.Framework;
using Pragmatic.EntityFramework.Interaction.StandardQueries;
using Pragmatic.EntityFramework.Tests.Integration.Data;
using Pragmatic.Interaction.StandardQueries;
using SwissKnife;

namespace Pragmatic.EntityFramework.Tests.Integration
{
    [TestFixture]
    public class GetOneQueryHandlerTests : BaseTest
    {
        [Test]
        public void Returns_person_by_name()
        {
            string name = Guid.NewGuid().ToString();

            var context = new PeopleContext();
            context.Persons.Add(new Person() {Name = name});
            context.SaveChanges();

            Option<Person> personsMaybe = new GetOneQueryHandler<Person>(new PeopleContext())
                .Execute(new GetOneQuery<Person>()
                {
                    Criteria = persons => persons.Name == name
                });

            Assert.IsTrue(personsMaybe.IsSome);
            Assert.AreEqual(name, personsMaybe.Value.Name);
        }
    }
}
