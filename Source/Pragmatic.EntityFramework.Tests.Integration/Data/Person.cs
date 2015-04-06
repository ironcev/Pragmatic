namespace Pragmatic.EntityFramework.Tests.Integration.Data
{
    public class Person : Entity
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public bool IsAdministrator { get; set; }
    }
}