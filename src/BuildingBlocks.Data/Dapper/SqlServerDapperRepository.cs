using System;
using System.Data;
using System.Threading.Tasks;
using BuildingBlocks.Core.Repositories;
using Dapper;

namespace BuildingBlocks.Data.Dapper
{
    public abstract class  SqlServerDapperRepository : IDapperRepository<DynamicParameters> 
    {
        public IDbConnection Connection { get; }
        public DynamicParameters Parameters { get; }

        protected SqlServerDapperRepository(IDbConnection connection)
        {
            Connection = connection;
            Parameters = new DynamicParameters();
        }
        
        public void AddParameter(string name, object value) => Parameters.Add(name, value);

        public Task<T> ConnectionWrapper<T>(Func<IDbConnection, Task<T>> func)
        {
            try
            {
                return func.Invoke(Connection);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return default;
            }
        }
    }
}