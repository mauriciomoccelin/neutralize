using System;
using Neutralize.Models;

namespace Neutralize.Tests.Models
{
    public class People : AggregateRoot
    {
        public string Name { get; private set; }
        public AddressVO Address { get; private set; }

        protected People() { }
        public People(
            long id,
            string name,
            AddressVO address
        ) : base(Guid.NewGuid())
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
