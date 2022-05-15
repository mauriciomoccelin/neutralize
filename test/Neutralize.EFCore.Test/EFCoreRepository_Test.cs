using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Neutralize.EFCore.Test.Setup;
using Xunit;

namespace Neutralize.EFCore.Test
{
    [Collection(nameof(NeutralizeEFCoreCollection))]
    public class EFCoreRepository_Test : IDisposable
    {
        private readonly NeutralizeEFCoreFixture fixture;
        
        public EFCoreRepository_Test(NeutralizeEFCoreFixture fixture)
        {
            this.fixture = fixture;
        }
        
        public void Dispose()
        {
            fixture?.Dispose();
        }
        
        [Trait("Data", "EFCore")]
        [Fact(DisplayName = "Add entity in generic repository must be success")]
        public async Task RepositoryAddAsyncMustBeSuccess()
        {
            // Arrange
            var todo = fixture.GenereteTodo();
            var repository = fixture.GenereteSQLiteRepository();
            
            // Act
            await repository.AddAsync(todo);
            var commit = await repository.Commit();
            
            // Assert
            commit.Should().BeTrue();
            todo.Id.Should().BeGreaterThan(0);
        }
        
        [Trait("Data", "EFCore")]
        [Fact(DisplayName = "Update entity in generic repository must be success")]
        public async Task RepositoryUpdateAsyncMustBeSuccess()
        {
            // Arrange
            var todo = fixture.GenereteTodo();
            var repository = fixture.GenereteSQLiteRepository();
            
            await repository.AddAsync(todo);
            await repository.Commit();

            todo.Desacription += " Update"; 
            
            // Act
            await repository.UpdateAsync(todo);
            await repository.Commit();

            // Assert
            todo.Id.Should().BeGreaterThan(0);
            var todoGet = await repository.GetAsync(todo.Id);

            todoGet.Should().NotBeNull();
            todoGet.Desacription.Should().ContainEquivalentOf("Update");
        }
        
        [Trait("Data", "EFCore")]
        [Fact(DisplayName = "Delete entity in generic repository must be success")]
        public async Task RepositoryDeleteAsyncMustBeSuccess()
        {
            // Arrange
            var todo = fixture.GenereteTodo();
            var repository = fixture.GenereteSQLiteRepository();
            
            await repository.AddAsync(todo);
            await repository.Commit();

            // Act
            await repository.RemoveAsync(todo);
            await repository.Commit();

            // Assert
            var todoGet = await repository.GetAsync(todo.Id);

            todoGet.Should().BeNull("Because the entity was removed");
        }
        
        [Trait("Data", "EFCore")]
        [Fact(DisplayName = "Get all entities in generic repository must be success")]
        public async Task RepositoryGetAllAsyncMustBeSuccess()
        {
            // Arrange
            var repository = fixture.GenereteSQLiteRepository();
            
            _ = await fixture.SeedForTodo(repository);
            
            // Act
            var todos = await repository.GetAll().ToListAsync();

            // Assert
            todos.Should().NotBeEmpty();
            todos.Count.Should().BeGreaterOrEqualTo(10);
        }
        
        [Trait("Data", "EFCore")]
        [Fact(DisplayName = "Count entities by condition in generic repository must be success")]
        public async Task RepositoryCountAsyncMustBeSuccess()
        {
            // Arrange
            var repository = fixture.GenereteSQLiteRepository();
            
            _ = await fixture.SeedForTodo(repository);
            
            // Act
            var count = await repository.CountAsync(todo => todo.Desacription != null);

            // Assert
            count.Should().BeGreaterOrEqualTo(10);
        }
        
        [Trait("Data", "EFCore")]
        [Fact(DisplayName = "Get all by condition entities in generic repository must be success")]
        public async Task RepositoryGetListAsyncMustBeSuccess()
        {
            // Arrange
            var repository = fixture.GenereteSQLiteRepository();
            
            _ = await fixture.SeedForTodo(repository);
            
            // Act
            var todos = await repository.GetListAsync(todo => todo.Desacription != null);

            // Assert
            todos.Should().NotBeEmpty();
            todos.Count().Should().BeGreaterOrEqualTo(10);
        }
        
        [Trait("Data", "EFCore")]
        [Fact(DisplayName = "First or default by true condition in generic repository must return non nullable entity")]
        public async Task RepositoryFirstOrAsyncMustBeSuccess()
        {
            // Arrange
            var repository = fixture.GenereteSQLiteRepository();
            
            _ = await fixture.SeedForTodo(repository);
            
            // Act
            var todo = await repository.FirstOrAsync(todo => todo.Desacription != null);

            // Assert
            todo.Should().NotBeNull();
        }
        
        [Trait("Data", "EFCore")]
        [Fact(DisplayName = "First or default by true condition in generic repository must return default value(null)")]
        public async Task RepositoryFirstOrAsyncForFalseConditionMustReturnDefaultValue()
        {
            // Arrange
            var repository = fixture.GenereteSQLiteRepository();
            
            _ = await fixture.SeedForTodo(repository);
            
            // Act
            var todo = await repository.FirstOrAsync(todo => todo.Id == -1);

            // Assert
            todo.Should().BeNull("Because not exists entity with id -1");
        }
        
        [Trait("Data", "EFCore")]
        [Fact(DisplayName = "Get by id entity in generic repository must be success")]
        public async Task RepositoryGetAsyncMustBeSuccess()
        {
            // Arrange
            var todo = fixture.GenereteTodo();
            var repository = fixture.GenereteSQLiteRepository();
            
            await repository.AddAsync(todo);
            await repository.Commit();

            // Act
            var result = await repository.GetAsync(todo.Id);
            
            // Assert
            result.Should().NotBeNull();
        }
        
        [Trait("Data", "EFCore")]
        [Fact(DisplayName = "Get by id entity in generic repository must fail entity not exists")]
        public async Task RepositoryGetAsyncMustReturnNull()
        {
            // Arrange
            var repository = fixture.GenereteSQLiteRepository();

            // Act
            var result = await repository.GetAsync(1);
            
            // Assert
            result.Should().BeNull();
        }
    }
}