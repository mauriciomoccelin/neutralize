using Neutralize.Models;

namespace Neutralize.Tests.Models
{
    public class People : Entity
    {
        public string Name { get; private set; }
        public AddressVO Address { get; private set; }

        public People(
            long id,
            string name,
            AddressVO address
        ) : base(id)
        {
            Id = id;
            Name = name;
            Address = address;
        }

        public void AlterName(string name) => Name = name ?? Name;

        public static class Factory
        {
            public static People Create(
                long id,
                string name,
                AddressVO address
            )
            {

                return new People(id, name, address);
            }
        }
    }
}
