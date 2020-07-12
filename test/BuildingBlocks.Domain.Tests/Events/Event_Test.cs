using System.Threading.Tasks;
using BuildingBlocks.Domain.Tests.Models;
using FluentAssertions;
using Xunit;

namespace BuildingBlocks.Domain.Tests.Events
{
    public class Event_Test : TestBase
    {
        private readonly IBus bus;

        public Event_Test() { bus = Resolve<IBus>(); }

        [Fact]
        public Task SendEvent_Test()
        {
            var people = People.Factory.Create(1, "lorem", new AddressVO("55-85"));
            var addPeopleEvent = AddedPeopleEvent.Factory.Create(people);
            bus.Awaiting(b => b.RaiseEvent(addPeopleEvent)).Should().NotThrow();

            return Task.CompletedTask;
        }
    }
}