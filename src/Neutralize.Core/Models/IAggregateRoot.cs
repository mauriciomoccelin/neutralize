using System;
using System.Collections.Generic;
using Neutralize.Events;

namespace Neutralize.Models
{
    public interface IAggregateRoot<out TId> : IEntity<TId> where TId: struct
    {
        Guid AggregateId { get; set; }
        IReadOnlyCollection<Event> Events { get; }
    }
}