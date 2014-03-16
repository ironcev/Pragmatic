using System.Collections.Generic;
using System.Linq;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.Interaction
{
    public class Paging // TODO-IG: Equality.
    {
        public static readonly Paging None = new Paging();

        public Paging() : this(1, int.MaxValue)
        {
        }

        public Paging(int page, int pageSize)
        {
            Argument.IsGreaterThanZero(page, "page");
            Argument.IsGreaterThanZero(pageSize, "pageSize");

            Page = page;
            PageSize = pageSize;
        }

        /// <summary>
        /// One-based page to retrieve.
        /// </summary>
        public int Page { get; private set; }

        /// <summary>
        /// Number of entities to retrieve per page.
        /// </summary>
        public int PageSize { get; private set; }

        // ReSharper disable CSharpWarnings::CS1584
        // ReSharper disable CSharpWarnings::CS1580
        /// <summary>
        /// Number of entities to be skipped in order to get the first entity on the page.
        /// This value can be used in <see cref="Enumerable.Skip(IEnumerable{TSource},int)"/>. 
        /// </summary>
        // ReSharper restore CSharpWarnings::CS1580
        // ReSharper restore CSharpWarnings::CS1584
        public int Skip { get { return (Page - 1) * PageSize; } }


        public bool IsNone { get { return Page == 1 && PageSize == int.MaxValue; } }
    }
}
