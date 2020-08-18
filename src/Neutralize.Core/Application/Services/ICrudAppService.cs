using System.Threading.Tasks;

namespace Neutralize.Application.Services
{
    public interface ICrudAppService
        <TId, in TCreateDto, in TUpdateDto, in TDeleteDto, TGetDto, TListDto, in TGetInput, in TListInput>
        : IApplicationService
        where TId : struct
        where TCreateDto: IEntityDto<TId> 
        where TUpdateDto : IEntityDto<TId> 
        where TDeleteDto : IEntityDto<TId> 
        where TGetDto : IEntityDto<TId>
        where TListDto :  IEntityDto<TId> 
        where TGetInput : IEntityDto<TId> 
        where TListInput : PagedRequestDto
    {
        Task<TId> Create(TCreateDto input);
        Task<TId> Update(TUpdateDto input);
        Task Delete(TDeleteDto input);
        
        Task<TGetDto> Get(TGetInput input);
        Task<PagedResultDto<TListDto>> GetList(TListInput input);
    }
    
    public interface ICrudAppService<TId, in TCreateDto, in TUpdateDto, in TDeleteDto, TGetDto, in TGetInput> :
        ICrudAppService<TId, TCreateDto, TUpdateDto, TDeleteDto, TGetDto, TGetDto, TGetInput, PagedRequestDto>
        where TId : struct
        where TCreateDto: IEntityDto<TId> 
        where TUpdateDto : IEntityDto<TId> 
        where TDeleteDto : IEntityDto<TId>
        where TGetDto : IEntityDto<TId>
        where TGetInput : IEntityDto<TId>
    {
    }
    
    public interface ICrudAppService<TId, TDto> :
        ICrudAppService<TId, TDto, TDto, TDto, TDto, TDto, IEntityDto<TId>, PagedRequestDto>
        where TId : struct where TDto: IEntityDto<TId>
    {
    }
}