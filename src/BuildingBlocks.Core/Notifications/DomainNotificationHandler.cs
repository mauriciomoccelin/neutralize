using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace BuildingBlocks.Notifications
{
    public sealed class DomainNotificationHandler : INotificationHandler<DomainNotification>
    {
        private List<DomainNotification> notifications;

        public DomainNotificationHandler()
        {
            notifications = new List<DomainNotification>();
        }

        public Task Handle(
            DomainNotification message,
            CancellationToken cancellationToken
        )
        {
            notifications.Add(message);
            return Task.CompletedTask;
        }

        public IEnumerable<DomainNotification> GetNotifications() => notifications;
        public string[] GetNotificationsMessages() => notifications.Select(n => n.Value).ToArray();
        public bool HasNotifications() => GetNotifications().Any();
        public void Dispose() => notifications = new List<DomainNotification>();
    }
}
