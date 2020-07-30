using System.ComponentModel.DataAnnotations.Schema;
using BuildingBlocks.Core.Models;

namespace BuildingBlocks.Data.Tests.Entities
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
    }
}