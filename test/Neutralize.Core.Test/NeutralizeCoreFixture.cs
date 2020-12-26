using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Moq.AutoMock;
using Neutralize.Bus;
using Neutralize.Commands;
using Neutralize.Events;
using Xunit;

namespace Neutralize.Core.Test
{
    [CollectionDefinition(nameof(NeutralizeCoreCollection))]
    public class NeutralizeCoreCollection : ICollectionFixture<NeutralizeCoreFixture> {}
    
    public class NeutralizeCoreFixture : IDisposable
    {
        public AutoMocker Mocker;
        public InMemoryBus Bus;
        
        public void Dispose() { }

        public InMemoryBus GenereteDefaultNeutralizeBus()
        {
            Mocker = new AutoMocker();
            Bus = Mocker.CreateInstance<InMemoryBus>();
            
            return Bus;
        }
        
        public Command<Guid> GenereteGenericCommand()
        {
            var faker = new Faker<CommandGuid<Guid>>()
                .CustomInstantiator(fake => new CommandGuid<Guid>())
                .RuleFor(x => x.Id, fake => fake.Random.Guid());

            return faker.Generate();
        }
        
        public IEnumerable<Command<Guid>> GenereteManyGenericCommand(int quantity = 5)
        {
            var faker = new Faker<CommandGuid<Guid>>()
                .CustomInstantiator(fake => new CommandGuid<Guid>())
                .RuleFor(x => x.Id, fake => fake.Random.Guid());

            return faker.Generate(quantity).AsEnumerable();
        }
        
        public CommandGuid<Guid> GenereteGuidCommand()
        {
            var faker = new Faker<CommandGuid<Guid>>()
                .CustomInstantiator(fake => new CommandGuid<Guid>())
                .RuleFor(x => x.Id, fake => fake.Random.Guid());

            return faker.Generate();
        }
        
        public Command<long> GenereteInt64Command()
        {
            var faker = new Faker<CommandInt64<long>>()
                .CustomInstantiator(fake => new CommandInt64<long>())
                .RuleFor(x => x.Id, fake => fake.Random.Long(1));

            return faker.Generate();
        }
        
        public Event GenereteDefaultEvent()
        {
            var faker = new Faker<Event>()
                .CustomInstantiator(fake => new Event())
                .RuleFor(x => x.AggregateId, fake => fake.Random.Guid());

            return faker.Generate();
        }
    }
}