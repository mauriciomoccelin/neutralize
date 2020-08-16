using System;
using System.Threading;
using System.Threading.Tasks;
using Neutralize.Bus;
using Neutralize.Commands;
using Neutralize.Notifications;
using Neutralize.UoW;
using MediatR;

namespace Neutralize.Tests.Commands
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
