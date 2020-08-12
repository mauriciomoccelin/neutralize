using BuildingBlocks.Core.Repositories;

namespace BuildingBlocks.EFCore.Tests
{
    public interface ITodoRepository : IEFCoreRepository<ToDo, int>
    {
    }
}