using System.Text;
using Force.Crc32;

namespace n3q.Tools
{
    public static class Crc32
    {
        public static uint Compute(string data)
        {
            var bytes = Encoding.UTF8.GetBytes(data);
            var crc = Crc32Algorithm.Compute(bytes);
            return crc;
        }

        public static double ToDouble(string data)
        {
            var crc = Compute(data);
            var d = (double)crc / uint.MaxValue;
            return d;
        }
    }
}
