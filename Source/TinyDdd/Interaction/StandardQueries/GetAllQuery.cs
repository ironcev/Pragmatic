using System.Collections.Generic;

namespace TinyDdd.Interaction.StandardQueries
{
    public class GetAllQuery<T> : IQuery<IEnumerable<T>> where T : class
    {
    }
}
