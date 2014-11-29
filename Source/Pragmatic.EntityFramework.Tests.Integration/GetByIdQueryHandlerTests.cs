using System;
using NUnit.Framework;
using Pragmatic.EntityFramework.Interaction.StandardQueries;
using Pragmatic.EntityFramework.Tests.Integration.Data;
using Pragmatic.Interaction.StandardQueries;
using SwissKnife;

namespace Pragmatic.EntityFramework.Tests.Integration
{
    [TestFixture]
    public class GetByIdQueryHandlerTests : BaseTest
    {
        [Test]
        public void Insert_and_return_by_id()
        {
            Guid id;
            using (var db = new PeopleContext())
            {
                var person = new Person();
                person.Name = "abc";
                id = person.Id;

                db.Persons.Add(person);
                db.SaveChanges();
            }

            Option<Person> personsMaybe = new GetByIdQueryHandler<Person>(new PeopleContext())
                .Execute(new GetByIdQuery<Person> { Id = id });

            Assert.IsTrue(personsMaybe.IsSome);
            Assert.AreEqual("abc", personsMaybe.Value.Name);
        }
    }
}