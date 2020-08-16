using System.Threading;
using System.Threading.Tasks;
using Neutralize.Bus;
using Neutralize.Events;
using Neutralize.Tests.Models;
using Neutralize.UoW;

namespace Neutralize.Tests.Events
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
            var people = data.People;

            if (people is null)
            {
                await AddNotificationError("AddedPeopleEvent", "People is empty.");
                return;
            }

            people.AlterName("New Name");
        }
    }
}
