using System.Data.Common;

namespace Neutralize.Tests.Dapper
{
    public class ToDoDapperRepository : SqLiteDapperRepository, IToDoDapperRepository
    {
        public ToDoDapperRepository(DbConnection connection) : base(connection)
        {
        }
    }
}
