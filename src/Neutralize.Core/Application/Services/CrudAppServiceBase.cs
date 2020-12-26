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
    public abstract class CrudAppServiceBase<TEntity, TCreateDto, TUpdateDto, TDeleteDto, TGetDto, TListDto, TGetInput, TListInput>
        : ApplicationService, ICrudAppService<long, TCreateDto, TUpdateDto, TDeleteDto, TGetDto, TListDto, TGetInput, TListInput>
        where TEntity : IEntity
        where TCreateDto : IEntityDto<long>
        where TUpdateDto : IEntityDto<long>
        where TDeleteDto : IEntityDto<long>
        where TGetDto : IEntityDto<long>
        where TListDto : IEntityDto<long>
        where TGetInput : IEntityDto<long>
        where TListInput : PagedRequestDto
    {
        protected IRepository<TEntity, long> Repository { get; }

        protected CrudAppServiceBase(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            INeutralizeBus neutralizeBus,
            IRepository<TEntity, long> repository
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

        protected virtual TEntity MapToEntity<TDto>(TDto dto) where TDto : IEntityDto<long>
        {
            return Mapper.Map<TEntity>(dto);
        }

        protected virtual TEntity MapToEntity<TDto>(TDto dto, TEntity entity) where TDto : IEntityDto<long>
        {
            return Mapper.Map(dto, entity);
        }

        public virtual async Task<long> Create(TCreateDto input)
        {
            var entity = MapToEntity(input);

            await Repository.AddAsync(entity);
            await UnitOfWork.Commit();

            return entity.Id;
        }

        public virtual async Task<long> Update(TUpdateDto input)
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