using System;
using System.Threading.Tasks;
using AutoMapper;
using Neutralize.Bus;
using Neutralize.Notifications;
using Neutralize.UoW;
using MediatR;

namespace Neutralize.Commands
{
    public abstract class CommandHandler : IDisposable
    {
        protected IMapper Mapper { get; }
        protected IUnitOfWork UnitOfWork { get; }
        protected INeutralizeBus NeutralizeBus { get; }
        protected DomainNotificationHandler Notifications { get; }

        protected CommandHandler(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            INeutralizeBus neutralizeBus,
            INotificationHandler<DomainNotification> notifications
        )
        {
            Mapper = mapper;
            UnitOfWork = unitOfWork;
            NeutralizeBus = neutralizeBus;
            Notifications = notifications as DomainNotificationHandler;
        }

        protected async Task<bool> CheckErrors<TId>(
            Command<TId> command
        ) where TId: struct
        {
            command.Validate();

            if (command.IsValid()) command.Normalize();

            foreach (var error in command.GetErrors())
            {
                await AddNotificationError(command.MessageType, error.ErrorMessage);
            }

            return command.IsValid();
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
            return NeutralizeBus.RaiseEvent(DomainNotification.Create(type, message));
        }

        public abstract void Dispose();
    }
}

