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
