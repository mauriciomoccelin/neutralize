using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuildingBlocks.Core.Bus;
using BuildingBlocks.Core.Commands;
using BuildingBlocks.Core.Events;
using MediatR;

namespace BuildingBlocks.Core.Tests
{
    public sealed class InMemoryInMemoryBus : IInMemoryBus
    {
        private readonly IMediator mediator;

        public InMemoryInMemoryBus(IMediator mediator) => this.mediator = mediator;

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
        ) where TCommand : Command where TResponse : class
        {
            return mediator.Send(
                command as IRequest<TResponse> ?? throw new ArgumentNullException(
                    nameof(command), "The command cano be null"
                )
            );
        }

        public Task RaiseEvent<TEvent>(
            TEvent @event
        ) where TEvent : Event
        {
            return mediator.Publish(@event);
        }
    }
}