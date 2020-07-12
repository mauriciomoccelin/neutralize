using System;
using BuildingBlocks.Domain.Models;

namespace BuildingBlocks.Domain.Tests.Models
{
    public class AddressVO : ValueObject<AddressVO>
    {
        public string ZipCode { get; }

        internal AddressVO(string zipCode) { ZipCode = zipCode; }
        
        protected override bool EqualsCore(
            AddressVO other
        )
        {
            return other != null && string.Equals(other.ZipCode, ZipCode);
        }

        protected override int GetHashCodeCore() { return HashCode.Combine(ZipCode); }
    }
}