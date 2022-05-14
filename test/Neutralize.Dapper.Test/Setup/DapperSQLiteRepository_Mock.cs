using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Neutralize.Bus;
using Neutralize.Notifications;
using Optional;

namespace Neutralize.Dapper.Test.Setup
{
    public class DapperSQLiteRepository_Mock : DapperMSSQLRepository
    {
        public DapperSQLiteRepository_Mock(
            IInMemoryBus inMemoryBus,
            IConfiguration configuration
        ) : base(inMemoryBus, configuration)
        {
        }

        protected override async Task<Option<T>> ConnectionWrapper<T>(Func<IDbConnection, Task<Option<T>>> action)
        {
            var connectionString = configuration["ConnectionString:DefaultConnection"];
            await using var connection = new SQLiteConnection(connectionString);

            try
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(ReadScriptFile("CreateInitialTables"));
                
                return await action.Invoke(connection);
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ResetColor();

                var notification = DomainNotification.Create(
                    nameof(T), "Não foi possível executar a consulta"
                );
                await inMemoryBus.RaiseEvent(notification);

                return Option.None<T>();
            }
            finally
            {
                await connection.CloseAsync();
            }
        }
        
        private static string ReadScriptFile(string name)
        {
            var path = Path.Combine(AppContext.BaseDirectory, "Setup", $"{name}.sql");
            using var sr = new StreamReader(path);
            return sr.ReadToEnd();
        }
    }
}