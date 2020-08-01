using BuildingBlocks.Test.Entities;
using FluentAssertions;
using Xunit;

namespace BuildingBlocks.EFCore.Tests
{
    public class EfCoreRepositoryEfCore : EFCoreBaseBase
    {
        private readonly ITodoRepository repository;
        
        public EfCoreRepositoryEfCore()
        {
            repository = Resolve<ITodoRepository>();
        }

        [Fact(DisplayName = "Test for: Entity Framework Core Repository")]
        public async void Test_Entity_Framework_Core_Repository()
        {
            // Act
            var todo = new ToDo(0, true, "Lorem Ipsum");
            repository.Awaiting(x => x.AddAsync(todo)).Should().NotThrow();
            var result = await repository.Commit();
            // Assert
            result.Should().BeTrue();
            
            // Act
            var todoGetById = await repository.GetAsync(todo.Id);
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
            todoGetById = await repository.GetAsync(todo.Id);
            // Assert
            todoGetById.Should().BeNull();
        }

        public override void Dispose()
        {
            repository.Dispose();
        }
    }
}