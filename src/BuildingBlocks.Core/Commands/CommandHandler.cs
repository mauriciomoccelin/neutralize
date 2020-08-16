﻿using System;
using System.Threading.Tasks;
using BuildingBlocks.Bus;
using BuildingBlocks.Notifications;
using BuildingBlocks.UoW;
using MediatR;

namespace BuildingBlocks.Commands
{
    public abstract class CommandHandler : IDisposable
    {
        protected IUnitOfWork UnitOfWork { get; }
        protected IInMemoryBus InMemoryBus { get; }
        protected DomainNotificationHandler Notifications { get; }

        protected CommandHandler(
            IUnitOfWork unitOfWork,
            IInMemoryBus inMemoryBus,
            INotificationHandler<DomainNotification> notifications
        )
        {
            UnitOfWork = unitOfWork;
            InMemoryBus = inMemoryBus;
            Notifications = notifications as DomainNotificationHandler;
        }

        protected async Task CheckErrors<TId>(
            Command<TId> command
        ) where TId: struct
        {
            command.Validate();
            foreach (var error in command.ValidationResult.Errors)
            {
                await AddNotificationError(command.MessageType, error.ErrorMessage);
            }
        }

        protected async Task<bool> Commit()
        {
            var commitResult = await UnitOfWork.Commit();
            
            if (commitResult) return true;
            
            await AddNotificationError("Commit", "Something went wrong while saving data");
            return false;
        }

        protected Task AddNotificationError(
            string type, string message
        )
        {
            return InMemoryBus.RaiseEvent(DomainNotification.Create(type, message));
        }

        public abstract void Dispose();
    }
}
