using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Neutralize.Commands;
using Neutralize.Identity;
using Neutralize.Json;
using Neutralize.Notifications;
using Neutralize.Repositories;
using Newtonsoft.Json;

namespace Neutralize.Models.Histories.Managers
{
    public sealed class HistoryManager : IHistoryManager
    {
        private readonly IAspNetUser aspNetUser;
        private readonly IRepository<History, Guid> repository;
        private readonly DomainNotificationHandler notifications;

        public HistoryManager(
            IAspNetUser aspNetUser,
            IRepository<History, Guid> repository,
            INotificationHandler<DomainNotification> notifications
        )
        {
            this.aspNetUser = aspNetUser;
            this.repository = repository;
            this.notifications = notifications as DomainNotificationHandler;
        }

        public void Dispose()
        {
            repository?.Dispose();
        }

        public async Task Register<TId>(Command<TId> command) where TId : struct
        {
            var error = command.ValidationResult.Errors.Select(x => x.ErrorMessage);

            if (!error.Any()) return;

            var history = History.Factory.Criar(
                JsonConvert.SerializeObject(error, CustomJsonSerializerSettings.Create()),
                aspNetUser.GetUserId(),
                command.MessageType,
                command.Timestamp,
                string.Empty
            );

            await Save(history);
        }

        public async Task Register<TId>(Command<TId> command, IEntity<TId> entity) where TId : struct
        {
            if (notifications.HasNotifications()) return;

            var history = History.Factory.Criar(
                JsonConvert.SerializeObject(entity, CustomJsonSerializerSettings.Create()),
                aspNetUser.GetUserId(),
                command.MessageType,
                command.Timestamp,
                entity.GetType().FullName
            );

            await Save(history);
        }

        public async Task Register<TId>(Command<TId> command, IAggregateRoot<TId> entity) where TId : struct
        {
            if (notifications.HasNotifications()) return;

            var history = History.Factory.Criar(
                JsonConvert.SerializeObject(entity, CustomJsonSerializerSettings.Create()),
                aspNetUser.GetUserId(),
                command.MessageType,
                command.Timestamp,
                entity.GetType().FullName,
                entity.AggregateId
            );

            await Save(history);
        }

        public async Task Register<TId>(Command<TId> command, IEntity<TId> entity, Guid aggregateId) where TId : struct
        {
            if (notifications.HasNotifications()) return;

            var history = History.Factory.Criar(
                JsonConvert.SerializeObject(entity, CustomJsonSerializerSettings.Create()),
                aspNetUser.GetUserId(),
                command.MessageType,
                command.Timestamp,
                entity.GetType().FullName,
                aggregateId
            );

            await Save(history);
        }

        private async Task Save(History history)
        {
            await repository.AddAsync(history);
            await repository.Commit();
        }
    }
}
