using BuildingBlocks.Domain.Models;

namespace BuildingBlocks.Domain.Tests.Models
{
    public class Product : Entity<People, long>
    {
        public string Name { get; private set; }

        public Product(
            long id,
            string name
        ) : base(id)
        {
            Name = name;
        }

        public static class Factory
        {
            public static Product Create(
                long id,
                string name
            )
            {

                return new Product(id, name);
            }
        }
    }
}