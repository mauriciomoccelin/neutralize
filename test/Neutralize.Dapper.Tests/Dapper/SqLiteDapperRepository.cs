using System.Data.Common;
using Neutralize.Dapper;

namespace Neutralize.Tests.Dapper
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
