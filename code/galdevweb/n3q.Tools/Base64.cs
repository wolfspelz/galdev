using System;
using System.Text;

namespace n3q.Tools
{
    public static class Base64
    {
        public static string Encode(string s)
        {
            return Encode(Encoding.UTF8.GetBytes(s));
        }

        public static string Decode(string s)
        {
            return Encoding.UTF8.GetString(ToBytes(s));
        }

        public static string Encode(byte[] data)
        {
            return Convert.ToBase64String(data);
        }

        public static byte[] ToBytes(string s)
        {
            return Convert.FromBase64String(s);
        }
    }
}
