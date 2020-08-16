using System.Collections.Generic;
using Neutralize.Models;

namespace Neutralize.Tests.Models
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
