using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace galdevtool
{
    public static class StringExtensions
    {
        public static int ToInteger(this string self)
        {
            int.TryParse(self, out var value);
            return value;
        }

        public static bool IsInteger(this string self)
        {
            return int.TryParse(self, out _);
        }

        public static T ToEnum<T>(this string self, T defaultValue) where T : struct
        {
            if (!Enum.TryParse<T>(self, out var value))
            {
                value = defaultValue;
            }
            return value;
        }

        public static bool IsTrue(this string self)
        {
            string s = self.ToLower();
            return s == "true" || s == "1" || s == "on" || s == "yes" || s == "ja" || s == "oui" || s == "ok" || s == "sure" || s == "yessir" || s == "youbet";
        }

        public static string Truncate(this string self, int nLen, string tail = "...")
        {
            if (self.Length <= nLen)
            { return self; }

            int nMax = nLen - tail.Length;
            nMax = nMax < 0 ? 0 : nMax;
            return self.Substring(0, nMax) + tail;
        }

        public static string IfEmpty(this string self, string defaultValue)
        {
            if (string.IsNullOrEmpty(self))
            {
                return defaultValue;
            }
            return self;
        }

        public static List<string> RegexTokens(this string self, string regEx)
        {
            var tokens = new List<string>();

            var re = new Regex(regEx);
            var match = re.Match(self);
            if (match.Success)
            {
                for (int i = 1; i < match.Groups.Count; i++)
                {
                    tokens.Add(match.Groups[i].Value);
                }
            }

            return tokens;
        }

        // ReSharper disable once InconsistentNaming
        public static string EscapeLF(this string self)
        {
            var pairs = new List<KeyValuePair<string, string>> {
                new KeyValuePair<string, string>("\r", "\\r"),
                new KeyValuePair<string, string>("\n", "\\n"),
                new KeyValuePair<string, string>("\\", "\\\\")
            };

            var sb = new StringBuilder();
            for (var i = 0; i < self.Length; i++)
            {
                var anyFound = false;
                foreach (var replace in pairs.Where(pair => string.Compare(self, i, pair.Key, 0, pair.Key.Length) == 0))
                {
                    i += replace.Key.Length - 1;
                    sb.Append(replace.Value);
                    anyFound = true;
                    break;
                }
                if (!anyFound)
                {
                    sb.Append(self[i]);
                }
            }

            return sb.ToString();
        }

    }
}
