using System;
using System.Threading.Tasks;
using BuildingBlocks.Core.Bus;
using BuildingBlocks.Core.Notifications;
using BuildingBlocks.Core.UoW;
using MediatR;

namespace BuildingBlocks.Core.Commands
{
    public abstract class CommandHandler : IDisposable
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IInMemoryBus inMemoryBus;
        private readonly DomainNotificationHandler notifications;

        protected CommandHandler(
            IUnitOfWork unitOfWork,
            IInMemoryBus inMemoryBus,
            INotificationHandler<DomainNotification> notifications)
        {
            this.unitOfWork = unitOfWork;
            this.inMemoryBus = inMemoryBus;
            this.notifications = notifications as DomainNotificationHandler;
        }

        protected virtual async Task CheckErrors(
            Command command
        )
        {
            command.Validate();

            foreach (var error in command.ValidationResult.Errors)
            {
                await inMemoryBus.RaiseEvent(
                    DomainNotification.Create(command.MessageType, error.ErrorMessage)
                );
            }
        }

        public virtual async Task<bool> Commit()
        {
            if (notifications.HasNotifications()) return false;
            
            var commit = await unitOfWork.Commit();
            
            if (commit is false)
            {
                await inMemoryBus.RaiseEvent(
                    DomainNotification.Create("Commit", "We had a problem during saving your data.")
                );
            }

            return true;
        }

        public abstract void Dispose();
    }
}
