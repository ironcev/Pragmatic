using System.Data.Entity;
using NUnit.Framework;

namespace Pragmatic.EntityFramework.Tests.Integration.Data
{
    [SetUpFixture]
    public class PrepareData
    {
        [SetUp]
        public void Setup()
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<PersonsContext>());
        }
    }
}