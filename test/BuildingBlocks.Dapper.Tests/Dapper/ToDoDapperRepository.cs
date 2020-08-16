using System.Data.Common;

namespace BuildingBlocks.Tests.Dapper
{
    public class ToDoDapperRepository : SqLiteDapperRepository, IToDoDapperRepository
    {
        public ToDoDapperRepository(DbConnection connection) : base(connection)
        {
        }
    }
}