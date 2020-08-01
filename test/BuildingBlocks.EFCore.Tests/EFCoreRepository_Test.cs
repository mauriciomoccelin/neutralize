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
            await repository.AddAsync(new ToDo(0, true, "DOLOR IPSUM"));
            await repository.AddAsync(new ToDo(0, false, "LOREM 2 IPSUM"));
            await repository.AddAsync(new ToDo(0, false, "LOREM 3 IPSUM"));
            await repository.AddAsync(new ToDo(0, false, "LOREM 4 IPSUM"));
            await repository.AddAsync(new ToDo(0, false, "LOREM 5 IPSUM"));
            await repository.AddAsync(new ToDo(0, true, "DOLOR IPSUM"));
            await repository.AddAsync(new ToDo(0, false, "LOREM 7 IPSUM"));
            await repository.AddAsync(new ToDo(0, false, "LOREM 8 IPSUM"));
            await repository.AddAsync(new ToDo(0, false, "LOREM 9 IPSUM"));
            await repository.AddAsync(new ToDo(0, false, "LOREM 10 IPSUM"));
            await repository.AddAsync(new ToDo(0, false, "LOREM 11 IPSUM"));
            await repository.AddAsync(new ToDo(0, false, "LOREM 12 IPSUM"));
            
            await repository.Commit();
            
            var toDos = repository.GetAll();
            var responseLike = await repository.GetListAsync(
                x => x.Description.Contains("LOREM")
            );
            var todo = await repository.GetAsync(2);
            todo.MarkAsDone();
            todo.ChangeDescription("DOLOR IPSUM");
            
            await repository.UpdateAsync(todo);
            await repository.Commit();
            
            var countDone = await repository.CountAsync(x => x.Done == true);
            
            // Assert
            toDos.Should().HaveCount(12);
            responseLike.Should().HaveCountGreaterThan(0);
            todo.Done.Should().BeTrue();
            countDone.Should().BeGreaterOrEqualTo(3);
        }

        public override void Dispose()
        {
            repository.Dispose();
        }
    }
}