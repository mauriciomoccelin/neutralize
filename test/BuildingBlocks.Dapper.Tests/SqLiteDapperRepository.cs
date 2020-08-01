using System.Data.Common;
using BuildingBlocks.Test.Entities;

namespace BuildingBlocks.Dapper.Tests
{
    public abstract class SqLiteDapperRepository : DapperRepository<ToDo, int>
    {
        protected SqLiteDapperRepository(
            DbConnection connection
        ) : base(connection)
        {
        }
    }
}