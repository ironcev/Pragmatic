using System.Data.Entity;
using NUnit.Framework;
using Pragmatic.EntityFramework.Tests.Integration.Data;

namespace Pragmatic.EntityFramework.Tests.Integration
{
    [SetUpFixture]
    public class PrepareData
    {
        [SetUp]
        public void Setup()
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<PeopleContext>());

            var peopleContext = new PeopleContext();

            for (int i = 0; i < 100; i++)
            {
                peopleContext.Persons.Add(new Person()
                {
                    Name = "User " + i,
                    Age = i
                });
            }

            peopleContext.SaveChanges();
        }
    }
}