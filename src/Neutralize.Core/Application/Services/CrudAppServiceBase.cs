using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Neutralize.Bus;
using Neutralize.Models;
using Neutralize.Repositories;
using Neutralize.UoW;

namespace Neutralize.Application.Services
{
    public abstract class CrudAppServiceBase<TId, TEntity, TCreateDto, TUpdateDto, TDeleteDto, TGetDto, TListDto, TGetInput, TListInput>
        : ApplicationService, ICrudAppService<TId, TCreateDto, TUpdateDto, TDeleteDto, TGetDto, TListDto, TGetInput, TListInput>
        where TId : struct
        where TEntity : IEntity<TId>
        where TCreateDto : IEntityDto<TId>
        where TUpdateDto : IEntityDto<TId>
        where TDeleteDto : IEntityDto<TId>
        where TGetDto : IEntityDto<TId>
        where TListDto : IEntityDto<TId>
        where TGetInput : IEntityDto<TId>
        where TListInput : PagedRequestDto
    {
        protected IRepository<TEntity, TId> Repository { get; }

        protected CrudAppServiceBase(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            INeutralizeBus neutralizeBus,
            IRepository<TEntity, TId> repository
        ) : base(mapper, unitOfWork, neutralizeBus)
        {
            Repository = repository;
        }

        protected virtual IQueryable<TEntity> CreateFilteredQuery()
        {
            return Repository.GetAll();
        }

        protected virtual IQueryable<TEntity> ApplySort(IQueryable<TEntity> query)
        {
            return query;
        }

        protected virtual TGetDto MapToDto(TEntity entity)
        {
            return Mapper.Map<TGetDto>(entity);
        }

        protected virtual TEntity MapToEntity<TDto>(TDto dto) where TDto : IEntityDto<TId>
        {
            return Mapper.Map<TEntity>(dto);
        }

        protected virtual TEntity MapToEntity<TDto>(TDto dto, TEntity entity) where TDto : IEntityDto<TId>
        {
            return Mapper.Map(dto, entity);
        }

        public virtual async Task<TId> Create(TCreateDto input)
        {
            var entity = MapToEntity(input);

            await Repository.AddAsync(entity);
            await UnitOfWork.Commit();

            return entity.Id;
        }

        public virtual async Task<TId> Update(TUpdateDto input)
        {
            var entity = await Repository.GetAsync(input.Id);
            entity = MapToEntity(input, entity);

            await Repository.UpdateAsync(entity);
            await UnitOfWork.Commit();

            return entity.Id;
        }

        public virtual async Task Delete(TDeleteDto input)
        {
            var entity = await Repository.GetAsync(input.Id);

            await Repository.RemoveAsync(entity);
            await UnitOfWork.Commit();
        }

        public virtual async Task<TGetDto> Get(TGetInput input)
        {
            var entity = await Repository.GetAsync(input.Id);
            return MapToDto(entity);
        }

        public virtual async Task<PagedResultDto<TListDto>> GetList(TListInput input)
        {
            var query = CreateFilteredQuery();
            var total = await query.LongCountAsync();

            query = ApplySort(query);
            var items = await query.ToListAsync();

            return new PagedResultDto<TListDto>(total, Mapper.Map<IEnumerable<TListDto>>(items));
        }

        public override void Dispose()
        {
            UnitOfWork?.Dispose();
            Repository?.Dispose();
        }
    }
}