using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using Dapper;
using Moq.AutoMock;
using Xunit;

namespace Neutralize.Dapper.Test.Setup
{
    [CollectionDefinition(nameof(NeutralizeDapperCollection))]
    public class NeutralizeDapperCollection : ICollectionFixture<NeutralizeDapperFixture> { }

    public class NeutralizeDapperFixture : IDisposable
    {
        public AutoMocker Mocker { get; set; }

        public NeutralizeDapperFixture()
        {
            Mocker = new AutoMocker();
        }

        public void Dispose() { }

        public IDapperRepository GenereteDapperRepository()
        {
            Mocker = new AutoMocker();
            return Mocker.CreateInstance<DapperRepository>();
        }

        public IDbConnection GenereteConnection()
        {
            var connection = new SQLiteConnection("Data Source=:memory:");

            connection.Open();
            connection.Execute(ReadScriptFile("CreateInitialTables"));

            return connection;
        }

        private static string ReadScriptFile(string name)
        {
            var path = Path.Combine(AppContext.BaseDirectory, "Setup", $"{name}.sql");
            using var sr = new StreamReader(path);
            return sr.ReadToEnd();
        }
    }
}