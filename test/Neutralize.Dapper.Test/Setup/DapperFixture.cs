using System;
using Microsoft.Extensions.Configuration;
using Moq.AutoMock;
using Xunit;

namespace Neutralize.Dapper.Test.Setup
{
    [CollectionDefinition(nameof(NeutralizeDapperCollection))]
    public class NeutralizeDapperCollection : ICollectionFixture<NeutralizeDapperFixture> {}
    
    public class NeutralizeDapperFixture : IDisposable
    {
        public AutoMocker Mocker { get; set; }
        public DapperSQLiteRepository_Mock SQLiteRepository { get; set; }
        
        public NeutralizeDapperFixture()
        {
            Mocker = new AutoMocker();
        }
        
        public void Dispose()
        {
            SQLiteRepository?.Dispose();
        }

        public DapperSQLiteRepository_Mock GenereteDapperSQLiteRepository()
        {
            Mocker = new AutoMocker();
            SQLiteRepository = Mocker.CreateInstance<DapperSQLiteRepository_Mock>();

            Mocker
                .GetMock<IConfiguration>()
                .Setup(s => s["ConnectionString:DefaultConnection"])
                .Returns("Data Source=:memory:");
            
            return SQLiteRepository;
        }
    }
}