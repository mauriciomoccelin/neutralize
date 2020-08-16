namespace BuildingBlocks.Application
{
    public class PagedRequestDto<TDto> where TDto : class
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string Keyword { get; set; }

        public PagedRequestDto() { }

        public PagedRequestDto(int page, int pageSize, string keyword)
        {
            Page = page;
            PageSize = pageSize;
            Keyword = keyword;
        }
    }
}