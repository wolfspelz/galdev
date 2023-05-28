using System.IO;
using System.IO.Compression;
using System.Text;

namespace n3q.Tools
{
    public static class Compression
    {
        public static byte[] GZipCompress(string plainText)
        {
            var bytes = Encoding.UTF8.GetBytes(plainText);
            return GZipCompress(bytes);
        }

        public static byte[] GZipCompress(byte[] plainBytes)
        {
            using var msi = new MemoryStream(plainBytes);
            using var mso = new MemoryStream();
            using (var gs = new GZipStream(mso, CompressionMode.Compress)) {
                CopyTo(msi, gs);
            }

            return mso.ToArray();
        }

        public static void CopyTo(Stream src, Stream dest)
        {
            byte[] bytes = new byte[4096];

            int cnt;

            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0) {
                dest.Write(bytes, 0, cnt);
            }
        }

        public static string GZipDecompressToString(byte[] compressedBytes)
        {
            return Encoding.UTF8.GetString(GZipDecompress(compressedBytes));
        }

        public static byte[] GZipDecompress(byte[] compressedBytes)
        {
            using var msi = new MemoryStream(compressedBytes);
            using var mso = new MemoryStream();
            using (var gs = new GZipStream(msi, CompressionMode.Decompress)) {
                CopyTo(gs, mso);
            }

            return mso.ToArray();
        }
    }
}
