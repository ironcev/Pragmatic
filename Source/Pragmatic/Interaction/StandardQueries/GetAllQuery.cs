using System.Collections.Generic;

namespace Pragmatic.Interaction.StandardQueries
{
    public sealed class GetAllQuery<T> : IQuery<IEnumerable<T>> where T : class
    {
    }
}
