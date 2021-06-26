using System;
using System.Collections.Generic;
using Neutralize.Events;

namespace Neutralize.Models
{
    public class AggregateRoot : Entity<Guid>, IAggregateRoot
    {
        protected AggregateRoot() { }

        protected AggregateRoot(Guid aggregateId)
        {
            AggregateId = SetAggregateId(aggregateId);
        }
        public Guid AggregateId { get; private set; }
        
        private readonly List<Event> events = new List<Event>();
        public IReadOnlyCollection<Event> Events => events.AsReadOnly();
        public void ClearEvents() => events.Clear();
        public void AddEvent(Event @event) => events.Add(@event);
        public void RemoveEvent(Event @event) => events.Remove(@event);
        
        public Guid SetAggregateId(Guid value)
        {
            if (Guid.Empty.Equals(value))
            {
                throw new ArgumentException($"AggregateId not be {Guid.Empty}");
            }

            AggregateId = value;
            return AggregateId;
        }
    }
}
