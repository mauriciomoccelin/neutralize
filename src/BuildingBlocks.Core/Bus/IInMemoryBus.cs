using System.Collections.Generic;
using System.Threading.Tasks;
using BuildingBlocks.Core.Commands;
using BuildingBlocks.Core.Events;

namespace BuildingBlocks.Core.Bus
{
    public interface IInMemoryBus
    {
        Task SendCommand<TCommand>(
            TCommand command
        ) where TCommand : Command;

        Task SendCommand<TCommand>(
            IEnumerable<TCommand> command
        ) where TCommand : Command;

        Task<TResponse> SendCommand<TCommand, TResponse>(
            TCommand command
        ) where TCommand : Command where TResponse : class;

        Task RaiseEvent<TEvent>(
            TEvent @event
        ) where TEvent : Event;
    }
}