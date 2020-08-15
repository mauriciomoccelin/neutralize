namespace BuildingBlocks.EFCore.Tests
{
    public class TodoRepository : Repository<EfCoreDbContext, ToDo, int>, ITodoRepository
    {
        public TodoRepository(EfCoreDbContext context) : base(context)
        {
        }
    }
}