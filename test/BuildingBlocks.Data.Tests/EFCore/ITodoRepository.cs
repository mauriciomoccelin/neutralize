using BuildingBlocks.Core.Repositories;
using BuildingBlocks.Data.Tests.Entities;

namespace BuildingBlocks.Data.Tests.EFCore
{
    public interface ITodoRepository : IEFCoreRepository<ToDo, int>
    {
    }
}