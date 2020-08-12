using BuildingBlocks.Core.Repositories;

namespace BuildingBlocks.Dapper.Tests
{
    public interface IToDoDapperRepository : IDapperRepository<ToDo, int>
    {
    }
}