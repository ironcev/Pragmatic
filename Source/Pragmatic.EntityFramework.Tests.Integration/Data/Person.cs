using System;

namespace Pragmatic.EntityFramework.Tests.Integration.Data
{
    public class Person : Entity
    {
        public Person()
        {
            // Easy, lazy, and a bad way to avoid: The conversion of a datetime2 data type to a datetime data type resulted in an out-of-range value.
            DateOfBirth = DateTime.Now;
        }

        public string Name { get; set; }

        public int Age { get; set; }

        public bool IsAdministrator { get; set; }

        public DateTime DateOfBirth { get; set; }
    }
}