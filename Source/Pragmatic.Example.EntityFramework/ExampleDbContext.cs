using System.Data.Entity;
using Pragmatic.Example.Model;

namespace Pragmatic.Example.EntityFramework
{
    public class ExampleDbContext : DbContext
    {
        public IDbSet<User> Users { get; set; } 

        public ExampleDbContext() : base("EntityFrameworkConnectionString") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Ignore(user => user.IsNewEntity); // TODO-IG: Use custom code conventions to ignore IsNewEntity on all entities.

            base.OnModelCreating(modelBuilder);
        }
    }
}
