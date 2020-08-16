using System.Collections.Generic;

namespace BuildingBlocks.Application
{
    public class PagedResultDto<TDto> where TDto : class
    {
        public long TotalCount { get; }
        public IEnumerable<TDto> Items { get; }

        public PagedResultDto()
        {
            TotalCount = 0;
            Items = new List<TDto>();
        }

        public PagedResultDto(long totalCount, IEnumerable<TDto> items)
        {
            TotalCount = totalCount;
            Items = items;
        }
    }
}