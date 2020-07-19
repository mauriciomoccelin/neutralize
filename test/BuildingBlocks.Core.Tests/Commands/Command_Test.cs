using FluentAssertions;
using Optional;
using System.Collections.Generic;
using System.Threading.Tasks;
using BuildingBlocks.Core.Commands;
using BuildingBlocks.Core.Bus;
using Xunit;

namespace BuildingBlocks.Core.Tests.Commands
{
    public class Command_Test : TestBase
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

            var responseCommandOne = await _inMemoryBus.SendCommand<AddProductCommand, Option<string>>(commandOne);
            var responseCommandTwo = await _inMemoryBus.SendCommand<AddProductCommand, Option<string>>(commandTwo);
            var responseInvalidCommand = await _inMemoryBus.SendCommand<AddProductCommand, Option<string>>(invalidCommand);

            // Send may commands
            await _inMemoryBus.SendCommand(commands);

            // Assert
            commandOne.Validate().Should().BeTrue();
            commandTwo.Validate().Should().BeTrue();
            invalidCommand.Validate().Should().BeFalse();

            responseCommandOne.HasValue.Should().BeTrue();
            responseCommandTwo.HasValue.Should().BeTrue();
            responseInvalidCommand.HasValue.Should().BeFalse();
        }
    }
}
