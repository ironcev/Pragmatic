using Pragmatic.EntityFramework.Interaction.StandardQueries;
using Pragmatic.EntityFramework.Tests.Integration.Data;

namespace Pragmatic.EntityFramework.Tests.Integration
{
    public abstract class BaseTest
    {
        protected static GetOneQueryHandler<Person> GetHandler()
        {
            return new GetOneQueryHandler<Person>(new PeopleContext());
        }
    }
}
