using AutoMapper;
using BuildingBlocks.Core.Bus;
using BuildingBlocks.Core.Models;
using BuildingBlocks.Core.Notifications;
using BuildingBlocks.Core.Repositories;
using BuildingBlocks.Core.UoW;
using MediatR;

namespace BuildingBlocks.Core.Commands
{
    public abstract class CrudBaseCommandHandler<TEntity, TDto, TId> : CommandHandler
        where TDto: class
        where TId : struct
        where TEntity : Entity<TEntity, TId>
    {
        protected IMapper Mapper { get; }
        protected IEFCoreRepository<TEntity, TId> Repository { get; }
        
        protected CrudBaseCommandHandler(
            IMapper mapper,
            IUnitOfWork unitOfWork, 
            IInMemoryBus inMemoryBus, 
            IEFCoreRepository<TEntity, TId> repository,
            INotificationHandler<DomainNotification> notifications
        ) : base(unitOfWork, inMemoryBus, notifications)
        {
            Mapper = mapper;
            Repository = repository;
        }
    }
    
    public abstract class CreateCommand<TId> : Command<TId> where TId: struct{ }
    public abstract class UpdateCommand<TId> : Command<TId> where TId : struct { }
    public abstract class DeleteCommand<TId> : Command<TId> where TId : struct { }
    public abstract class GetResultCommand<TItem, TId> : Command<TId>, IRequest<TItem> where TItem: class where TId : struct { }
}