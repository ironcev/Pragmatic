using FluentNHibernate.Mapping;
using TinyDdd.Example.Model;

namespace TinyDdd.Example.Client.Desktop.NHibernateMappings
{
    public class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Not.LazyLoad();
            Id(x => x.Id).GeneratedBy.Assigned();
            Map( x => x.Email ).Not.Nullable();
            Map( x => x.FirstName ).Not.Nullable();
            Map( x => x.LastName ).Not.Nullable();
        }
    }
}