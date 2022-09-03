using System.Data;

namespace Neutralize.Dapper
{
    public interface IDapperConnectionFactory
    {
        IDbConnection CreateOpened();
    }
}
