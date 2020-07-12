using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;

namespace BuildingBlocks.Domain.Tests
{
    public sealed class InMemoryBus : IBus
    {
        private readonly IMediator mediator;

        public InMemoryBus(IMediator mediator) => this.mediator = mediator;

        public Task SendCommand<TCommand>(
            TCommand command
        ) where TCommand : Command
        {
            return mediator.Send(command);
        }

        public Task SendCommand<TCommand>(
            IEnumerable<TCommand> commands
        ) where TCommand : Command
        {
            return Task.WhenAll(commands.Select(command => mediator.Send<Unit>(command)));
        }

        public Task<TResponse> SendCommand<TCommand, TResponse>(
            TCommand command
        ) where TCommand : Command where TResponse : struct
        {
            return mediator.Send(command as IRequest<TResponse>);
        }

        public Task RaiseEvent<TEvent>(
            TEvent @event
        ) where TEvent : Event
        {
            return mediator.Publish(@event);
        }
    }
}