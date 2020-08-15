using System.Collections.Generic;
using BuildingBlocks.Core.Models;

namespace BuildingBlocks.Core.Tests.Models
{
    public class AddressVO : ValueObject
    {
        public string ZipCode { get; }

        internal AddressVO(string zipCode) { ZipCode = zipCode; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return ZipCode;
        }
    }
}