using AutoMapper;
using Neutralize.Models;
using Neutralize.Repositories;
using Neutralize.UoW;

namespace Neutralize.Application.Services
{
    public abstract class CrudAppService<TEntity, TCreateDto, TUpdateDto, TDeleteDto, TGetDto, TListDto, TGetInput, TListInput>
        : CrudAppServiceBase<TEntity, TCreateDto, TUpdateDto, TDeleteDto, TGetDto, TListDto, TGetInput, TListInput>
        where TEntity: Entity
        where TCreateDto : IEntityDto<long>
        where TUpdateDto : IEntityDto<long>
        where TDeleteDto : IEntityDto<long>
        where TGetDto : IEntityDto<long>
        where TListDto : IEntityDto<long>
        where TGetInput : IEntityDto<long>
        where TListInput : PagedRequestDto
    {
        protected CrudAppService(IMapper mapper, IUnitOfWork unitOfWork, IRepository<TEntity, long> repository) 
            : base(mapper, unitOfWork, repository)
        {
        }
    }
    
    public abstract class CrudAppService<TEntity, TId, TCreateDto, TUpdateDto, TDeleteDto, TGetDto, TGetInput> : 
        CrudAppServiceBase<TEntity, TCreateDto, TUpdateDto, TDeleteDto, TGetDto, TGetDto, TGetInput, PagedRequestDto>
        where TEntity : Entity
        where TCreateDto: IEntityDto<long>
        where TUpdateDto : IEntityDto<long> 
        where TDeleteDto : IEntityDto<long>
        where TGetDto : IEntityDto<long>
        where TGetInput : IEntityDto<long>
    {
        protected CrudAppService(IMapper mapper, IUnitOfWork unitOfWork, IRepository<TEntity, long> repository) 
            : base(mapper, unitOfWork, repository)
        {
        }
    }
    
    public abstract class CrudAppService<TEntity, TDto> : 
        CrudAppServiceBase<TEntity, TDto, TDto, TDto, TDto, TDto, IEntityDto<long>, PagedRequestDto>
        where TEntity : Entity
        where TDto: IEntityDto<long>
    {
        protected CrudAppService(IMapper mapper, IUnitOfWork unitOfWork, IRepository<TEntity, long> repository) 
            : base(mapper, unitOfWork, repository)
        {
        }
    }
}