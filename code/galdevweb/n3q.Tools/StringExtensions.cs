using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

namespace n3q.Tools
{
    public static class StringExtensions
    {
        public static T ToEnum<T>(this string self, T defaultValue) where T : struct
        {
            if (!Enum.TryParse<T>(self, out var value)) {
                value = defaultValue;
            }
            return value;
        }

        public static T ToEnum<T>(this string self) where T : struct
        {
            if (!Enum.TryParse<T>(self, out var value)) {
                throw new Exception($"Enum {typeof(T).Name}: no such value: {self}");
            }
            return value;
        }

        public static long ToLong(this string self)
        {
            if (long.TryParse(self, NumberStyles.Any, CultureInfo.InvariantCulture, out long value)) {
                return value;
            }
            return 0L;
        }

        public static double ToDouble(this string self)
        {
            if (double.TryParse(self, NumberStyles.Any, CultureInfo.InvariantCulture, out double value)) {
                return value;
            }
            return 0.0D;
        }

        public static bool IsTrue(this string self)
        {
            string s = self.ToLower(CultureInfo.InvariantCulture);
            return s == "true" || s == "1" || s == "on" || s == "yes" || s == "y" || s == "ja" || s == "oui" || s == "ok" || s == "sure" || s == "yessir" || s == "youbet";
        }

        public static string Truncate(this string self, int nLen, string tail = "...")
        {
            if (self.Length <= nLen) { return self; }

            int nMax = nLen - tail.Length;
            nMax = nMax < 0 ? 0 : nMax;
            return self.Substring(0, nMax) + tail;
        }

        public static string ToSingleLine(this string self)
        {
            return self.Replace("\r\n", "\n").Replace("\n", "\\n");
        }

        public static string Quotes(this string self)
        {
            return self.Replace("'", "\"");
        }

        public static int SimpleHash(this string self)
        {
            var s = "abcd" + self;

            var hash = 0;
            for (var i = 0; i < s.Length; i++) {
                var c = s[i];
                hash <<= 5;
                hash ^= c << 16 | c;
            }

            return Math.Abs(hash);
        }

        public static string IfEmpty(this string self, string defaultValue)
        {
            if (string.IsNullOrEmpty(self)) {
                return defaultValue;
            }
            return self;
        }

        public static string Capitalize(this string self)
        {
            if (string.IsNullOrEmpty(self)) { return ""; }
            var s = self.Substring(0, 1).ToUpperInvariant() + self.Substring(1);
            return s;
        }

        public static string CamelCase(this string self)
        {
            if (string.IsNullOrEmpty(self)) { return ""; }
            var s = self.Substring(0, 1).ToLowerInvariant() + self.Substring(1);
            return s;
        }

        public static List<string> RegexTokens(this string self, string regEx)
        {
            var tokens = new List<string>();

            var re = new Regex(regEx);
            var match = re.Match(self);
            if (match.Success) {
                for (int i = 1; i < match.Groups.Count; i++) {
                    tokens.Add(match.Groups[i].Value);
                }
            }

            return tokens;
        }

        public static Match Match(this string self, string regex)
        {
            return new Regex(regex).Match(self);
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
            for (var i = 0; i < self.Length; i++) {
                var anyFound = false;
                foreach (var replace in pairs.Where(pair => string.Compare(self, i, pair.Key, 0, pair.Key.Length) == 0)) {
                    i += replace.Key.Length - 1;
                    sb.Append(replace.Value);
                    anyFound = true;
                    break;
                }
                if (!anyFound) {
                    sb.Append(self[i]);
                }
            }

            return sb.ToString();
        }

        public static string ToBase64(this string self)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(self));
        }

        public static string FromBase64(this string self)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(self));
        }

        public static string XmlEncode(this string self, bool isAttribute = false)
        {
            var sb = new StringBuilder(self.Length);

            foreach (var c in self) {
                if (c == '<') {
                    sb.Append("&lt;");
                } else if (c == '>') {
                    sb.Append("&gt;");
                } else if (c == '&') {
                    sb.Append("&amp;");
                } else if (isAttribute && c == '\"') {
                    sb.Append("&quot;");
                } else if (isAttribute && c == '\'') {
                    sb.Append("&apos;");
                } else if (c == '\n') {
                    sb.Append(isAttribute ? "&#xA;" : "\n");
                } else if (c == '\r') {
                    sb.Append(isAttribute ? "&#xD;" : "\r");
                } else if (c == '\t') {
                    sb.Append(isAttribute ? "&#x9;" : "\t");
                } else {
                    //if (chr < 32) { throw new InvalidOperationException("Invalid character in Xml String. Chr " + Convert.ToInt16(chr) + " is illegal.");}
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }
    }
}
