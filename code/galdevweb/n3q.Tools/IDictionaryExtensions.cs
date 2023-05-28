using System;
using System.Collections.Generic;
using System.Linq;

namespace n3q.Tools
{
    public static class IDictionaryExtensions
    {
        public static string ToString<A, B>(this Dictionary<A, B> self, string lineSeparator = " ", string fieldSeparator = "=")
        {
            return string.Join(lineSeparator, self.Select(kv => $"{kv.Key}{fieldSeparator}{kv.Value}"));
        }
    }
}
