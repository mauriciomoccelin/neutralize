using System;
using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Core.Commands;
using BuildingBlocks.Core.Bus;
using BuildingBlocks.Core.Notifications;
using MediatR;

namespace BuildingBlocks.Core.Tests.Commands
{
    public class AddPeopleCommandHandler : CommandHandler, IRequestHandler<AddPeopleCommand>
    {
        public AddPeopleCommandHandler(
            IInMemoryBus inMemoryBus,
            INotificationHandler<DomainNotification> notifications
        ) : base(inMemoryBus, notifications)
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