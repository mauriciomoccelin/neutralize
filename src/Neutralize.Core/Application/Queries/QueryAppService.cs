using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Neutralize.Bus;
using Neutralize.Extensions;
using Neutralize.Models;
using Neutralize.Notifications;
using Neutralize.Repositories;

namespace Neutralize.Application.Queries
{
    public abstract class QueryAppService<TId, TEntity, TGetDto, TListDto, TPagedRequest> :
        QueryAppServiceBase<TId, TEntity, TGetDto, TListDto, TPagedRequest>,
        IQueryAppService<TId, TGetDto, TListDto, TPagedRequest>
        where TId : struct
        where TEntity : Entity<TId>
        where TGetDto : IEntityDto<TId>
        where TListDto : IEntityDto<TId>
        where TPagedRequest : PagedRequestDto
    {
        protected QueryAppService(
            IMapper mapper,
            IInMemoryBus neutralizeBus,
            IRepository<TEntity, TId> repository
        ) : base(mapper, neutralizeBus, repository)
        {
        }

        public virtual async Task<TGetDto> GetById(TId id)
        {
            var entity = await Repository.GetAsync(id);
            return ObjectMapper.Map<TGetDto>(entity);
        }
    }

    public abstract class QueryAppService<TId, TEntity, TGetDto, TListDto, TGetInput, TPagedRequest> :
        QueryAppServiceBase<TId, TEntity, TGetDto, TListDto, TPagedRequest>,
        IQueryAppService<TId, TGetDto, TListDto, TGetInput, TPagedRequest>
        where TId : struct
        where TGetInput : IEntityDto<TId>
        where TEntity : Entity<TId>
        where TGetDto : IEntityDto<TId>
        where TListDto : IEntityDto<TId>
        where TPagedRequest : PagedRequestDto
    {
        protected QueryAppService(
            IMapper mapper,
            IInMemoryBus neutralizeBus,
            IRepository<TEntity, TId> repository
        ) : base(mapper, neutralizeBus, repository)
        {
        }

        public virtual async  Task<TGetDto> GetById(TGetInput input)
        {
            var entity = await Repository.GetAsync(input.Id);
            return ObjectMapper.Map<TGetDto>(entity);
        }
    }

    public abstract class
        QueryAppServiceBase<TId, TEntity, TGetDto, TListDto, TPagedRequest> : IQueryAppServiceBase<TId, TGetDto, TListDto, TPagedRequest>
        where TId : struct
        where TEntity : Entity<TId>
        where TGetDto : IEntityDto<TId>
        where TListDto : IEntityDto<TId>
        where TPagedRequest : PagedRequestDto
    {
        protected IMapper ObjectMapper { get; }
        protected IInMemoryBus NeutralizeBus { get; }
        protected IRepository<TEntity, TId> Repository { get; }

        protected QueryAppServiceBase(
            IMapper mapper,
            IInMemoryBus neutralizeBus,
            IRepository<TEntity, TId> repository
        )
        {
            ObjectMapper = mapper;
            Repository = repository;
            NeutralizeBus = neutralizeBus;
        }

        public virtual void Dispose()
        {
            Repository.Dispose();
        }

        public virtual async Task<PagedResultDto<TListDto>> GetPagedList(TPagedRequest input)
        {
            input.Normalize();
            var query = CreateQuery(input);

            query = ApplyFilter(query, input);
            var total = await query.LongCountAsync();

            query = ApplySort(query, input);
            var items = await query.PageBy(input.Page, input.PageSize).ToListAsync();

            return new PagedResultDto<TListDto>(total, ObjectMapper.Map<IEnumerable<TListDto>>(items));
        }

        protected virtual IQueryable<TEntity> CreateQuery(TPagedRequest input)
        {
            return Repository.GetAll();
        }

        protected virtual IQueryable<TEntity> ApplyFilter(IQueryable<TEntity> query, TPagedRequest paged)
        {
            return query;
        }

        protected virtual IQueryable<TEntity> ApplySort(IQueryable<TEntity> query, TPagedRequest paged)
        {
            return query;
        }

        protected async Task AddNotificationError(
            string type, string message
        )
        {
            await NeutralizeBus.RaiseEvent(DomainNotification.Create(type, message));
        }
    }
}
