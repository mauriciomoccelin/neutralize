using System;
using System.Threading.Tasks;
using FluentAssertions;
using Neutralize.Dapper.Test.Setup;
using Xunit;

namespace Neutralize.Dapper.Test
{
    [Collection(nameof(NeutralizeDapperCollection))]
    public class DapperRepository_Test : IDisposable
    {
        private readonly NeutralizeDapperFixture fixture;

        public DapperRepository_Test(NeutralizeDapperFixture fixture)
        {
            this.fixture = fixture;
        }

        public void Dispose()
        {
            fixture?.Dispose();
        }

        [Trait("Data", "Dapper")]
        [Fact(DisplayName = "Execute dapper query paged abstraction with sucess")]
        public async Task Paged_ValidQuery_PagedWithSuccess()
        {
            // Arrange
            var repository = fixture.GenereteDapperRepository();

            fixture.Mocker
                .GetMock<IDapperConnectionFactory>()
                .Setup(x => x.CreateOpened())
                .Returns(fixture.GenereteConnection());

            const string commandCount = "SELECT COUNT(Id) FROM ToDo";
            const string commandSelectFromTodo = "SELECT * FROM ToDo";
            const string command = commandCount + ";" + commandSelectFromTodo;

            // Act
            var queryPaged = await repository.Paged<ToDoList>(command);

            // Assert
            queryPaged.Should().NotBeNull();
            queryPaged.HasValue.Should().BeTrue();
        }

        [Trait("Data", "Dapper")]
        [Fact(DisplayName = "Execute dapper query paged abstraction with failure because forgout split query")]
        public async Task Paged_ForgoutSplitQuery_ThrowArgumentException()
        {
            // Arrange
            var repository = fixture.GenereteDapperRepository();

            fixture.Mocker
                .GetMock<IDapperConnectionFactory>()
                .Setup(x => x.CreateOpened())
                .Returns(fixture.GenereteConnection());

            const string commandCount = "SELECT COUNT(Id) FROM ToDo";
            const string commandSelectFromTodo = "SELECT * FROM ToDo";
            const string command = commandCount + commandSelectFromTodo;

            // Act
            var queryPaged = await repository.Paged<ToDoList>(command);

            // Assert
            queryPaged.Should().NotBeNull();
            queryPaged.HasValue.Should().BeFalse();
        }

        [Trait("Data", "Dapper")]
        [Fact(DisplayName = "Execute dapper query to get first record with success")]
        public async Task QueryPaged_ValidQuery_MustRetournTopOneItem()
        {
            // Arrange
            var repository = fixture.GenereteDapperRepository();

            fixture.Mocker
                .GetMock<IDapperConnectionFactory>()
                .Setup(x => x.CreateOpened())
                .Returns(fixture.GenereteConnection());

            repository.AddParameter("id", 1);

            const string command = "SELECT * FROM ToDo WHERE Id = @id";

            // Act
            var queryPaged = await repository.First<ToDoList>(command);

            // Assert
            queryPaged.Should().NotBeNull();
            queryPaged.HasValue.Should().BeTrue();
        }

        [Trait("Data", "Dapper")]
        [Fact(DisplayName = "Execute dapper query to get first record with fail because not found")]
        public async Task QueryPaged_ValidQuery_ButNotFoundReturnEmpty()
        {
            // Arrange
            var repository = fixture.GenereteDapperRepository();

            fixture.Mocker
                .GetMock<IDapperConnectionFactory>()
                .Setup(x => x.CreateOpened())
                .Returns(fixture.GenereteConnection());

            repository.AddParameter("id", 0);

            const string command = "SELECT * FROM ToDo WHERE Id = @id";

            // Act
            var queryPaged = await repository.First<ToDoList>(command);

            // Assert
            queryPaged.Should().NotBeNull();
            queryPaged.HasValue.Should().BeFalse();
        }
    }
}