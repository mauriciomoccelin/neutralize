using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Optional;

namespace BuildingBlocks.Domain.Tests.Commands
{
    public class AddProductCommandHandler : CommandHandler, IRequestHandler<AddProductCommand, Option<string>>
    {
        public AddProductCommandHandler(
            IBus bus,
            IUnitOfWork uow,
            INotificationHandler<DomainNotification> notifications
        ) : base(bus, uow, notifications)
        {
        }

        public override void Dispose() { GC.SuppressFinalize(this); }

        public async Task<Option<string>> Handle(
            AddProductCommand request,
            CancellationToken cancellationToken
        )
        {
            if (!request.IsValid())
            {
                await NotifyValidationErrors(request);
                return Option.None<string>();
            }

            return Option.Some("Ok");
        }
    }
}