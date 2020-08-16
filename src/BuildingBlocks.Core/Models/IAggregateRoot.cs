using System.Collections.Generic;
using BuildingBlocks.Events;

namespace BuildingBlocks.Models
{
    public class AggregateRoot<TEntity, TId> : Entity<TEntity, TId> 
        where TEntity : Entity<TEntity, TId>
        where TId : struct
    {
        private readonly List<Event> events = new List<Event>();
        public IReadOnlyCollection<Event> Events => events.AsReadOnly();
        
        public void ClearEvents() => events.Clear();
        public void AddEvent(Event @event) => events.Add(@event);
        public void RemoveEvent(Event @event) => events.Remove(@event);
    }
}