using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using MediatR;
using Moq;
using Neutralize.Bus;
using Neutralize.Commands;
using Neutralize.Events;
using Xunit;

namespace Neutralize.Core.Test
{
    [Collection(nameof(NeutralizeCoreCollection))]
    public class InMemoryBus_Tests
    {
        private readonly InMemoryBus bus;
        private readonly NeutralizeCoreFixture fixture;
        
        public InMemoryBus_Tests(NeutralizeCoreFixture fixture)
        {
            this.fixture = fixture;
            bus = fixture.GenereteDefaultNeutralizeBus();
        }
        
        [Trait("Category", "Core - Bus")]
        [Fact(DisplayName = "Send generic command to memory bus (local command)")]
        public void InMemoryBus_SendGenericCommand_WithSuccess()
        {
            // Arrange
            var command = fixture.GenereteGenericCommand();

            // Act
            bus.SendCommand<Command<Guid>, Guid>(command);

            // Assert
            fixture.Mocker
                .GetMock<IMediator>()
                .Verify(x => x.Send(It.IsAny<Command<Guid>>(), It.IsAny<CancellationToken>()), Times.Once);
        }
        
        [Trait("Category", "Core - Bus")]
        [Fact(DisplayName = "Send many generic command to memory bus (local command)")]
        public void InMemoryBus_SendManyGenericCommand_WithSuccess()
        {
            // Arrange
            var command = fixture.GenereteManyGenericCommand(5);
        
            // Act
            bus.SendCommand<Command<Guid>, Guid>(command);
        
            // Assert
            fixture.Mocker
                .GetMock<IMediator>()
                .Verify(x => x.Send(It.IsAny<Command<Guid>>(), It.IsAny<CancellationToken>()), Times.Exactly(5));
        }

        [Trait("Category", "Core - Bus")]
        [Fact(DisplayName = "Send guid command to memory bus (local command)")]
        public async void InMemoryBus_SendGuidCommand_WithSuccess()
        {
            // Arrange
            var resultValue = Guid.NewGuid();
            var command = fixture.GenereteGuidCommand();
            fixture.Mocker
                .GetMock<IMediator>()
                .Setup(x => x.Send<Guid>(It.IsAny<CommandGuid<Guid>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(resultValue);
            
            // Act
            var result = await bus.SendCommandGuidId<Guid>(command);
        
            // Assert
            Assert.IsType<Guid>(result);
            
            fixture.Mocker
                .GetMock<IMediator>()
                .Verify(x => x.Send<Guid>(It.IsAny<CommandGuid<Guid>>(), It.IsAny<CancellationToken>()), Times.Once);
        }
        
        [Trait("Category", "Core - Bus")]
        [Fact(DisplayName = "Send int64 command to memory bus (local command)")]
        public async void InMemoryBus_SendInt64Command_WithSuccess()
        {
            // Arrange
            const long resultValue = 0L;
            var command = fixture.GenereteInt64Command();
            fixture.Mocker
                .GetMock<IMediator>()
                .Setup(x => x.Send<long>(It.IsAny<CommandInt64<long>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(resultValue);

            // Act
            var result = await bus.SendCommandInt64Id<long>(command);
        
            // Assert
            Assert.IsType<long>(result);

            fixture.Mocker
                .GetMock<IMediator>()
                .Verify(x => x.Send<long>(It.IsAny<CommandInt64<long>>(), It.IsAny<CancellationToken>()), Times.Once);
        }
        
        [Trait("Category", "Core - Bus")]
        [Fact(DisplayName = "Raise event to memory bus (local event)")]
        public async void InMemoryBus_RaiseEvent_WithSuccess()
        {
            // Arrange
            var @event  = fixture.GenereteDefaultEvent();

            // Act
            await bus.RaiseEvent(@event);
        
            // Assert
            fixture.Mocker
                .GetMock<IMediator>()
                .Verify(x => x.Publish(It.IsAny<Event>(), It.IsAny<CancellationToken>()), Times.Once);
        }
        
        [Trait("Category", "Core - Bus")]
        [Fact(DisplayName = "Send null generic command to memory bus (local command)")]
        public void InMemoryBus_SendNullGenericCommand_CannotSend()
        {
            // Arrange

            // Act
            bus.SendCommand<Command<Guid>, Guid>((Command<Guid>)null);

            // Assert
            fixture.Mocker
                .GetMock<IMediator>()
                .Verify(x => x.Send(It.IsAny<Command<Guid>>(), It.IsAny<CancellationToken>()), Times.Never);
        }
        
        [Trait("Category", "Core - Bus")]
        [Fact(DisplayName = "Send many null generic command to memory bus (local command)")]
        public void InMemoryBus_SendManyNullGenericCommand_CannotSend()
        {
            // Arrange

            // Act
            bus.SendCommand<Command<Guid>, Guid>((IEnumerable<Command<Guid>>)null);

            // Assert
            fixture.Mocker
                .GetMock<IMediator>()
                .Verify(x => x.Send(It.IsAny<IEnumerable<Command<Guid>>>(), It.IsAny<CancellationToken>()), Times.Never);
        }
        
        [Trait("Category", "Core - Bus")]
        [Fact(DisplayName = "Send empty list generic command to memory bus (local command)")]
        public void InMemoryBus_SendEmptyListGenericCommand_CannotSend()
        {
            // Arrange
            var commands = Enumerable.Empty<Command<Guid>>();
            
            // Act
            bus.SendCommand<Command<Guid>, Guid>(commands);

            // Assert
            fixture.Mocker
                .GetMock<IMediator>()
                .Verify(x => x.Send(It.IsAny<IEnumerable<Command<Guid>>>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Trait("Category", "Core - Bus")]
        [Fact(DisplayName = "Raise null event to memory bus (local event)")]
        public async void InMemoryBus_RaiseEvent_CannotRaise()
        {
            // Arrange

            // Act
            await bus.RaiseEvent((Event) null);
        
            // Assert
            fixture.Mocker
                .GetMock<IMediator>()
                .Verify(x => x.Publish(It.IsAny<Event>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}