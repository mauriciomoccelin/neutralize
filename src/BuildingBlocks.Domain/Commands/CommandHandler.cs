using MediatR;
using System;
using System.Threading.Tasks;

namespace BuildingBlocks.Domain
{
    public abstract class CommandHandler : IDisposable
    {
        private readonly IUnitOfWork uow;
        private readonly IBus bus;
        private readonly DomainNotificationHandler notifications;

        public CommandHandler(
            IBus bus,
            IUnitOfWork uow,
            INotificationHandler<DomainNotification> notifications)
        {
            this.bus = bus;
            this.uow = uow;
            this.notifications = notifications as DomainNotificationHandler;
        }

        protected async virtual Task NotifyValidationErrors(
            Command message
        )
        {
            foreach (var error in message.ValidationResult.Errors)
                await bus.RaiseEvent(DomainNotification.Create(message.MessageType, error.ErrorMessage));
        }

        public virtual async Task<bool> Commit()
        {
            if (notifications.HasNotifications()) return false;
            if (await uow.Commit()) return true;

            await bus.RaiseEvent(
                DomainNotification.Create("Commit", "We had a problem during saving your data.")
            );

            return false;
        }

        public abstract void Dispose();
    }
}
