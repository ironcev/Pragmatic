using System.Data.Entity;

namespace Pragmatic.EntityFramework.Tests.Integration.Data
{
    public class PragmaticDbContext : DbContext
    {
        public IDbSet<Person> Persons { get; set; }

        public PragmaticDbContext() : base("PragmaticIntegrationTests")
        {
            
        }
    }
}