using NUnit.Framework;
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
            string name = "User 5";

            Option<Person> personsMaybe = GetHandler().Execute(new GetOneQuery<Person>()
            {
                Criteria = persons => persons.Name == name
            });

            Assert.IsTrue(personsMaybe.IsSome);
            Assert.AreEqual(name, personsMaybe.Value.Name);
        }
    }
}
