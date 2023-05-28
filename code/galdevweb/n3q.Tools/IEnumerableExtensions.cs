using System;
using System.Collections.Generic;
using System.Linq;

namespace n3q.Tools
{
    public static class IEnumerableExtensions
    {
        public static Dictionary<string, string> ToStringDictionary<TValue>(
            this IEnumerable<KeyValuePair<string, TValue>> self,
            Func<TValue, string> valueMapper
            )
        {
            return self
                .Select(kv => new KeyValuePair<string, string>(kv.Key, valueMapper(kv.Value)))
                .ToDictionary(kv => kv.Key, kv => kv.Value)
                ;
        }
    }
}
