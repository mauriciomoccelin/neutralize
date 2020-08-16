using BuildingBlocks.Events;
using BuildingBlocks.Tests.Models;

namespace BuildingBlocks.Tests.Events
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