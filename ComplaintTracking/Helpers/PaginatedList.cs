using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComplaintTracking.Generic
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; }
        public int PageSize => CTS.PageSize;
        public int TotalItems { get; }

        public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);
        public int FirstItemIndex => Math.Min(PageSize * (PageIndex - 1) + 1, TotalItems);
        public int LastItemIndex => Math.Min(PageSize * PageIndex, TotalItems);

        public bool HasPreviousPage => (PageIndex > 1);
        public bool HasNextPage => (PageIndex < TotalPages);

        public PaginatedList(IEnumerable<T> items, int totalCount, int pageNumber)
        {
            PageIndex = pageNumber;
            TotalItems = totalCount;
            
            AddRange(items);
        }
    }
}
