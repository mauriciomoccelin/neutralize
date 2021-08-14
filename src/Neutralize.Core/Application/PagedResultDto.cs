using System.Collections.Generic;
using System.Linq;

namespace Neutralize.Application
{
    public class PagedResultDto<TItem>
    {
        private static readonly PagedResultDto<TItem> empty = new();

        public long TotalCount { get; }
        public IEnumerable<TItem> Items { get; }

        public PagedResultDto()
        {
            TotalCount = 0;
            Items = new List<TItem>();
        }

        public PagedResultDto(long totalCount, IEnumerable<TItem> items)
        {
            TotalCount = totalCount;
            Items = items ?? Enumerable.Empty<TItem>();
        }

        public static PagedResultDto<TItem> Empty() => empty;
    }
}
