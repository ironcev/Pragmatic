using System;
using TinyDdd.Examples.Model;
using TinyDdd.Interaction;

namespace TinyDdd.Examples.Queries
{
    public class GetUserByIdQuery : IQuery<User>
    {
        public Guid Id { get; set; }
    }
}
