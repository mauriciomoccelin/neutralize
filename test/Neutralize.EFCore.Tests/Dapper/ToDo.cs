using Neutralize.Models;

namespace Neutralize.EFCore.Tests.Dapper
{
    public class ToDo : Entity<ToDo, int>
    {
        public bool Done { get; private set; }
        public string Description { get; private set; }

        protected ToDo() { }
        
        public ToDo(int id, bool done, string description)
        {
            Id = id;
            Done = done;
            Description = description;
        }

        public void MarkAsDone() => Done = true;
        public void ChangeDescription(string newDescription) => Description = newDescription;
    }
}
