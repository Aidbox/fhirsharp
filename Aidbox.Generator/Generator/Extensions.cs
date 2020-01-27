using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aidbox.Generator
{
    public static class Extensions
    {
        public static int? ToNullableInt(this JValue item)
        {
            if (item is null)
            {
                return null;
            }

            return int.Parse(item.Value.ToString());
        }

        public static string FUpper(this string s)
        {
            return $"{char.ToUpperInvariant(s[0])}{s.Substring(1)}";
        }
    }
}
