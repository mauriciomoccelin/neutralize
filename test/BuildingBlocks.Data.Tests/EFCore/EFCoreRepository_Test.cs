using BuildingBlocks.Data.Tests.Entities;
using FluentAssertions;
using Xunit;

namespace BuildingBlocks.Data.Tests.EFCore
{
    public class EfCoreRepositoryBuildingBlocksDataTest : BuildingBlocksDataTestBase
    {
        private readonly ITodoRepository repository;
        
        public EfCoreRepositoryBuildingBlocksDataTest()
        {
            repository = Resolve<ITodoRepository>();
        }

        [Fact]
        public async void Add_Test()
        {
            // Act
            var todo = new ToDo(0, true, "Lorem Ipsum");
            repository.Awaiting(x => x.AddAsync(todo)).Should().NotThrow();
            var result = await repository.Commit();
            // Assert
            result.Should().BeTrue();
            
            // Act
            var todoGetById = await repository.GetByIdAsync(todo.Id);
            // Assert
            todoGetById.Should().NotBeNull();

            // Act
            todoGetById.MarkAsDone();
            await repository.UpdateAsync(todoGetById);
            result = await repository.Commit();
            // Assert
            result.Should().BeTrue();
            
            // Act
            await repository.RemoveAsync(todoGetById);
            result = await repository.Commit();
            // Assert
            result.Should().BeTrue();
            
            // Act
            todoGetById = await repository.GetByIdAsync(todo.Id);
            // Assert
            todoGetById.Should().BeNull();
        }

        public override void Dispose()
        {
            repository.Dispose();
        }
    }
}