using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Neutralize.Commands;
using Neutralize.Events;

namespace Neutralize.Bus
{
    public sealed class InMemoryBus : IInMemoryBus
    {
        public IMediator Mediator { get; }
        
        public InMemoryBus(IMediator mediator) => Mediator = mediator;

        public Task SendCommand<TCommand, TId>(
            TCommand command
        ) where TId : struct where TCommand : Command<TId>
        {
            return command == null ? Task.CompletedTask : Mediator.Send(command);
        }

        public Task SendCommand<TCommand, TId>(
            IEnumerable<TCommand> commands
        ) where TId : struct where TCommand : Command<TId>
        {
            return commands == null || commands?.Any() == false
                ? Task.CompletedTask
                : Task.WhenAll(commands.Select(command => Mediator.Send(command)));
        }

        public Task<TResponse> SendCommand<TCommand, TId, TResponse>(
            TCommand command
        ) where TCommand : Command<TId> where TId : struct
        {
            return Mediator.Send(
                command as IRequest<TResponse> ?? throw new ArgumentNullException(
                    nameof(command), "The command not be null"
                )
            );
        }

        public Task<TResponse> SendCommandGuidId<TResponse>(
            Command<Guid> command
        )
        {
            return SendCommand<Command<Guid>, Guid, TResponse>(command);
        }

        public Task<TResponse> SendCommandInt64Id<TResponse>(
            Command<long> command
        )
        {
            return SendCommand<Command<long>, long, TResponse>(command);
        }

        public Task RaiseEvent<TEvent>(
            TEvent @event
        ) where TEvent : Event
        {
            return @event == null ? Task.CompletedTask : Mediator.Publish(@event);
        }
    }
}
