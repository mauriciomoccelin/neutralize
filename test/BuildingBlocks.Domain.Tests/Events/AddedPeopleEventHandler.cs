using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Domain.Events;
using BuildingBlocks.Domain.Tests.Models;
using MediatR;

namespace BuildingBlocks.Domain.Tests.Events
{
    public class AddedPeopleEventHandler : EventHandler<AddedPeopleEvent>
    {
        public AddedPeopleEventHandler(IBus bus) : base(bus)
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
                await AddNotificarionError("AddedPeopleEvent", "People is empty.");
                return;
            }
            else
            {
                people.AlterName("New Name");
            }
        }
    }
}