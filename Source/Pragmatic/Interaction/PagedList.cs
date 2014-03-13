using System;
using System.Collections.Generic;
using System.Linq;
using SwissKnife.Diagnostics.Contracts;

namespace Pragmatic.Interaction
{
    public class PagedList<T> : IPagedEnumerable<T>
    {
        private readonly List<T> _innerList;

        public PagedList(IEnumerable<T> source, int currentPage, int pageSize, int totalCount)
        {
            Argument.IsNotNull(source, "source");

            _innerList = source.ToList();

            Argument.IsValid(_innerList.Count <= totalCount, string.Format("Source count cannot be greater than total count. Source count was '{0}' and total count was '{1}'.", _innerList.Count, totalCount), "source");
            Argument.IsValid(currentPage > 0, string.Format("Current page must be greater than zero. Current page was: '{0}'.", currentPage), "currentPage");
            Argument.IsValid(pageSize > 0, string.Format("Page size must be greater than zero. Page size was: '{0}'.", pageSize), "pageSize");
            Argument.IsValid(totalCount >= 0, string.Format("Total count must be greater or equal to zero. Total count was: '{0}'.", totalCount), "totalCount");

            // Argument.IsValid( ( totalCount > pageSize * ( currentPage - 1 ) ) || totalCount == 0, string.Format( "Total count must be greater or equal to zero. If greater than zero, it must be also greater than (pageSize * ( currentPage - 1 ). Total count was '{0}', page size was '{1}' and current page was '{2}'.", totalCount, pageSize, currentPage ), "totalCount" );

            TotalCount = totalCount;
            CurrentPage = currentPage;
            PageSize = pageSize;
        }

        public int TotalCount { get; private set; }

        public int PageSize { get; private set; }

        public int CurrentPage { get; private set; }

        public int TotalPages { get { return (int)Math.Ceiling((double)TotalCount / PageSize); } }

        public IEnumerator<T> GetEnumerator() { return _innerList.GetEnumerator(); }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() { return _innerList.GetEnumerator(); }
    }
}
