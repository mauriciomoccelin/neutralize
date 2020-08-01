using BuildingBlocks.Core.Repositories;
using BuildingBlocks.Test.Entities;

namespace BuildingBlocks.EFCore.Tests
{
    public interface ITodoRepository : IEFCoreRepository<ToDo, int>
    {
    }
}