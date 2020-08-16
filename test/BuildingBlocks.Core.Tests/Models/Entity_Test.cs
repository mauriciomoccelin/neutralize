using FluentAssertions;
using Xunit;

namespace BuildingBlocks.Tests.Models
{
    public class Entity_Test
    {
        [Fact]
        public void ModelEntityDomainTest()
        {
            // act
            var peopleOne = People.Factory.Create(1, "lorem", new AddressVO("55-85"));
            var peopleTwo = People.Factory.Create(2, "ipsum", new AddressVO("55-85"));

            var productOne = Product.Factory.Create(1, "lorem");
            var productTwo = Product.Factory.Create(1, "ipsum ipsum");

            // Assert
            var equalsPeople = peopleOne.Equals(peopleTwo);
            var equalsAddress = peopleOne.Address.Equals(peopleTwo.Address);

            var equalsProduct = productOne.Equals(productTwo);

            equalsPeople.Should().BeFalse();
            equalsAddress.Should().BeTrue();

            equalsProduct.Should().BeTrue();
        }
    }
}