using System.Collections.Generic;

namespace TinyDdd.Interaction.StandardQueries
{
    public sealed class GetAllQuery<T> : IQuery<IEnumerable<T>> where T : class
    {
    }
}
