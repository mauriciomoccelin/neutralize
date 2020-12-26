using System.Threading;
using System.Threading.Tasks;
using Neutralize.Bus;
using Neutralize.Notifications;
using Neutralize.UoW;
using MediatR;

namespace Neutralize.Events
{
    public abstract class EventHandler<TEvent> : INotificationHandler<TEvent> where TEvent : Event
    {
        protected IUnitOfWork UnitOfWork { get; }
        protected INeutralizeBus NeutralizeBus { get; }

        protected EventHandler(IUnitOfWork unitOfWork, INeutralizeBus neutralizeBus)
        {
            UnitOfWork = unitOfWork;
            NeutralizeBus = neutralizeBus;
        }

        protected async Task AddNotificationError(
            string type, string message
        )
        {
            await NeutralizeBus.RaiseEvent(DomainNotification.Create(type, message));
        }

        protected async Task<bool> Commit()
        {
            var commitResult = await UnitOfWork.Commit();
            
            if (commitResult) return true;
            
            await AddNotificationError("Commit", "Something went wrong while saving data");
            return false;
        }

        public abstract Task Handle(TEvent data, CancellationToken cancellationToken);
    }
}
