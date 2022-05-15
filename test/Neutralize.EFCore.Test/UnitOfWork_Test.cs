using System;
using System.Threading.Tasks;
using FluentAssertions;
using Neutralize.EFCore.Test.Setup;
using Xunit;

namespace Neutralize.EFCore.Test
{
    [Collection(nameof(NeutralizeEFCoreCollection))]
    public class UnitOfWork_Test : IDisposable
    {
        private readonly NeutralizeEFCoreFixture fixture;
        
        public UnitOfWork_Test(NeutralizeEFCoreFixture fixture)
        {
            this.fixture = fixture;
        }
        
        public void Dispose()
        {
            fixture?.Dispose();
        }
        
        [Trait("Data", "UnitOfWork")]
        [Fact(DisplayName = "Commit with succes")]
        public async Task RepositoryAddAsyncMustBeSuccess()
        {
            // Arrange
            var todo = fixture.GenereteTodo();
            
            var dbCOntext = fixture.GeneretDbCOntext();
            var uwo = fixture.GenereteSQLiteRepository(dbCOntext);
            var repository = fixture.GenereteSQLiteRepository(dbCOntext);
            
            // Act
            await repository.AddAsync(todo);
            var commit = await uwo.Commit();
            
            // Assert
            commit.Should().BeTrue();
            todo.Id.Should().BeGreaterThan(0);
        }
        
        [Trait("Data", "UnitOfWork")]
        [Fact(DisplayName = "Commit winout affected rows")]
        public async Task RepositoryAddsAsyncMustBeSuccess()
        {
            // Arrange
            var dbCOntext = fixture.GeneretDbCOntext();
            var uwo = fixture.GenereteSQLiteRepository(dbCOntext);
            
            // Act
            var commit = await uwo.Commit();
            
            // Assert
            commit.Should().BeFalse();
        }
    }
}