using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Neutralize.Bus;
using Neutralize.Notifications;
using Optional;
using Microsoft.Extensions.Configuration;

namespace Neutralize.Dapper
{
    public abstract class DapperMSSQLRepository : DaperRepository
    {
        protected readonly IInMemoryBus inMemoryBus;
        protected readonly IConfiguration configuration;
        
        protected DapperMSSQLRepository(
            IInMemoryBus inMemoryBus,
            IConfiguration configuration
        )
        {
            this.inMemoryBus = inMemoryBus;
            this.configuration = configuration;
        }

        /// <summary>
        /// Opens a connection to the database and waits for a return query
        /// If it fails add an error to the bus
        /// </summary>
        /// <param name="action"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected override async Task<Option<T>> ConnectionWrapper<T>(Func<IDbConnection, Task<Option<T>>> action)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            await using var connection = new SqlConnection(connectionString);

            try
            {
                await connection.OpenAsync();
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
    }
}