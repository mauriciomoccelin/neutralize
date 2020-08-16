using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Bus;
using BuildingBlocks.Events;
using BuildingBlocks.Tests.Models;
using BuildingBlocks.UoW;

namespace BuildingBlocks.Tests.Events
{
    public class AddedPeopleEventHandler : EventHandler<AddedPeopleEvent>
    {
        public AddedPeopleEventHandler(
            IUnitOfWork unitOfWork,
            IInMemoryBus inMemoryBus
        ) : base(unitOfWork, inMemoryBus)
        {
        }

        public override async Task Handle(
            AddedPeopleEvent data,
            CancellationToken cancellationToken
        )
        {
            var people = data.People ?? People.Empty();

            if (people.IsEmpty())
            {
                await AddNotificationError("AddedPeopleEvent", "People is empty.");
                return;
            }

            people.AlterName("New Name");
        }
    }
}