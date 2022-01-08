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
            return NeutralizeBus.RaiseEvent(DomainNotification.Create(type, message));
        }

        public abstract void Dispose();
    }
}

