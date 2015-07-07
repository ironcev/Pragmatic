using Pragmatic.Interaction;

namespace Pragmatic.Example.Model.Users
{
    public class GetUsersQuery : IEquatableQuery<GetUsersQuery, User[]>
    {
        public string SearchTerm { get; set; }
        public bool? IsAdministrator { get; set; }

        public bool IsEquivalentTo(GetUsersQuery query)
        {
            if (query == null) return false;

            if (ReferenceEquals(this, query)) return true;

            return NormalizeSeachTerm(SearchTerm).Equals(NormalizeSeachTerm(query.SearchTerm))
                   && IsAdministrator == query.IsAdministrator;
        }

        public static string NormalizeSeachTerm(string searchTerm)
        {
            return (searchTerm ?? string.Empty).Trim().ToLower();
        }
    }
}
