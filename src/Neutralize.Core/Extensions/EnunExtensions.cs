using System;
using System.ComponentModel;
using System.Linq;

namespace Neutralize.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value, bool upper = false)
        {
            var fi = value.GetType().GetField(value.ToString());
            var attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            var description = string.Empty;
            if (attributes != null && attributes.Any())
            {
                description = attributes.First().Description;
            }

            if (upper) description = description.ToUpper();

            return description;
        }
    }
}