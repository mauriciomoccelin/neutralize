using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BuildingBlocks.Core.Bus;
using FluentAssertions;
using Xunit;

namespace BuildingBlocks.Core.Tests.Commands.OnDemand
{
    public class Command_Test : BuildingBlocksCoreBaseTest
    {
        private readonly IInMemoryBus inMemoryBus;

        public Command_Test() { inMemoryBus = Resolve<IInMemoryBus>(); }

        [Fact]
        public async Task SendCommand_Test()
        {
            // Act
            var commandOne = new AddProductCommand("dolor sit");
            var commandTwo = new AddProductCommand("lorem ipsum");
            var invalidCommand = new AddProductCommand(null);

            var commands = new List<AddPeopleCommand>()
            {
                new AddPeopleCommand("dolor sit"),
                new AddPeopleCommand("lorem ipsum"),
            };

            var responseCommandOne = await inMemoryBus.SendCommand<AddProductCommand, Guid, string>(commandOne);
            var responseCommandTwo = await inMemoryBus.SendCommand<AddProductCommand, Guid, string>(commandTwo);
            var responseInvalidCommand = await inMemoryBus.SendCommand<AddProductCommand, Guid, string>(invalidCommand);

            // Send may commands
            await inMemoryBus.SendCommand<AddPeopleCommand, Guid>(commands);

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
