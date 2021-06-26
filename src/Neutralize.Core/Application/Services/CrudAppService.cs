using AutoMapper;
using Neutralize.Bus;
using Neutralize.Models;
using Neutralize.Repositories;
using Neutralize.UoW;

namespace Neutralize.Application.Services
{
    public abstract class CrudAppService<TId, TEntity, TCreateDto, TUpdateDto, TDeleteDto, TGetDto, TListDto, TGetInput, TListInput>
        : CrudAppServiceBase<TId, TEntity, TCreateDto, TUpdateDto, TDeleteDto, TGetDto, TListDto, TGetInput, TListInput>
        where TId: struct
        where TEntity : IEntity<TId>
        where TCreateDto : IEntityDto<TId>
        where TUpdateDto : IEntityDto<TId>
        where TDeleteDto : IEntityDto<TId>
        where TGetDto : IEntityDto<TId>
        where TListDto : IEntityDto<TId>
        where TGetInput : IEntityDto<TId>
        where TListInput : PagedRequestDto
    {
        protected CrudAppService(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            INeutralizeBus neutralizeBus,
            IRepository<TEntity, TId> repository
        ) : base(mapper, unitOfWork, neutralizeBus, repository)
        {
        }
    }

    public abstract class CrudAppService<TId, TEntity, TCreateDto, TUpdateDto, TDeleteDto, TGetDto, TGetInput> :
        CrudAppServiceBase<TId, TEntity, TCreateDto, TUpdateDto, TDeleteDto, TGetDto, TGetDto, TGetInput, PagedRequestDto>
        where TId: struct
        where TEntity : IEntity<TId>
        where TCreateDto : IEntityDto<TId>
        where TUpdateDto : IEntityDto<TId>
        where TDeleteDto : IEntityDto<TId>
        where TGetDto : IEntityDto<TId>
        where TGetInput : IEntityDto<TId>
    {
        protected CrudAppService(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            INeutralizeBus neutralizeBus,
            IRepository<TEntity, TId> repository
        ) : base(mapper, unitOfWork, neutralizeBus, repository)
        {
        }
    }

    public abstract class CrudAppService<TId, TEntity, TDto> :
        CrudAppServiceBase<TId, TEntity, TDto, TDto, TDto, TDto, TDto, IEntityDto<TId>, PagedRequestDto>
        where TId: struct
        where TEntity : IEntity<TId>
        where TDto : IEntityDto<TId>
    {
        protected CrudAppService(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            INeutralizeBus neutralizeBus,
            IRepository<TEntity, TId> repository
        ) : base(mapper, unitOfWork, neutralizeBus, repository)
        {
        }
    }
}