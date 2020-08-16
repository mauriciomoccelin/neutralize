using System;
using System.Threading;
using System.Threading.Tasks;
using Neutralize.Bus;
using Neutralize.Commands;
using Neutralize.Notifications;
using Neutralize.UoW;
using MediatR;
using Neutralize.Repositories;
using Neutralize.Tests.Events;
using Neutralize.Tests.Models;

namespace Neutralize.Tests.Commands
{
    public class AddProductCommandHandler : CommandHandler, IRequestHandler<AddProductCommand, string>
    {
        private readonly IRepository<People, long> repository;
        public AddProductCommandHandler(
            IUnitOfWork unitOfWork,
            IInMemoryBus inMemoryBus,
            IRepository<People, long> repository,
            INotificationHandler<DomainNotification> notifications
        ) : base(unitOfWork, inMemoryBus, notifications)
        {
            this.repository = repository;
        }

        public override void Dispose() { GC.SuppressFinalize(this); }

        public async Task<string> Handle(
            AddProductCommand request,
            CancellationToken cancellationToken
        )
        {
            await CheckErrors(request);
            if (!request.IsValid()) return string.Empty;
            
            var people = People.Factory.Create(0, "lorem ipsum", new AddressVO("999-9999"));
            
            people.AddEvent(
                AddedPeopleEvent.Factory.Create(
                    People.Factory.Create(0, "lorem ipsum", new AddressVO("999-9999"))
                )
            );

            await repository.AddAsync(people);
            await Commit();
            return "Ok";
        }
    }
}
