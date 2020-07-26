using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Core.Bus;
using BuildingBlocks.Core.Notifications;
using BuildingBlocks.Core.UoW;
using MediatR;

namespace BuildingBlocks.Core.Events
{
    public abstract class EventHandler<TEvent> : INotificationHandler<TEvent> where TEvent : Event
    {
        protected IUnitOfWork UnitOfWork { get; }
        protected IInMemoryBus InMemoryBus { get; }

        protected EventHandler(IUnitOfWork unitOfWork, IInMemoryBus inMemoryBus)
        {
            UnitOfWork = unitOfWork;
            InMemoryBus = inMemoryBus;
        }

        protected virtual async Task AddNotificationError(
            string type, string message
        )
        {
            await InMemoryBus.RaiseEvent(DomainNotification.Create(type, message));
        }

        public abstract Task Handle(TEvent data, CancellationToken cancellationToken);
    }
}