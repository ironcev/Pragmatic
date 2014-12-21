using System.Data.Entity;

namespace Pragmatic.EntityFramework.Tests.Integration.Data
{
    public class PersonsContext : DbContext
    {
        public IDbSet<Person> Persons { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // TODO-IG: Find a way not to map the IsNewEntity property but to ensure that is always set to false.

            base.OnModelCreating(modelBuilder);
        }
    }
}