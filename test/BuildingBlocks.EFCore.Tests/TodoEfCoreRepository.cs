namespace BuildingBlocks.EFCore.Tests
{
    public class TodoEfCoreRepository : EfCoreRepository<EfCoreDbContext, ToDo, int>, ITodoRepository
    {
        public TodoEfCoreRepository(EfCoreDbContext context) : base(context)
        {
        }
    }
}