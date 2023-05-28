#nullable enable
using System;
using System.Text;

namespace n3q.Tools
{
    public enum Endianness {
        LittleEndian,
        BigEndian,
    }

    public class BytesParser
    {
        protected readonly byte[] Bytes;
        protected readonly int BytesLength; 
        protected readonly Endianness Endianness;
        protected readonly bool ReverseNumBytes;
        protected int BaseOffset;

        public BytesParser(byte[] bytes, Endianness endianness, int offset=0, int? count=null)
        {
            Bytes = bytes;
            BytesLength = Bytes.Length;
            BaseOffset = offset;
            var countMax = GetLength();
            var countM = count ?? countMax;
            CheckLength(0, countM);
            BytesLength = BaseOffset + countM;
            Endianness = endianness;
            var isBigEndian = endianness == Endianness.BigEndian;
            ReverseNumBytes = BitConverter.IsLittleEndian && isBigEndian;
        }

        public bool IsEmpty(int offset=0) {
            return GetLength(offset) == 0;
        }

        // Retrieves the remaining byte count from offset.
        public int GetLength(int offset=0) {
            return Math.Max(0, BytesLength - BaseOffset - offset);
        }

        public void CheckLength(int offset, int count) {
            if (GetLength(offset) < count) {
                var msg = $"Buffer length: {BytesLength}, BaseOffset: {BaseOffset}, offset: {offset}, count: {count}!";
                throw new IndexOutOfRangeException(msg);
            }
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Non-consuming Parsing:

        public byte[] GetBytes(int offset, int count, bool reverse=false) {
            CheckLength(offset, count);
            int start = BaseOffset + offset;
            int step = 1;
            if (reverse) {
                (start, step) = (start + count - 1, -1);
            }
            int sourceI = start;
            var bytes = new byte[count];
            for (int resultI = 0; resultI < count; resultI++) {
                bytes[resultI] = Bytes[sourceI];
                sourceI += step;
            }
            return bytes;
        }

        public BytesParser GetBytesParser(int offset, int count) {
            CheckLength(offset, count);
            return new BytesParser(Bytes, Endianness, BaseOffset + offset, count);
        }

        public int GetUInt32(int offset) {
            var valBytes = GetBytes(offset, 4, ReverseNumBytes);
            return Convert.ToInt32(BitConverter.ToUInt32(valBytes));
        }

        public int GetUInt24(int offset) {
            var resultBytes = new byte[]{0, 0, 0, 0};
            var valBytes = GetBytes(offset, 3, ReverseNumBytes);
            var start = BitConverter.IsLittleEndian ? 0 : 1;
            resultBytes[start] = valBytes[0];
            resultBytes[start + 1] = valBytes[1];
            resultBytes[start + 2] = valBytes[2];
            return BitConverter.ToInt32(valBytes);
        }

        public int GetUInt16(int offset) {
            var valBytes = GetBytes(offset, 2, ReverseNumBytes);
            return Convert.ToInt32(BitConverter.ToUInt16(valBytes));
        }

        public int GetUInt8(int offset) {
            CheckLength(offset, 1);
            return Convert.ToInt32(Bytes[BaseOffset + offset]);
        }

        public bool GetBit(int offset, int bit) {
            return (GetUInt8(offset) & (1 << bit)) != 0;
        }

        public string GetAsciiString(int offset, int count) {
            return ASCIIEncoding.ASCII.GetString(GetBytes(offset, count));
        }

        // - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
        // Cconsuming Parsing:

        public void Skip(int count) {
            BaseOffset += count;
        }

        public byte[] GetBytes(int count, bool reverse=false) {
            var result = GetBytes(0, count, reverse);
            Skip(count);
            return result;
        }

        public BytesParser NextBytesParser(int count, bool reverse=false) {
            var result = GetBytesParser(0, count);
            Skip(count);
            return result;
        }

        public int NextUInt32() {
            var result = GetUInt32(0);
            Skip(4);
            return result;
        }

        public int NextUInt24() {
            var result = GetUInt24(0);
            Skip(3);
            return result;
        }

        public int NextUInt16() {
            var result = GetUInt16(0);
            Skip(2);
            return result;
        }

        public int NextUInt8() {
            var result = GetUInt8(0);
            Skip(1);
            return result;
        }

        public string NextAsciiString(int count) {
            var result = GetAsciiString(0, count);
            Skip(count);
            return result;
        }

    }

}
