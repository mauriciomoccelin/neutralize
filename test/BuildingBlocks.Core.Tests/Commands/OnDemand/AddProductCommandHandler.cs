using System;
using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Core.Bus;
using BuildingBlocks.Core.Commands;
using BuildingBlocks.Core.Notifications;
using BuildingBlocks.Core.UoW;
using MediatR;

namespace BuildingBlocks.Core.Tests.Commands.OnDemand
{
    public class AddProductCommandHandler : CommandHandler, IRequestHandler<AddProductCommand, string>
    {
        public AddProductCommandHandler(
            IUnitOfWork unitOfWork,
            IInMemoryBus inMemoryBus,
            INotificationHandler<DomainNotification> notifications
        ) : base(unitOfWork, inMemoryBus, notifications)
        {
        }

        public override void Dispose() { GC.SuppressFinalize(this); }

        public async Task<string> Handle(
            AddProductCommand request,
            CancellationToken cancellationToken
        )
        {
            await CheckErrors(request);
            return request.IsValid() ? "Ok" : default;
        }
    }
}