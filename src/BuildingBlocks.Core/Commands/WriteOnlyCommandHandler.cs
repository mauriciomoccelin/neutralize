using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using BuildingBlocks.Core.Bus;
using BuildingBlocks.Core.Models;
using BuildingBlocks.Core.Notifications;
using BuildingBlocks.Core.Repositories;
using BuildingBlocks.Core.UoW;
using MediatR;

namespace BuildingBlocks.Core.Commands
{
    public abstract class WriteOnlyCommandHandler<TEntity, TDto, TId, TCreateCommand, TUpdateCommand, TDeleteCommand> :
        CrudBaseCommandHandler<TEntity, TDto, TId>
        where TDto : class
        where TId : struct
        where TEntity : Entity<TEntity, TId>
        where TCreateCommand : CreateCommand<TId>
        where TUpdateCommand : UpdateCommand<TId>
        where TDeleteCommand : DeleteCommand<TId>
    {
        protected WriteOnlyCommandHandler(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IInMemoryBus inMemoryBus,
            IEFCoreRepository<TEntity, TId> repository,
            INotificationHandler<DomainNotification> notifications
        ) : base(mapper, unitOfWork, inMemoryBus, repository, notifications)
        {
        }

        public virtual async Task<Unit> Handle(TCreateCommand request, CancellationToken cancellationToken)
        {
            await CheckErrors(request);
            if (!request.IsValid()) return await Unit.Task;

            var entity = MapToEntity(request);

            await Repository.AddAsync(entity);
            await Commit();

            return await Unit.Task;
        }

        public virtual async Task<Unit> Handle(TUpdateCommand request, CancellationToken cancellationToken)
        {
            await CheckErrors(request);
            if (!request.IsValid()) return await Unit.Task;

            var existingEntity = await Repository.GetAsync(request.Id);
            var entity = MapToEntity(request, existingEntity);

            await Repository.UpdateAsync(entity);
            await Commit();

            return await Unit.Task;
        }

        public virtual async Task<Unit> Handle(TDeleteCommand request, CancellationToken cancellationToken)
        {
            await CheckErrors(request);
            if (!request.IsValid()) return await Unit.Task;

            var entity = await Repository.GetAsync(request.Id);

            await Repository.RemoveAsync(entity);
            await Commit();

            return await Unit.Task;
        }

        protected virtual TEntity MapToEntity(TCreateCommand request)
        {
            return Mapper.Map<TEntity>(request);
        }

        protected virtual TEntity MapToEntity(TUpdateCommand request, TEntity entity)
        {
            return Mapper.Map(request, entity);
        }
    }
}