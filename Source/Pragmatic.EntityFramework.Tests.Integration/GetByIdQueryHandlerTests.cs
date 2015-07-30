// !!! WARNING: Intentionally bad code! !!!
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
    public class GetByIdQueryHandlerTests
    {
        [Test]
        public void Insert_and_return_by_id()
        {
            Guid id;
            using (var db = new PragmaticDbContext())
            {
                var newPerson = new Person { Name = "Han Solo"};
                id = newPerson.Id;

                db.Persons.Add(newPerson);
                db.SaveChanges();
            }

            Option<Person> person = new GetByIdQueryHandler<Person>(new PragmaticDbContext())
                .Execute(new GetByIdQuery<Person> { Id = id });

            Assert.IsTrue(person.IsSome);
            Assert.AreEqual("Han Solo", person.Value.Name);
        }

        [Test]
        public void Insert_and_return_by_id_as_object()
        {
            Guid id;
            using (var db = new PragmaticDbContext())
            {
                var newPerson = new Person { Name = "Han Solo" };
                id = newPerson.Id;

                db.Persons.Add(newPerson);
                db.SaveChanges();
            }

            Option<object> person = new GetByIdQueryHandler(new PragmaticDbContext())
                .Execute(new GetByIdQuery{ EntityType = typeof(Person), EntityId = id });

            Assert.IsTrue(person.IsSome);
            Assert.That(person.Value.GetType(), Is.EqualTo(typeof(Person)));
            Assert.AreEqual("Han Solo", ((Person)person.Value).Name);
        }
    }
    // ReSharper restore InconsistentNaming
}