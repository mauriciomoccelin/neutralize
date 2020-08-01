using System.Data.Common;

namespace BuildingBlocks.Dapper.Tests
{
    public class ToDoDapperRepository : SqLiteDapperRepository, IToDoDapperRepository
    {
        public ToDoDapperRepository(DbConnection connection) : base(connection)
        {
        }
    }
}