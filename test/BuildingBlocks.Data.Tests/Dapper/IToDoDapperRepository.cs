using BuildingBlocks.Core.Repositories;
using BuildingBlocks.Data.Tests.Entities;

namespace BuildingBlocks.Data.Tests.Dapper
{
    public interface IToDoDapperRepository : IDapperRepository<ToDo, int>
    {
    }
}