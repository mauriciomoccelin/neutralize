using System.Collections.Generic;
using System.Threading.Tasks;

namespace BuildingBlocks.Domain
{
    public interface IBus
    {
        Task SendCommand<TCommand>(
            TCommand command
        ) where TCommand : Command;

        Task SendCommand<TCommand>(
            IEnumerable<TCommand> command
        ) where TCommand : Command;

        Task<TResponse> SendCommand<TCommand, TResponse>(
            TCommand command
        ) where TCommand : Command where TResponse : struct;

        Task RaiseEvent<TEvent>(
            TEvent @event
        ) where TEvent : Event;
    }
}