using System;
using NUnit.Framework;
using Pragmatic.EntityFramework.Interaction.StandardQueries;
using Pragmatic.EntityFramework.Tests.Integration.Data;
using Pragmatic.Interaction.StandardQueries;
using SwissKnife;

namespace Pragmatic.EntityFramework.Tests.Integration
{
    // ReSharper disable InconsistentNaming
    [TestFixture]
    public class GetOneQueryHandlerTests // TODO-IG: Define integration tests in a persistence independent way and test them for all persistences.
    {
        [Test]
        public void Returns_person_by_name()
        {
            string name = Guid.NewGuid().ToString();

            var context = new PersonsContext();
            context.Persons.Add(new Person() {Name = name});
            context.SaveChanges();

            Option<Person> person = new GetOneQueryHandler<Person>(new PersonsContext())
                .Execute(new GetOneQuery<Person>()
                {
                    Criteria = persons => persons.Name == name
                });

            Assert.IsTrue(person.IsSome);
            Assert.AreEqual(name, person.Value.Name);
        }
    }
    // ReSharper restore InconsistentNaming
}
