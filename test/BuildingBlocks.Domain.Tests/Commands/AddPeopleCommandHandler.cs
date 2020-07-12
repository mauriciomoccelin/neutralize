using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace BuildingBlocks.Domain.Tests.Commands
{
    public class AddPeopleCommandHandler : CommandHandler, IRequestHandler<AddPeopleCommand>
    {
        public AddPeopleCommandHandler(
            IBus bus,
            IUnitOfWork uow,
            INotificationHandler<DomainNotification> notifications
        ) : base(bus, uow, notifications)
        {
        }

        public override void Dispose() { GC.SuppressFinalize(this); }

        public async Task<Unit> Handle(
            AddPeopleCommand request,
            CancellationToken cancellationToken
        )
        {
            if (request.IsValid())
            {
                await NotifyValidationErrors(request);
                return await Unit.Task;
            }

            return await Unit.Task;
        }
    }
}