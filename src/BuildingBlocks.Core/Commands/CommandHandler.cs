using System;
using System.Threading.Tasks;
using BuildingBlocks.Core.Bus;
using BuildingBlocks.Core.Notifications;
using MediatR;

namespace BuildingBlocks.Core.Commands
{
    public abstract class CommandHandler : IDisposable
    {
        protected IInMemoryBus InMemoryBus { get; }
        protected DomainNotificationHandler Notifications { get; }

        protected CommandHandler(
            IInMemoryBus inMemoryBus,
            INotificationHandler<DomainNotification> notifications)
        {
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

        protected virtual async Task AddNotificationError(
            string type, string message
        )
        {
            await InMemoryBus.RaiseEvent(DomainNotification.Create(type, message));
        }

        public abstract void Dispose();
    }
}
