using BuildingBlocks.Data.EFCore;
using BuildingBlocks.Data.Tests.Entities;

namespace BuildingBlocks.Data.Tests.EFCore
{
    public class TodoRepository : EFCoreRepository<EfCoreDbContext, ToDo, int>, ITodoRepository
    {
        public TodoRepository(EfCoreDbContext efCoreDb) : base(efCoreDb)
        {
        }
    }
}