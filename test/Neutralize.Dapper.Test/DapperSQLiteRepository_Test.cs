using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Neutralize.Bus;
using Neutralize.Dapper.Test.Setup;
using Neutralize.Events;
using Xunit;

namespace Neutralize.Dapper.Test
{
    [Collection(nameof(NeutralizeDapperCollection))]
    public class DapperSQLiteRepository_Test : IDisposable
    {
        private readonly NeutralizeDapperFixture fixture;
        
        public DapperSQLiteRepository_Test(NeutralizeDapperFixture fixture)
        {
            this.fixture = fixture;
        }
        
        public void Dispose()
        {
            fixture?.Dispose();
        }
        
        [Trait("Data", "Dapper")]
        [Fact(DisplayName = "Execute dapper query paged abstraction with sucess")]
        public async Task DapperExecuteQueryPagedWithSuccess()
        {
            // Arrange
            var repository = fixture.GenereteDapperSQLiteRepository();

            const string commandCount = "SELECT COUNT(Id) FROM ToDo";
            const string commandSelectFromTodo = "SELECT * FROM ToDo";
            const string command = commandCount + ";" + commandSelectFromTodo;

            // Act
            var queryPaged  = await repository.QueryPaged<ToDoList>(command);

            // Assert
            queryPaged.Should().NotBeNull();
            queryPaged.HasValue.Should().BeTrue();
            
            fixture.Mocker
                .GetMock<IInMemoryBus>()
                .Verify(x => x.RaiseEvent(It.IsAny<Event>()), Times.Never);
        }
        
        [Trait("Data", "Dapper")]
        [Fact(DisplayName = "Execute dapper query paged abstraction with failure because forgout split query")]
        public async Task DapperExecuteQueryPagedWithFailureBecauseForgoutSplitQuery()
        {
            // Arrange
            var repository = fixture.GenereteDapperSQLiteRepository();
            
            fixture.Mocker
                .GetMock<IInMemoryBus>()
                .Setup(s => s.RaiseEvent(It.IsAny<Event>()))
                .Returns(Task.CompletedTask);

            const string commandCount = "SELECT COUNT(Id) FROM ToDo";
            const string commandSelectFromTodo = "SELECT * FROM ToDo";
            const string command = commandCount + commandSelectFromTodo;

            // Act
            var queryPaged  = await repository.QueryPaged<ToDoList>(command);

            // Assert
            queryPaged.Should().NotBeNull();
            queryPaged.HasValue.Should().BeFalse();
            
            fixture.Mocker
                .GetMock<IInMemoryBus>()
                .Verify(x => x.RaiseEvent(It.IsAny<Event>()), Times.Once);
        }
    }
}