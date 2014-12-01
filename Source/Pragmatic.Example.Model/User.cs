namespace Pragmatic.Example.Model
{
    public class User : Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public string FullName { get { return (FirstName + " " + LastName).Trim(); } }

        public bool IsAdministrator { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}