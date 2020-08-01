using BuildingBlocks.Core.Repositories;
using BuildingBlocks.Test.Entities;

namespace BuildingBlocks.Dapper.Tests
{
    public interface IToDoDapperRepository : IDapperRepository<ToDo, int>
    {
    }
}