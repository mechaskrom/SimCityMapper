using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SimCityMapper
{
    class StateFileSnes9x : StateFile
    {
        private const string ChunkNameRam = "RAM";
        private const string ChunkNameVram = "VRA";
        private const string ChunkNameSram = "SRA";

        private readonly string mSignature;
        private readonly string mVersion;
        private readonly List<Chunk> mChunks;

        //CGRAM position in 'PPU' chunk data.
        //"#!snes9x:0004\n" = @0x3A, stored in big endian for some reason. SNES uses little endian.

        private StateFileSnes9x(string signature, string version, List<Chunk> chunks, byte[] ram, byte[] vram)
            : base(ram, vram)
        {
            mSignature = signature;
            mVersion = version;
            mChunks = chunks;
        }

        public static StateFileSnes9x open(string statePath)
        {
            using (FileStream fs = File.OpenRead(statePath))
            {
                return open(fs);
            }
        }

        public static StateFileSnes9x open(Stream stream)
        {
            Stream ds = getDecompressed(stream);

            //Check signature and version in header.
            string signature = ds.readString(9);
            if (signature != "#!snes9x:" && signature != "#!s9xsnp:")
            {
                throw new InvalidDataException("Snes9x signature not present!");
            }
            string version = ds.readString(5);
            if (version != "0001\n" && version != "0004\n" && version != "0011\n" && version != "1430\n")
            {
                throw new InvalidDataException("Snes9x version not supported!");
            }

            //Read chunks.
            List<Chunk> chunks = new List<Chunk>();
            byte[] ram = null;
            byte[] vram = null;
            for (Chunk chunk = Chunk.read(ds); chunk != null; chunk = Chunk.read(ds)) //Keep reading until end.
            {
                if (chunk.Name == ChunkNameRam)
                {
                    if (chunk.Data.Length != RamLength)
                    {
                        throw new InvalidDataException("Snes9x RAM chunk has an unexpected data length!");
                    }
                    ram = chunk.Data;
                }
                else if (chunk.Name == ChunkNameVram)
                {
                    if (chunk.Data.Length != VramLength)
                    {
                        throw new InvalidDataException("Snes9x VRAM chunk has an unexpected data length!");
                    }
                    vram = chunk.Data;
                }
                chunks.Add(chunk);
            }
            if (ram == null)
            {
                throw new InvalidDataException("Snes9x RAM chunk not present!");
            }
            if (vram == null)
            {
                throw new InvalidDataException("Snes9x VRAM chunk not present!");
            }

            return new StateFileSnes9x(signature, version, chunks, ram, vram);
        }

        public override byte[] findSram()
        {
            Chunk chunk = mChunks.Find((Chunk c) => c.Name == ChunkNameSram);
            return chunk != null ? chunk.Data : null;
        }

        public override void save(string savePath)
        {
            MemoryStream ms = new MemoryStream();
            ms.Seek(0, SeekOrigin.Begin);

            //Write header.
            ms.writeString(mSignature);
            ms.writeString(mVersion);

            //Write chunks.
            foreach (Chunk chunk in mChunks)
            {
                chunk.write(ms);
            }

            //Save whole stream to a compressed file.
            ms.Seek(0, SeekOrigin.Begin);
            ms.saveCompressed(savePath);
        }

        private static Stream getDecompressed(Stream stream)
        {
            //If stream starts with 0x1F, 0x8B, then it's probably gzip compressed.
            UInt16 magicNumber = stream.readUInt16();
            if (magicNumber == 0x8B1F)
            {
                return stream.getDecompressed();
            }
            stream.Seek(-2, SeekOrigin.Current); //Undo the magic number read.
            return stream;
        }

        private class Chunk
        {
            //Name          :    Array[3] of Char
            //Separator1    :    Char ':'
            //Length        :    Array[6] of Char
            //Separator2    :    Char ':'
            //Data          :    Array[Length] of Byte

            private const byte Separator = (byte)':';

            private readonly string mName;
            private readonly byte[] mData;

            private Chunk(string name, byte[] data)
            {
                mName = name;
                mData = data;
            }

            public static Chunk read(Stream stream)
            {
                string name = stream.readString(3);
                if (name == "") //End of stream?
                {
                    return null;
                }
                stream.ReadByte(); //Skip separator1.
                int length = int.Parse(stream.readString(6));
                stream.ReadByte(); //Skip separator2.
                byte[] data = stream.readArray(length);
                return new Chunk(name, data);
            }

            public void write(Stream stream)
            {
                stream.writeString(mName); //Name.
                stream.WriteByte(Separator); //Separator1.
                stream.writeString(mData.Length.ToString("D6")); //Length.
                stream.WriteByte(Separator); //Separator2.
                stream.writeArray(mData); //Data.
            }

            public string Name
            {
                get { return mName; }
            }

            public byte[] Data
            {
                get { return mData; }
            }
        }
    }
}
