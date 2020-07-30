using System.Data.Common;

namespace BuildingBlocks.Data.Tests.Dapper
{
    public class ToDoDapperRepository : SqLiteDapperRepository, IToDoDapperRepository
    {
        public ToDoDapperRepository(DbConnection connection) : base(connection)
        {
            connection.Open();
        }
    }
}