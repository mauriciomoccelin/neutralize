using Neutralize.Models;

namespace Neutralize.Tests.Models
{
    public class Product : Entity
    {
        public string Name { get; private set; }

        protected Product() { }
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
