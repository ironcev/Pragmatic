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
            using (var db = new PersonsContext())
            {
                var newPerson = new Person { Name = "Han Solo"};
                id = newPerson.Id;

                db.Persons.Add(newPerson);
                db.SaveChanges();
            }

            Option<Person> person = new GetByIdQueryHandler<Person>(new PersonsContext())
                .Execute(new GetByIdQuery<Person> { Id = id });

            Assert.IsTrue(person.IsSome);
            Assert.AreEqual("Han Solo", person.Value.Name);
        }
    }
    // ReSharper restore InconsistentNaming
}