using System;
using System.Threading.Tasks;

namespace Neutralize.Application.Queries
{
    public interface IQueryAppService<in TId, TGetDto, TListDto, in TPagedRequest> : IDisposable
        where TId : struct
        where TGetDto : IEntityDto<TId>
        where TListDto : IEntityDto<TId>
        where TPagedRequest : PagedRequestDto
    {
        Task<TGetDto> GetById(TId input);
        Task<PagedResultDto<TListDto>> GetPagedList(TPagedRequest input);
    }
}
