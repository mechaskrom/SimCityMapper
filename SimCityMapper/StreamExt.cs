using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace SimCityMapper
{
    static class StreamExt
    {
        private static byte[] mBufferInt = new byte[8];
        private static char[] mBufferChars = new char[64];

        public static byte readUInt8(this Stream s)
        {
            return (byte)s.ReadByte();
        }

        public static UInt16 readUInt16(this Stream s)
        {
            s.Read(mBufferInt, 0, 2);
            return (UInt16)(
                (mBufferInt[0] << 0) |
                (mBufferInt[1] << 8));
        }

        public static UInt32 readUInt24(this Stream s) //Read and convert 3 bytes to UInt32.
        {
            s.Read(mBufferInt, 0, 3);
            return (UInt32)(
                (mBufferInt[0] << 0) |
                (mBufferInt[1] << 8) |
                (mBufferInt[2] << 16));
        }

        public static UInt32 readUInt32(this Stream s)
        {
            s.Read(mBufferInt, 0, 4);
            return (UInt32)(
                (mBufferInt[0] << 0) |
                (mBufferInt[1] << 8) |
                (mBufferInt[2] << 16) |
                (mBufferInt[3] << 24));
        }

        public static UInt64 readUInt64(this Stream s)
        {
            s.Read(mBufferInt, 0, 8);
            return (UInt64)(
                (mBufferInt[0] << 0) |
                (mBufferInt[1] << 8) |
                (mBufferInt[2] << 16) |
                (mBufferInt[3] << 24) |
                (mBufferInt[4] << 32) |
                (mBufferInt[5] << 40) |
                (mBufferInt[6] << 48) |
                (mBufferInt[7] << 56));
        }

        public static void writeUInt8(this Stream s, byte val)
        {
            s.WriteByte(val);
        }

        public static void writeUInt16(this Stream s, UInt16 val)
        {
            mBufferInt[0] = (byte)(val >> 0);
            mBufferInt[1] = (byte)(val >> 8);
            s.Write(mBufferInt, 0, 2);
        }

        public static void writeUInt24(this Stream s, UInt32 val)
        {
            mBufferInt[0] = (byte)(val >> 0);
            mBufferInt[1] = (byte)(val >> 8);
            mBufferInt[2] = (byte)(val >> 16);
            s.Write(mBufferInt, 0, 3);
        }

        public static void writeUInt32(this Stream s, UInt32 val)
        {
            mBufferInt[0] = (byte)(val >> 0);
            mBufferInt[1] = (byte)(val >> 8);
            mBufferInt[2] = (byte)(val >> 16);
            mBufferInt[3] = (byte)(val >> 24);
            s.Write(mBufferInt, 0, 4);
        }

        public static void writeUInt64(this Stream s, UInt64 val)
        {
            mBufferInt[0] = (byte)(val >> 0);
            mBufferInt[1] = (byte)(val >> 8);
            mBufferInt[2] = (byte)(val >> 16);
            mBufferInt[3] = (byte)(val >> 24);
            mBufferInt[4] = (byte)(val >> 32);
            mBufferInt[5] = (byte)(val >> 40);
            mBufferInt[6] = (byte)(val >> 48);
            mBufferInt[7] = (byte)(val >> 56);
            s.Write(mBufferInt, 0, 8);
        }

        public static byte[] readArray(this Stream s, long length)
        {
            return readArrayInner(s, null, length);
        }

        public static void readArray(this Stream s, byte[] array)
        {
            readArrayInner(s, array, array.LongLength);
        }

        private static byte[] readArrayInner(this Stream s, byte[] array, long length)
        {
            if ((s.Position + length) > s.Length)
            {
                throw new ArgumentException("Stream is too short to read '" + length + "' bytes from its current position!");
            }

            if (array == null) //Create array if no one was provided.
            {
                array = new byte[length];
            }

            if (length <= int.MaxValue) //Can use Stream.Read?
            {
                s.Read(array, 0, array.Length);
            }
            else
            {
                for (long i = 0; i < length; i++)
                {
                    array[i] = s.readUInt8();
                }
            }
            return array;
        }

        public static void writeArray(this Stream s, byte[] array)
        {
            long length = array.LongLength;
            if (length <= int.MaxValue)
            {
                s.Write(array, 0, array.Length);
            }
            else
            {
                for (long i = 0; i < length; i++)
                {
                    s.writeUInt8(array[i]);
                }
            }
        }

        //A bit slower than the char array version below, but simpler/safer?
        //private static List<char> mListChars = new List<char>();
        //public static string readString(this Stream s) //Read bytes until null or end of stream.
        //{
        //    mListChars.Clear();
        //    for (int b = s.ReadByte(); b > 0; b = s.ReadByte()) //Read until null byte or end of stream.
        //    {
        //        mListChars.Add((char)b);
        //    }
        //    return new string(mListChars.ToArray());
        //}

        public static string readString(this Stream s) //Read bytes until null or end of stream.
        {
            char[] chars = mBufferChars; //Start with pre-allocated buffer.
            int charCount = 0;
            for (int b = s.ReadByte(); b > 0; b = s.ReadByte()) //Read until null byte or end of stream.
            {
                if (charCount >= chars.Length) //Need to temporarily expand chars buffer?
                {
                    char[] oldChars = chars;
                    chars = new char[oldChars.Length * 2];
                    Array.Copy(oldChars, chars, oldChars.Length);
                }
                chars[charCount++] = (char)b;
            }
            return new string(chars, 0, charCount); //Returns "" if count is 0.
        }

        public static string readString(this Stream s, long count) //Read count bytes until null or end of stream.
        {
            char[] chars = mBufferChars; //Start with pre-allocated buffer.
            int charCount = 0;
            for (int b = s.ReadByte(); b > 0; b = s.ReadByte()) //Read until null byte or end of stream.
            {
                if (charCount >= chars.Length) //Need to temporarily expand chars buffer?
                {
                    char[] oldChars = chars;
                    chars = new char[oldChars.Length * 2];
                    Array.Copy(oldChars, chars, oldChars.Length);
                }
                chars[charCount++] = (char)b;
                if (charCount >= count)
                {
                    break;
                }
            }
            return new string(chars, 0, charCount); //Returns "" if count is 0.
        }

        public static void writeString(this Stream s, string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            s.Write(bytes, 0, bytes.Length);
        }

        public static void writeStringLine(this Stream s, string str)
        {
            s.writeString(str + Environment.NewLine);
        }

        public static long copyTo(this Stream src, Stream dst) //Return total bytes copied.
        {
            byte[] buffer = new byte[8192];
            long total = 0;
            int read = 0;
            while ((read = src.Read(buffer, 0, buffer.Length)) > 0)
            {
                dst.Write(buffer, 0, read);
                total += read;
            }
            return total;
        }

        public static long copyTo(this Stream src, Stream dst, long length) //Return total bytes copied. May be less than requested length.
        {
            byte[] buffer = new byte[8192];
            long total = 0;
            int read = (int)Math.Min(buffer.Length, length);
            while (length > 0 && (read = src.Read(buffer, 0, read)) > 0)
            {
                dst.Write(buffer, 0, read);
                total += read;
                length -= read;
                read = (int)Math.Min(buffer.Length, length);
            }
            return total;
        }

        public static MemoryStream getCompressed(this Stream src)
        {
            MemoryStream memoryStream = new MemoryStream();
            using (GZipStream compressionStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
            {
                src.Seek(0, SeekOrigin.Begin);
                src.copyTo(compressionStream);
            }
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

        public static MemoryStream getDecompressed(this Stream src)
        {
            MemoryStream memoryStream = new MemoryStream();
            using (GZipStream decompressionStream = new GZipStream(src, CompressionMode.Decompress, true))
            {
                src.Seek(0, SeekOrigin.Begin);
                decompressionStream.copyTo(memoryStream);
            }
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

        public static void saveCompressed(this Stream originalStream, string filePath)
        {
            using (FileStream fileStream = File.Create(filePath))
            {
                using (GZipStream compressionStream = new GZipStream(fileStream, CompressionMode.Compress, true))
                {
                    originalStream.copyTo(compressionStream);
                }
            }
        }

        public static byte[] decompressFileToBytes(this string filePath)
        {
            using (FileStream fileStream = File.OpenRead(filePath))
            {
                using (MemoryStream memoryStream = fileStream.getDecompressed())
                {
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    return memoryStream.ToArray();
                }
            }
        }
    }
}
