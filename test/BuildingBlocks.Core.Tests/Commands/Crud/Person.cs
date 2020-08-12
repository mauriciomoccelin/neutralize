using BuildingBlocks.Core.Models;

namespace BuildingBlocks.Core.Tests.Commands.Crud
{
    public class Person : Entity<Person, long>
    {
        public string Name { get; private set; }

        public void ChangeName(string newName) => Name = newName;
    }
}