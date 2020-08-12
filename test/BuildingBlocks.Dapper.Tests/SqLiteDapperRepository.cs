using System.Data.Common;

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