using BuildingBlocks.Core.Repositories;

namespace BuildingBlocks.EFCore.Tests
{
    public interface ITodoRepository : IRepository<ToDo, int>
    {
    }
}