using BuildingBlocks.Repositories;

namespace BuildingBlocks.EFCore.Tests.Dapper
{
    public interface ITodoRepository : IRepository<ToDo, int>
    {
    }
}