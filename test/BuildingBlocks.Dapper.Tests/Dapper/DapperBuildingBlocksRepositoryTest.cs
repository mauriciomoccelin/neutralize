using FluentAssertions;
using Xunit;

namespace BuildingBlocks.Tests.Dapper
{
    public class DapperBuildingBlocksRepositoryTest : DapperBuildingBlocksBaseTest
    {
        private readonly IToDoDapperRepository repository;
        
        public DapperBuildingBlocksRepositoryTest()
        {
            repository = Resolve<IToDoDapperRepository>();
        }

        [Fact(DisplayName = "Test for: Dapper Repository")]
        public async void Test_Dapper_Repository()
        {
            // Act
            await repository.AddAsync(new ToDo(1, true, "DOLOR IPSUM"));
            await repository.AddAsync(new ToDo(2, false, "LOREM 2 IPSUM"));
            await repository.AddAsync(new ToDo(3, false, "LOREM 3 IPSUM"));
            await repository.AddAsync(new ToDo(4, false, "LOREM 4 IPSUM"));
            await repository.AddAsync(new ToDo(5, false, "LOREM 5 IPSUM"));
            await repository.AddAsync(new ToDo(6, true, "DOLOR IPSUM"));
            await repository.AddAsync(new ToDo(7, false, "LOREM 7 IPSUM"));
            await repository.AddAsync(new ToDo(8, false, "LOREM 8 IPSUM"));
            await repository.AddAsync(new ToDo(9, false, "LOREM 9 IPSUM"));
            await repository.AddAsync(new ToDo(10, false, "LOREM 10 IPSUM"));
            await repository.AddAsync(new ToDo(11, false, "LOREM 11 IPSUM"));
            await repository.AddAsync(new ToDo(12, false, "LOREM 12 IPSUM"));
            
            var toDos = await repository.GetAllAsync();
            var responseLike = await repository.GetAllAsync(
                x => x.Description.Contains("LOREM"), // LIKE
                sort: x => x.Description
            );
            var page = await repository.GetAllPagedAsync(
                0, 10,
                x => x.Description.Contains("LOREM") && x.Done == false,
                sort: x => x.Description
            );
            var todo = await repository.GetAsync(2);
            
            todo.MarkAsDone();
            todo.ChangeDescription("DOLOR IPSUM");
            
            await repository.UpdateAsync(todo);

            var countDone = await repository.CountAsync(x => x.Done == true); 
            
            // Assert
            toDos.Should().NotBeNull();
            page.Should().HaveCount(9);
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