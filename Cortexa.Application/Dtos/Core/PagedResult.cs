using System;
using System.Collections.Generic;
using System.Text;

namespace Cortexa.Application.Dtos.Core
{
    public class PagedResult<T>
    {
        public IReadOnlyList<T> Items { get; set; } = [];

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public int TotalPages =>
            (int)Math.Ceiling((double)TotalCount / PageSize);

        public PagedResult(
       int pageNumber,
       int pageSize,
       int totalCount,
       IReadOnlyList<T> items)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalCount = totalCount;
            Items = items;
        }
    }
}
