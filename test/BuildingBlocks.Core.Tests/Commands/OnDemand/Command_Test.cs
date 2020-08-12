using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BuildingBlocks.Core.Bus;
using BuildingBlocks.Core.Commands;
using FluentAssertions;
using Xunit;

namespace BuildingBlocks.Core.Tests.Commands.OnDemand
{
    public class Command_Test : BuildingBlocksCoreBaseTest
    {
        private readonly IInMemoryBus _inMemoryBus;

        public Command_Test() { _inMemoryBus = Resolve<IInMemoryBus>(); }

        [Fact]
        public async Task SendCommand_Test()
        {
            // Act
            var commandOne = new AddProductCommand("dolor sit");
            var commandTwo = new AddProductCommand("lorem ipsum");
            var invalidCommand = new AddProductCommand(null);

            var commands = new List<Command>()
            {
                new AddPeopleCommand("dolor sit"),
                new AddPeopleCommand("lorem ipsum"),
            };

            var responseCommandOne = await _inMemoryBus.SendCommand<AddProductCommand, string>(commandOne);
            var responseCommandTwo = await _inMemoryBus.SendCommand<AddProductCommand, string>(commandTwo);
            var responseInvalidCommand = await _inMemoryBus.SendCommand<AddProductCommand, string>(invalidCommand);

            // Send may commands
            await _inMemoryBus.SendCommand(commands);

            // Assert
            commandOne.Validate().Should().BeTrue();
            commandTwo.Validate().Should().BeTrue();
            invalidCommand.Validate().Should().BeFalse();

            responseCommandOne.Should().NotBeNull();
            responseCommandTwo.Should().NotBeNull();
            responseInvalidCommand.Should().BeNull();
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
