using FluentAssertions;
using Optional;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace BuildingBlocks.Domain.Tests.Commands
{
    public class Command_Test : TestBase
    {
        private readonly IBus bus;

        public Command_Test() { bus = Resolve<IBus>(); }

        [Fact]
        public async Task SendCommand_Test()
        {
            // Act
            var comandOne = new AddProductCommand("dolor sit");
            var comandTwo = new AddProductCommand("lorem ipsum");
            var invalidComand = new AddProductCommand(null);

            var comands = new List<Command>()
            {
                new AddPeopleCommand("dolor sit"),
                new AddPeopleCommand("lorem ipsum"),
            };

            var responseComandOne = await bus.SendCommand<AddProductCommand, Option<string>>(comandOne);
            var responseComandTwo = await bus.SendCommand<AddProductCommand, Option<string>>(comandTwo);
            var responseInvalidComand = await bus.SendCommand<AddProductCommand, Option<string>>(invalidComand);

            // Send may commands
            await bus.SendCommand(comands);

            // Assert
            comandOne.IsValid().Should().BeTrue();
            comandTwo.IsValid().Should().BeTrue();
            invalidComand.IsValid().Should().BeFalse();

            responseComandOne.HasValue.Should().BeTrue();
            responseComandTwo.HasValue.Should().BeTrue();
            responseInvalidComand.HasValue.Should().BeFalse();
        }
    }
}
