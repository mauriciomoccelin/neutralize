namespace Neutralize.EFCore.Tests.Dapper
{
    public class TodoEfCoreRepository : EfCoreRepository<EfCoreDbContext, ToDo, long>, ITodoRepository
    {
        public TodoEfCoreRepository(EfCoreDbContext context) : base(context)
        {
        }
    }
}
