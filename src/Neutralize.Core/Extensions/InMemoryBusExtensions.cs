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
        /// <summary>
        /// Publish all events by adding to an aggregation root on commit and has change tracker
        /// </summary>
        /// <param name="mediator"></param>
        /// <param name="dbContext"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TId"></typeparam>
        /// <returns></returns>
        public static async Task PublishDomainEvents<T, TId>(
            this INeutralizeBus mediator, T dbContext
        ) where T : DbContext where TId: struct
        {
            var domainEntities = dbContext.ChangeTracker
                .Entries<AggregateRoot<TId>>()
                .Where(x => x.Entity.Events != null && x.Entity.Events.Any());

            var entityEntries = domainEntities as EntityEntry<AggregateRoot<TId>>[]
                                ?? domainEntities.ToArray();

            var domainEvents = entityEntries
                .SelectMany(x => x.Entity.Events)
                .ToList();

            entityEntries.ToList()
                .ForEach(entity => entity.Entity.ClearEvents());

            var tasks = domainEvents.Select(
                async domainEvent => await mediator.RaiseEvent(domainEvent)
            );

            await Task.WhenAll(tasks);
        }
    }
}