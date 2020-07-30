using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;
using BuildingBlocks.Data.Tests.Dapper;
using BuildingBlocks.Data.Tests.EFCore;
using BuildingBlocks.Test;
using Dapper;
using DapperExtensions.Sql;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Data.Tests
{
    public abstract class BuildingBlocksDataTestBase : TestBase
    {
        protected BuildingBlocksDataTestBase()
        {
            // Setup In Memory Database
            services.AddDbContext<EfCoreDbContext>(
                option => option.UseInMemoryDatabase("Test")
            );
            
            // Setup Dapper Database

            DapperExtensions.DapperExtensions.Configure(
                typeof(BuildingBlocksDataTestBase), 
                new List<Assembly>() {typeof(BuildingBlocksDataTestBase).Assembly }, 
                new SqliteDialect()
            );

            services.AddScoped<DbConnection>(sp =>
            {
                var connection = new SqliteConnection("Data Source=Contatos.db");
                connection.Open();
                connection.Execute(@"
                    DROP TABLE ToDos;
                    CREATE TABLE IF NOT EXISTS ToDos (
	                    Id PRIMARY KEY,
	                    Description TEXT NULL,
	                    Done BIT NOT NULL
                    );
                ");

                return connection;
            });

            // Register DB Context Provider
            services.AddScoped<EfCoreDbContext>();
            
            // Register repositories 
            services.AddScoped<ITodoRepository, TodoRepository>();
            services.AddScoped<IToDoDapperRepository, ToDoDapperRepository>();
            
            // build service provider
            provider = services.BuildServiceProvider();
        }
    }
}