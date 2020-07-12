using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace BuildingBlocks.Domain.Events
{
    public abstract class EventHandler<TEvent> : INotificationHandler<TEvent> where TEvent : Event
    {
        protected readonly IBus bus;

        protected EventHandler(IBus bus)
        {
            this.bus = bus;
        }

        protected async virtual Task AddNotificarionError(
            string type, string message
        )
        {
            await bus.RaiseEvent(DomainNotification.Create(type, message));
        }

        public abstract Task Handle(TEvent data, CancellationToken cancellationToken);
    }
}