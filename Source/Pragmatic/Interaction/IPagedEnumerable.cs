using System.Collections.Generic;

namespace Pragmatic.Interaction
{
    /// <summary>
    /// Represents a subset of a original list of objects with additional information about the list in whole.
    /// </summary>
    public interface IPagedEnumerable<out T> : IEnumerable<T>
    {
        /// <summary>
        /// Total number of elements in original (not paged) list.
        /// </summary>
        int TotalCount
        {
            get;
        }

        /// <summary>
        /// Maximum size of a subset of original list.
        /// </summary>
        int PageSize
        {
            get;
        }

        /// <summary>
        /// Current subset number of original list.
        /// </summary>
        int CurrentPage
        {
            get;
        }

        /// <summary>
        /// Total number of subsets in original list.
        /// </summary>
        int TotalPages
        {
            get;
        }
    }
}
