using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Logging;
using Neutralize.Application;
using Optional;

namespace Neutralize.Dapper
{
    public class DapperRepository : IDapperRepository
    {
        private readonly DynamicParameters parameters;
        private readonly ILogger<DapperRepository> logger;
        private readonly IDapperConnectionFactory connectionFactory;

        public DapperRepository(
            ILogger<DapperRepository> logger,
            IDapperConnectionFactory connectionFactory
        )
        {
            parameters = new DynamicParameters();

            this.logger = logger;
            this.connectionFactory = connectionFactory;
        }

        public void Dispose()
        {
        }

        public void AddParameter(string name, object value) => parameters.Add(name, value);

        public async Task<Option<T>> First<T>(string command)
        {
            return await ConnectionWrapper(
                async connection =>
                {
                    var query = await connection.QueryFirstOrDefaultAsync<T>(command, parameters);

                    return query is null ? Option.None<T>() : Option.Some(query);
                }
            );
        }

        public async Task<Option<PagedResultDto<T>>> Paged<T>(string command)
        {
            return await ConnectionWrapper(
                async connection =>
                {
                    var query = await connection.QueryMultipleAsync(command, parameters);

                    var count = query.Read<long>().FirstOrDefault();
                    var items = query.Read<T>().ToList();

                    var page = new PagedResultDto<T>(count, items);
                    return Option.Some(page);
                }
            );
        }

        protected virtual async Task<Option<T>> ConnectionWrapper<T>(Func<IDbConnection, Task<Option<T>>> action)
        {
            var connection = connectionFactory.CreateOpened();

            try
            {
                return await action.Invoke(connection);
            }
            catch (Exception e)
            {
                const string loggerMessage = "Command is faild";
                logger.LogError(e, loggerMessage);

                return Option.None<T>();
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
