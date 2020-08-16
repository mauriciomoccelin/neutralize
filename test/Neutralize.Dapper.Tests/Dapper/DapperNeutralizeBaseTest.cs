using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;
using Dapper;
using DapperExtensions.Sql;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;

namespace Neutralize.Tests.Dapper
{
    public abstract class DapperNeutralizeBaseTest : NeutralizeBaseTest
    {
        protected DapperNeutralizeBaseTest()
        {
            // Setup Dapper Database

            DapperExtensions.DapperExtensions.Configure(
                typeof(DapperNeutralizeBaseTest), 
                new List<Assembly>() {typeof(DapperNeutralizeBaseTest).Assembly }, 
                new SqliteDialect()
            );

            services.AddScoped<DbConnection>(sp =>
            {
                var connection = new SqliteConnection("Data Source=Tests.db");
                connection.Open();
                connection.Execute(@"
                    DROP TABLE IF EXISTS ToDos;
                    CREATE TABLE IF NOT EXISTS ToDos (
	                    Id PRIMARY KEY,
	                    Description TEXT NULL,
	                    Done BIT NOT NULL
                    );
                ");

                return connection;
            });
            
            // Register repositories
            services.AddScoped<IToDoDapperRepository, ToDoDapperRepository>();
            
            // build service provider
            provider = services.BuildServiceProvider();
        }
    }
}
