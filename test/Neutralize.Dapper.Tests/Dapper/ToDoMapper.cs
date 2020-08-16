using DapperExtensions.Mapper;

namespace Neutralize.Tests.Dapper
{
    /// <summary>
    /// Class form Dapper Extensions mapper queries 
    /// </summary>
    public sealed class ToDoMapper : ClassMapper<ToDo>
    {
        public ToDoMapper()
        {
            Table("ToDos");

            Map(x => x.Id).Key(KeyType.Assigned);
            Map(x => x.Done);
            Map(x => x.Description);
        }
    }
}
