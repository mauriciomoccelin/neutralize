using BuildingBlocks.Data.Tests.Entities;
using FluentAssertions;
using Xunit;

namespace BuildingBlocks.Data.Tests.Dapper
{
    public class DapperRepository_Test : BuildingBlocksDataTestBase
    {
        private readonly IToDoDapperRepository repository;
        
        public DapperRepository_Test()
        {
            repository = Resolve<IToDoDapperRepository>();
        }

        [Fact]
        public async void GetAll_Test()
        {
            // Act
            await repository.AddAsync(new ToDo(1, true, "LOREM IPSUM"));
            await repository.AddAsync(new ToDo(2, true, "LOREM IPSUM"));
            await repository.AddAsync(new ToDo(3, true, "LOREM IPSUM"));
            await repository.AddAsync(new ToDo(4, true, "LOREM IPSUM"));
            await repository.AddAsync(new ToDo(5, true, "LOREM IPSUM"));
            
            var toDos = await repository.GetAll();
            
            // Assert
            toDos.Should().NotBeNull();
        }
        
        public override void Dispose()
        {
            repository.Dispose();
        }
    }
}