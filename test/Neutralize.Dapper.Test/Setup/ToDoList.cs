namespace Neutralize.Dapper.Test.Setup
{
    public class ToDoList
    {
        public int Id { get; private set; }
        public bool Done { get; private  set; }
        public string Description { get; private set; }
        
        public ToDoList()
        {
            Id = default;
            Done = default;
            Description = string.Empty;
        }
    }
}