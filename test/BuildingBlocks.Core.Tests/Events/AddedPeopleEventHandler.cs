using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Core.Events;
using BuildingBlocks.Core.Bus;
using BuildingBlocks.Core.Tests.Models;
using MediatR;

namespace BuildingBlocks.Core.Tests.Events
{
    public class AddedPeopleEventHandler : EventHandler<AddedPeopleEvent>
    {
        public AddedPeopleEventHandler(IInMemoryBus inMemoryBus) : base(inMemoryBus)
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
            else
            {
                people.AlterName("New Name");
            }
        }
    }
}