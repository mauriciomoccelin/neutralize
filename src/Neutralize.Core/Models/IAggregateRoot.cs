using System;
using System.Collections.Generic;
using Neutralize.Events;

namespace Neutralize.Models
{
    public interface IAggregateRoot : IEntity<Guid>
    {
        IReadOnlyCollection<Event> Events { get; }
    }
}