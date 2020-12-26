using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Neutralize.Commands;
using Neutralize.Events;

namespace Neutralize.Bus
{
    public interface INeutralizeBus
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

        Task<TResponse> SendCommandGuidId<TResponse>(
            Command<Guid> command
        );

        Task<TResponse> SendCommandInt64Id<TResponse>(
            Command<long> command
        );

        Task RaiseEvent<TEvent>(
            TEvent @event
        ) where TEvent : Event;
    }
}
