using System;
using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Core.Commands;
using BuildingBlocks.Core.Bus;
using BuildingBlocks.Core.Notifications;
using BuildingBlocks.Core.UoW;
using MediatR;
using Optional;

namespace BuildingBlocks.Core.Tests.Commands
{
    public class AddProductCommandHandler : CommandHandler, IRequestHandler<AddProductCommand, Option<string>>
    {
        public AddProductCommandHandler(
            IUnitOfWork unitOfWork,
            IInMemoryBus inMemoryBus,
            INotificationHandler<DomainNotification> notifications
        ) : base(unitOfWork, inMemoryBus, notifications)
        {
        }

        public override void Dispose() { GC.SuppressFinalize(this); }

        public async Task<Option<string>> Handle(
            AddProductCommand request,
            CancellationToken cancellationToken
        )
        {
            await CheckErrors(request);
            return request.IsValid() ? Option.Some("Ok") : Option.None<string>();
        }
    }
}