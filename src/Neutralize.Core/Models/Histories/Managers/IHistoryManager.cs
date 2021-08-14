using System;
using System.Threading.Tasks;
using Neutralize.Commands;

namespace Neutralize.Models.Histories.Managers
{
    public interface IHistoryManager : IDisposable
    {
        Task Register<TId>(Command<TId> command) where TId : struct;
        Task Register<TId>(Command<TId> command, IEntity<TId> entity) where TId : struct;
        Task Register<TId>(Command<TId> command, IAggregateRoot<TId> entity) where TId : struct;
        Task Register<TId>(Command<TId> command, IEntity<TId> entity, Guid aggregateId) where TId : struct;
    }
}
