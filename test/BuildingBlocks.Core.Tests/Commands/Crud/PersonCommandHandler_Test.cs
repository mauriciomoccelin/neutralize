using System;
using System.Threading.Tasks;
using BuildingBlocks.Core.Application;
using BuildingBlocks.Core.Bus;
using BuildingBlocks.Core.Commands;
using FluentAssertions;
using Xunit;

namespace BuildingBlocks.Core.Tests.Commands.Crud
{
    public class PersonCommandHandler_Test : BuildingBlocksCoreBaseTest
    {
        private readonly IInMemoryBus inMemoryBus;
        
        public PersonCommandHandler_Test() { inMemoryBus = Resolve<IInMemoryBus>(); }

        [Fact]
        public async Task CrudCommandHandler_Test()
        {
            await inMemoryBus.SendCommand(new CreatePersonCommand("Lorem Ipsum"));
            await inMemoryBus.SendCommand(new UpdatePersonCommand(1, "Lorem Ipsum"));

            var persons = await inMemoryBus.SendCommand<GetPagedPersonCommand, PagedResultDto<PersonDto>>(
                new GetPagedPersonCommand()
            );
            
            var person = await inMemoryBus.SendCommand<GetPersonCommand, PersonDto>(new GetPersonCommand(1));
            await inMemoryBus.SendCommand(new DeletePersonCommand(1));
            
            person.Should().NotBeNull();
            persons.Should().NotBeNull();
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}