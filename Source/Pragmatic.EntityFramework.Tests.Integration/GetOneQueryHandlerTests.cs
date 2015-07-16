using System;
using NUnit.Framework;
using Pragmatic.EntityFramework.Interaction.StandardQueries;
using Pragmatic.EntityFramework.Tests.Integration.Data;
using Pragmatic.Interaction;
using Pragmatic.Interaction.StandardQueries;
using SwissKnife;

namespace Pragmatic.EntityFramework.Tests.Integration
{
    // ReSharper disable InconsistentNaming
    [TestFixture]
    public class GetOneQueryHandlerTests // TODO-IG: Define integration tests in a persistence independent way and test them for all persistence's.
    {
        [Test]
        public void Returns_person_by_name_ordered_by_age()
        {
            string name = Guid.NewGuid().ToString();
            int age = 20;

            var context = new PragmaticDbContext();
            context.Persons.Add(new Person() { Name = name, Age = age });
            context.Persons.Add(new Person() { Name = name, Age = age + 1 });
            context.SaveChanges();

            Option<Person> person = new GetOneQueryHandler<Person>(new PragmaticDbContext())
                .Execute(new GetOneQuery<Person>()
                {
                    Criteria = persons => persons.Name == name,
                    OrderBy = new OrderBy<Person>(persons => persons.Age)
                });

            Assert.IsTrue(person.IsSome);
            Assert.AreEqual(name, person.Value.Name);
            Assert.AreEqual(age, person.Value.Age);
        }

        [Test]
        public void Returns_person_by_name_ordered_by_age_descending()
        {
            string name = Guid.NewGuid().ToString();
            int age = 20;

            var context = new PragmaticDbContext();
            context.Persons.Add(new Person() { Name = name, Age = age });
            context.Persons.Add(new Person() { Name = name, Age = age + 1 });
            context.SaveChanges();

            Option<Person> person = new GetOneQueryHandler<Person>(new PragmaticDbContext())
                .Execute(new GetOneQuery<Person>()
                {
                    Criteria = persons => persons.Name == name,
                    OrderBy = new OrderBy<Person>(persons => persons.Age, OrderByDirection.Descending)
                });

            Assert.IsTrue(person.IsSome);
            Assert.AreEqual(name, person.Value.Name);
            Assert.AreEqual(age + 1, person.Value.Age);
        }
    }
    // ReSharper restore InconsistentNaming
}
