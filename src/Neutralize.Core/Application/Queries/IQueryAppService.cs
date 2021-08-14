using System;
using System.Threading.Tasks;

namespace Neutralize.Application.Queries
{
    public interface IQueryAppServiceBase<TId, TGetDto, TListDto, TPagedRequest> : IDisposable
        where TId : struct
        where TGetDto : IEntityDto<TId>
        where TListDto : IEntityDto<TId>
        where TPagedRequest : PagedRequestDto
    {
        Task<PagedResultDto<TListDto>> GetPagedList(TPagedRequest input);
    }

    public interface IQueryAppService<TId, TGetDto, TListDto, TPagedRequest>
        : IQueryAppServiceBase<TId, TGetDto, TListDto, TPagedRequest>
        where TId : struct
        where TGetDto : IEntityDto<TId>
        where TListDto : IEntityDto<TId>
        where TPagedRequest : PagedRequestDto
    {
        Task<TGetDto> GetById(TId input);
    }

    public interface IQueryAppService<TId, TGetDto, TListDto, in TGetInput, TPagedRequest>
        : IQueryAppServiceBase<TId, TGetDto, TListDto, TPagedRequest>
        where TId : struct
        where TGetInput: IEntityDto<TId>
        where TGetDto : IEntityDto<TId>
        where TListDto : IEntityDto<TId>
        where TPagedRequest : PagedRequestDto
    {
        Task<TGetDto> GetById(TGetInput input);
    }
}
