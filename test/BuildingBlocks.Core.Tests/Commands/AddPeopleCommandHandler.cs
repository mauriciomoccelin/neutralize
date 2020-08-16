using System;
using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Bus;
using BuildingBlocks.Commands;
using BuildingBlocks.Notifications;
using BuildingBlocks.UoW;
using MediatR;

namespace BuildingBlocks.Tests.Commands
{
    public class AddPeopleCommandHandler : CommandHandler, IRequestHandler<AddPeopleCommand>
    {
        public AddPeopleCommandHandler(
            IUnitOfWork unitOfWork,
            IInMemoryBus inMemoryBus,
            INotificationHandler<DomainNotification> notifications
        ) : base(unitOfWork, inMemoryBus, notifications)
        {
        }

        public override void Dispose() { GC.SuppressFinalize(this); }

        public async Task<Unit> Handle(
            AddPeopleCommand request,
            CancellationToken cancellationToken
        )
        {
            request.Validate();
            await CheckErrors(request);

            return await Unit.Task;
        }
    }
}