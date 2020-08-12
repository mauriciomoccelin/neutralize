using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BuildingBlocks.Core.Application;
using BuildingBlocks.Core.Bus;
using BuildingBlocks.Core.Models;
using BuildingBlocks.Core.Notifications;
using BuildingBlocks.Core.Repositories;
using BuildingBlocks.Core.UoW;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Core.Commands
{
    public abstract class CrudCommandHandler<TEntity, TDto, TCreateCommand, TUpdateCommand, TDeleteCommand, TGetResultCommand, TGetPageResultCommand> :
        WriteOnlyCommandHandler<TEntity, TDto, TCreateCommand, TUpdateCommand, TDeleteCommand>
        where TDto: class
        where TEntity : Entity<TEntity, long>
        where TCreateCommand: CreateCommand
        where TUpdateCommand: UpdateCommand
        where TDeleteCommand: DeleteCommand
        where TGetResultCommand: GetResultCommand<TDto>
        where TGetPageResultCommand: GetPageResultCommand<TDto>
    {
        protected CrudCommandHandler(
            IMapper mapper,
            IUnitOfWork unitOfWork, 
            IInMemoryBus inMemoryBus,
            IEFCoreRepository<TEntity, long> repository,
            INotificationHandler<DomainNotification> notifications
        ) : base(mapper, unitOfWork, inMemoryBus,repository, notifications)
        {
        }

        public virtual async Task<TDto> Handle(
            TGetResultCommand request, CancellationToken cancellationToken
        )
        {
            var entity = await Repository.GetAsync(request.Id);
            return Mapper.Map<TDto>(entity);
        }

        public virtual async Task<PagedResultDto<TDto>> Handle(
            TGetPageResultCommand request, CancellationToken cancellationToken
        )
        {
            var query = Repository.GetAll();
            
            query = QueryFilter(query, request);
            
            var totalCount = await query.LongCountAsync(cancellationToken);
            
            query = QuerySort(query, request);
            query = QueryPage(query, request);

            var entities = await query.ToListAsync(cancellationToken);
            var items = Mapper.Map<IEnumerable<TDto>>(entities); 
            
            return new PagedResultDto<TDto>(totalCount, items);
        }
        
        protected virtual IQueryable<TEntity> QuerySort(
            IQueryable<TEntity> query, GetPageResultCommand<TDto> request
        )
        {
            return query;
        }
        
        protected virtual IQueryable<TEntity> QueryFilter(
            IQueryable<TEntity> query, GetPageResultCommand<TDto> request
        )
        {
            return query;
        }

        protected virtual IQueryable<TEntity> QueryPage(
            IQueryable<TEntity> query, TGetPageResultCommand request
        )
        {
            return query.Skip(request.Page).Take(request.PageSize);
        }
        
        protected virtual TDto MapToDto(Command request)
        {
            return Mapper.Map<TDto>(request);
        }
    }
}