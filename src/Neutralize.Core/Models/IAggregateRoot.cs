using System.Collections.Generic;
using Neutralize.Events;

namespace Neutralize.Models
{
    public interface IAggregateRoot : IEntity
    {
        IReadOnlyCollection<Event> Events { get; }
    }
}