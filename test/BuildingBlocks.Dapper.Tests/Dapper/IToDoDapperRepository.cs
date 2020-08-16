using BuildingBlocks.Repositories;

namespace BuildingBlocks.Tests.Dapper
{
    public interface IToDoDapperRepository : IDapperRepository<ToDo, int>
    {
    }
}