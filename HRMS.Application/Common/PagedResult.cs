using System;
using System.Collections.Generic;
using System.Linq;

namespace HRMS.Application.Common
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public int TotalPages => PageSize == 0 ? 0 : (int)Math.Ceiling((double)TotalCount / PageSize);
        public bool HasPrevious => PageNumber > 1;
        public bool HasNext => PageNumber < TotalPages;

        public static PagedResult<T> Empty(int pageNumber, int pageSize) => new()
        {
            Items = Enumerable.Empty<T>(),
            TotalCount = 0,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }
}

