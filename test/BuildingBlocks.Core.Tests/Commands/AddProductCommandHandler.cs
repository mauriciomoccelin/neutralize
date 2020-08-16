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