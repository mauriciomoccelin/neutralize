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
        protected IUnitOfWork UnitOfWork { get; }
        protected IInMemoryBus InMemoryBus { get; }
        protected DomainNotificationHandler Notifications { get; }

        protected CommandHandler(
            IUnitOfWork unitOfWork,
            IInMemoryBus inMemoryBus,
            INotificationHandler<DomainNotification> notifications)
        {
            UnitOfWork = unitOfWork;
            InMemoryBus = inMemoryBus;
            Notifications = notifications as DomainNotificationHandler;
        }

        protected virtual async Task CheckErrors(
            Command command
        )
        {
            command.Validate();

            foreach (var error in command.ValidationResult.Errors)
            {
                await AddNotificationError(command.MessageType, error.ErrorMessage);
            }
        }
        
        public virtual async Task<bool> Commit()
        {
            if (Notifications.HasNotifications()) return false;
            
            var commit = await UnitOfWork.Commit();
            
            if (commit is false)
            {
                await AddNotificationError(
                    "Commit", "We had a problem during saving your data."
                );
            }

            return true;
        }
        
        protected virtual async Task AddNotificationError(
            string type, string message
        )
        {
            await InMemoryBus.RaiseEvent(DomainNotification.Create(type, message));
        }

        public abstract void Dispose();
    }
}
