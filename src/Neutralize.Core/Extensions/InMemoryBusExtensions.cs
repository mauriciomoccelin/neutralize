using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Neutralize.Bus;
using Neutralize.Models;

namespace Neutralize.Extensions
{
    public static class InMemoryBusExtensions
    {
        public static async Task PublishDomainEvents<T>(this INeutralizeBus mediator, T dbContext) where T : DbContext
        {
            var domainEntities = dbContext.ChangeTracker
                .Entries<AggregateRoot>()
                .Where(x => x.Entity.Events != null && x.Entity.Events.Any());

            var entityEntries = domainEntities as EntityEntry<AggregateRoot>[] ?? domainEntities.ToArray();
            var domainEvents = entityEntries
                .SelectMany(x => x.Entity.Events)
                .ToList();

            entityEntries.ToList()
                .ForEach(entity => entity.Entity.ClearEvents());

            var tasks = domainEvents
                .Select(async domainEvent => { await mediator.RaiseEvent(domainEvent); });

            await Task.WhenAll(tasks);
        }
    }
}