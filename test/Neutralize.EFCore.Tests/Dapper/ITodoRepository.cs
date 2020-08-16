using Neutralize.Repositories;

namespace Neutralize.EFCore.Tests.Dapper
{
    public interface ITodoRepository : IRepository<ToDo, long>
    {
    }
}
