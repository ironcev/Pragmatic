using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using TinyDdd.Examples.Model;
using TinyDdd.Interaction;

namespace TinyDdd.Examples.Queries
{
    class GetUserByIdQuery : IQuery<User>
    {
        public Guid Id { get; set; }
    }
}
