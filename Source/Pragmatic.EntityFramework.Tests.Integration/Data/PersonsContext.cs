using System.Data.Entity;

namespace Pragmatic.EntityFramework.Tests.Integration.Data
{
    public class PersonsContext : DbContext
    {
        public IDbSet<Person> Persons { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().Ignore(x => x.IsNewEntity);

            base.OnModelCreating(modelBuilder);
        }
    }
}