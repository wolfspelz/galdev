using System;

namespace n3q.Tools
{
    public static class RandomCommon
    {
        public static readonly Random Rnd = new Random();
    }

    public static class RandomString
    {
        const string AlphanumChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";
        const string AlphanumLowercaseChars = "abcdefghijklmnopqrstuvwxyz1234567890";
        const string AlphanumUppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        const string AlphaChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        const string AlphaLowercaseChars = "abcdefghijklmnopqrstuvwxyz";
        const string AlphaUppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        public static string Generate(int len, string src)
        {
            var aChars = new char[len];

            for (int i = 0; i < len; i++) {
                aChars[i] = src[RandomCommon.Rnd.Next(src.Length)];
            }

            return new string(aChars);
        }

        public static string Alphanum(int len) { return Generate(len, AlphanumChars); }
        public static string AlphanumLowercase(int len) { return Generate(len, AlphanumLowercaseChars); }
        public static string AlphanumUppercase(int len) { return Generate(len, AlphanumUppercaseChars); }
        public static string Alpha(int len) { return Generate(len, AlphaChars); }
        public static string AlphaLowercase(int len) { return Generate(len, AlphaLowercaseChars); }
        public static string AlphaUppercase(int len) { return Generate(len, AlphaUppercaseChars); }
    }

    public static class RandomInt
    {
        public static int Between(int min, int max)
        {
            return RandomCommon.Rnd.Next(min, max);
        }
    }

    public static class RandomFloat
    {
        public static double Between(double min, double max)
        {
            return RandomCommon.Rnd.NextDouble() * (max - min) + min;
        }
    }

    public static class RandomExtensions
    {
        public static double NextGaussian(this Random r, double mu = 0, double sigma = 1)
        {
            var u1 = r.NextDouble();
            var u2 = r.NextDouble();

            var rand_std_normal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
            var rand_normal = mu + sigma * rand_std_normal;

            return rand_normal;
        }

        public static double NextTriangular(this Random r, double min = -1, double peak = 0, double max = 1)
        {
            if (max < min) { throw new ArgumentException("max < min"); }
            if (peak < min) { throw new ArgumentException("peak < min"); }
            if (peak > max) { throw new ArgumentException("peak > max"); }

            if (max == min) { return min; }

            var u = r.NextDouble();

            var f = (peak - min) / (max - min);
            if (u < f) { // u > 0 && u < f
                return min + Math.Sqrt(u * (max - min) * (peak - min));
            } else { // u >= f && u < 1
                return max - Math.Sqrt((1 - u) * (max - min) * (max - peak));
            }
        }
    }

}
