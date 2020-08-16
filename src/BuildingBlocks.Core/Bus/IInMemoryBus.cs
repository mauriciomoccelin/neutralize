using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BuildingBlocks.Commands;
using BuildingBlocks.Events;

namespace BuildingBlocks.Bus
{
    public interface IInMemoryBus
    {
        Task SendCommand<TCommand, TId>(
            TCommand command
        ) where TId : struct where TCommand : Command<TId>;

        Task SendCommand<TCommand, TId>(
            IEnumerable<TCommand> command
        ) where TId : struct where TCommand : Command<TId>;

        Task<TResponse> SendCommand<TCommand, TId, TResponse>(
            TCommand command
        ) where TId : struct where TCommand : Command<TId>;

        Task<TResponse> SendCommandGuidId<TCommand, TResponse>(
            TCommand command
        ) where TCommand : Command<Guid>;

        Task<TResponse> SendCommandInt64Id<TCommand, TResponse>(
            TCommand command
        ) where TCommand : Command<long>;

        Task RaiseEvent<TEvent>(
            TEvent @event
        ) where TEvent : Event;
    }
}