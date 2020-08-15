using BuildingBlocks.Core.Events;
using BuildingBlocks.Core.Tests.Models;

namespace BuildingBlocks.Core.Tests.Events
{
    public class AddedPeopleEvent : Event
    {
        public People People { get; }

        private AddedPeopleEvent(People people) { People = people; }

        public static class Factory
        {
            public static AddedPeopleEvent Create(People people)
            {
                return new AddedPeopleEvent(people);
            }
        }
    }
}