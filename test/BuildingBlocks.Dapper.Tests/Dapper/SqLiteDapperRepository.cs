using System.Data.Common;
using BuildingBlocks.Dapper;

namespace BuildingBlocks.Tests.Dapper
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