using Neutralize.Events;
using Neutralize.Tests.Models;

namespace Neutralize.Tests.Events
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
