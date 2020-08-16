using System;
using System.Threading.Tasks;
using Neutralize.Bus;
using Neutralize.Tests.Models;
using FluentAssertions;
using Xunit;

namespace Neutralize.Tests.Events
{
    public class Event_Test : NeutralizeCoreBaseTest
    {
        private readonly IInMemoryBus _inMemoryBus;

        public Event_Test() { _inMemoryBus = Resolve<IInMemoryBus>(); }

        [Fact]
        public Task SendEvent_Test()
        {
            var people = People.Factory.Create(1, "lorem", new AddressVO("55-85"));
            var addPeopleEvent = AddedPeopleEvent.Factory.Create(people);
            _inMemoryBus.Awaiting(b => b.RaiseEvent(addPeopleEvent)).Should().NotThrow();

            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
