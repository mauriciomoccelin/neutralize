namespace Neutralize.Application
{
    public class PagedRequestDto : IShouldNormalizer
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

        public void Normalize()
        {
            if (PageSize > 1000)
            {
                PageSize = 1000;
            }

            Keyword = Keyword?.ToLower();
        }
    }
}
