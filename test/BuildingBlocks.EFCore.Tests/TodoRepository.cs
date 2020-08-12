namespace BuildingBlocks.EFCore.Tests
{
    public class TodoRepository : EFCoreRepository<EfCoreDbContext, ToDo, int>, ITodoRepository
    {
        public TodoRepository(EfCoreDbContext context) : base(context)
        {
        }
    }
}