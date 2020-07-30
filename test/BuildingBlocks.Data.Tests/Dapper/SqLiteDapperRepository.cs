using System.Data.Common;
using BuildingBlocks.Data.Dapper;
using BuildingBlocks.Data.Tests.Entities;

namespace BuildingBlocks.Data.Tests.Dapper
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