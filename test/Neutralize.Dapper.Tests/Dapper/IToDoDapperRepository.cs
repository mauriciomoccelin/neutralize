using Neutralize.Repositories;

namespace Neutralize.Tests.Dapper
{
    public interface IToDoDapperRepository : IDapperRepository<ToDo, int>
    {
    }
}
