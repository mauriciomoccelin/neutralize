using BuildingBlocks.Core.Application;
using MediatR;

namespace BuildingBlocks.Core.Commands
{
    public abstract class GetPageResultCommand<TDto, TId> : Command<TId>, IRequest<PagedResultDto<TDto>> where TId: struct where TDto : class
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string Keyword { get; set; }

        protected GetPageResultCommand()
        {
            Page = 0;
            PageSize = 10;
            Keyword = string.Empty;
        }

        protected GetPageResultCommand(int page, int pageSize, string keyword)
        {
            Page = page;
            PageSize = pageSize;
            Keyword = keyword;
        }
    }
}