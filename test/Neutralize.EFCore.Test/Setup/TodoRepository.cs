namespace Neutralize.EFCore.Test.Setup
{
    public class TodoRepository : EfCoreRepository<TodoDbContext, ToDo, int>
    {
        public TodoRepository(TodoDbContext context) : base(context)
        {
        }
    }
}