using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BuildingBlocks.Bus;
using BuildingBlocks.Commands;
using BuildingBlocks.Events;
using MediatR;

namespace BuildingBlocks.Tests
{
    public sealed class InMemoryInMemoryBus : IInMemoryBus
    {
        private readonly IMediator mediator;

        public InMemoryInMemoryBus(IMediator mediator) => this.mediator = mediator;

        public Task SendCommand<TCommand, TId>(
            TCommand command
        ) where TId : struct where TCommand : Command<TId>
        {
            return mediator.Send(command);
        }

        public Task SendCommand<TCommand, TId>(
            IEnumerable<TCommand> commands
        ) where TId : struct where TCommand : Command<TId>
        {
            return Task.WhenAll(commands.Select(command => mediator.Send<Unit>(command)));
        }

        public Task<TResponse> SendCommand<TCommand, TId, TResponse>(
            TCommand command
        ) where TCommand : Command<TId> where TId : struct
        {
            return mediator.Send(
                command as IRequest<TResponse> ?? throw new ArgumentNullException(
                    nameof(command), "The command not be null"
                )
            );
        }

        public Task<TResponse> SendCommandGuidId<TCommand, TResponse>(
            TCommand command
        ) where TCommand : Command<Guid>
        {
            return SendCommand<TCommand, Guid, TResponse>(command);
        }

        public Task<TResponse> SendCommandInt64Id<TCommand, TResponse>(
            TCommand command
        ) where TCommand : Command<long>
        {
            return SendCommand<TCommand, long, TResponse>(command);
        }

        public Task RaiseEvent<TEvent>(
            TEvent @event
        ) where TEvent : Event
        {
            return mediator.Publish(@event);
        }
    }
}