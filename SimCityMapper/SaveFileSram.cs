using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SimCityMapper
{
    //The battery backup 32KB RAM inside the SimCity cartridge. Commonly called S-RAM/SRAM (Save RAM?).
    //Implementation based on Adam Croke's "SNES SimCity Save RAM Guide" and usrshare's "snescityeditor".
    //https://gamefaqs.gamespot.com/snes/588657-simcity/faqs/73744
    //https://github.com/usrshare/snescityeditor
    class SaveFileSram : SaveFile
    {
        private const int SramLength = 1 << 15;
        private const int HeaderLength = 16;
        private const int SlotLength = (1 << 14) - 16;
        private const int Slot1Start = HeaderLength;
        private const int Slot2Start = Slot1Start + SlotLength;

        //Offsets to values in a slot. There are many more, but just some that are interesting.
        private const int SlotOffsetYear = 0x0020; //16bit.
        private const int SlotOffsetMonth = 0x0022; //16bit.
        private const int SlotOffsetPopulation = 0x002C; //32bit. Max 16777200? 24bit = 16777216?
        private const int SlotOffsetCityName = 0x0066; //10 bytes.
        private const int SlotOffsetMapNumber = 0x0070; //32bit. 3 bytes = 3 digits. 4th byte = 0. Scenarios/practice use 0000.
        private const int SlotOffsetTileTypeData = 0x0BF0; //Array of 16bit values.

        private readonly City[] mCities;

        private SaveFileSram(City[] cities)
        {
            mCities = cities;
        }

        public static SaveFileSram open(string sramPath)
        {
            using (FileStream fs = File.OpenRead(sramPath))
            {
                return open(fs);
            }
        }

        public static SaveFileSram open(Stream stream)
        {
            long streamStartPos = stream.Position;
            if ((streamStartPos + SramLength) > stream.Length)
            {
                throw new ArgumentException("SRAM length should be at least " + SramLength + " bytes!");
            }

            //Read some values in the header (first 16 bytes).
            string signature = stream.readString(5);
            if (!signature.StartsWith("SIM"))
            {
                throw new InvalidDataException("SRAM signature not present!");
            }
            bool isSlot1Used = stream.readUInt8() != 0;
            bool isSlot2Used = stream.readUInt8() != 0;

            List<City> cities = new List<City>();
            if (isSlot1Used)
            {
                cities.Add(readCitySlot(stream, streamStartPos + Slot1Start, CityOrigin.Slot1));
            }
            if (isSlot2Used)
            {
                cities.Add(readCitySlot(stream, streamStartPos + Slot2Start, CityOrigin.Slot2));
            }

            return new SaveFileSram(cities.ToArray());
        }

        public static SaveFileSram open(StateFile state) //Read SRAM data in state file.
        {
            return open(new MemoryStream(state.getSram()));
        }

        public override City[] getCities()
        {
            return mCities;
        }

        //public void save(string savePath) //TODO: Maybe try and implement a save method?
        //{
        //    //Implementing this would be a lot of work for something not really needed.
        //    //Saving all city data and creating a tile type data encoder (even a basic one) would be a lot of code/work.
        //    throw new NotImplementedException();
        //}

        private static City readCitySlot(Stream stream, long slotStartPos, CityOrigin origin)
        {
            stream.Seek(slotStartPos + SlotOffsetYear, SeekOrigin.Begin);
            int year = stream.readUInt16();

            stream.Seek(slotStartPos + SlotOffsetMonth, SeekOrigin.Begin);
            int month = stream.readUInt16();

            stream.Seek(slotStartPos + SlotOffsetPopulation, SeekOrigin.Begin);
            int population = (int)stream.readUInt32();

            stream.Seek(slotStartPos + SlotOffsetCityName, SeekOrigin.Begin);
            string name = SimCity.getCityName(stream.readArray(10), 0);

            stream.Seek(slotStartPos + SlotOffsetMapNumber, SeekOrigin.Begin);
            string mapNumberDigit3 = stream.readUInt8().ToString(); //10^0.
            string mapNumberDigit2 = stream.readUInt8().ToString(); //10^1.
            string mapNumberDigit1 = stream.readUInt8().ToString(); //10^2.
            string mapNumber = mapNumberDigit1 + mapNumberDigit2 + mapNumberDigit3;

            stream.Seek(slotStartPos + SlotOffsetTileTypeData, SeekOrigin.Begin);
            byte[] tileTypeData = decodeTileTypeData(stream);

            return new City(tileTypeData, year, month, population, name, mapNumber, origin);
        }

        private static byte[] decodeTileTypeData(Stream stream)
        {
            //SRAM city tile data is stored as a variable length array of unsigned shorts.
            //Tile data is read and written left-to-right and top-to-bottom to make the 120*100 tiles in a full city.
            //0x0000: 00CC CCTT TTTT TTTT = 1*1 tile specified by T is repeated C+1 times.
            //0x4000: 01CC CCLL LLLL LLLL = Read the last L bytes and then repeat it until C*2 bytes were written.
            //0x8000: 10CC CCTT TTTT TTTT = 3*3 tile specified by T is repeated C+1 times.
            //0xC000: ?                   = Not used? Same as 0x4000?
            //0xFFFF: 1111 1111 1111 1111 = End of tile data marker.

            //SimCity will not save if tile data can't be compressed enough to fit in a save slot.
            //Can happen if tile data is mostly random (rare, but it's possible to build such a "city").
            //Game will just show an "unable to save" message and clear the slot it tried saving to.
            //Pretty bad, but at least it doesn't crash/freeze. A game that can't guarantee that it
            //can perform a save feels like a very unusual case.

            MemoryStream ms = decompress(stream); //Make sure data is decompressed before decoding it further.
            return decodeTiles(ms);
        }

        private static MemoryStream decompress(Stream stream)
        {
            //Check for and expand any compression commands in stream.
            //01CC CCLL LLLL LLLL = Read the last L bytes and then repeat it until C*2 bytes were written.

            //Can instructions compress other compression instructions? Doesn't seem very likely.
            //Should be easy to fix though by decompressing the data over and over until no
            //compression instructions were found.

            MemoryStream ms = new MemoryStream();
            long streamStartPos = stream.Position;
            UInt16 data;
            while ((data = stream.readUInt16()) != 0xFFFF) //End marker?
            {
                int command = data >> 14;
                if (command == 1) //Compression instruction?
                {
                    int count = ((data >> 10) & 0x0F) * 2;
                    int length = data & 0x03FF;


                    //TODO: Try more saves if below conditions are ever true? Doesn't seem like it so far.
                    System.Diagnostics.Debug.Assert(
                        !(count == 0 || length == 0 || length % 2 != 0 || length > 256),
                        "Are these conditions ever true?");
                    //Count == 0? -> C+1 else C
                    //Length == 0? -> L+1 else L
                    //Length % 2? Length always even?
                    //Length max 128 bytes according to Adam Croke? But my airport save had longer repetitions? Max 256 bytes perhaps?


                    //Read the last L bytes and then repeat it until C*2 bytes were written.
                    ms.Seek(-length, SeekOrigin.Current);
                    byte[] last = ms.readArray(length);
                    for (int i = 0; i < count; i++)
                    {
                        ms.writeUInt8(last[i % length]);
                    }
                }
                else
                {
                    ms.writeUInt16(data);
                }
            }
            ms.writeUInt16(0xFFFF); //Write end marker before quitting.
            Bench.add(string.Format("Decompressed {0} bytes into {1} bytes!", stream.Position - streamStartPos, ms.Position));
            ms.Seek(0, SeekOrigin.Begin);
            return ms;
        }

        private static byte[] decodeTiles(Stream stream)
        {
            //Check for and expand any repeat tile commands in stream.
            //00CC CCTT TTTT TTTT = 1*1 tile specified by T is repeated C+1 times.
            //10CC CCTT TTTT TTTT = 3*3 tile specified by T is repeated C+1 times.

            byte[] tileTypeData = new byte[SimCity.TileTypeDataLength];
            int ind = 0;
            UInt16 data;
            while ((data = stream.readUInt16()) != 0xFFFF) //End marker?
            {
                int command = data >> 14;
                if (command == 0) //1*1 tile?
                {
                    writeTile(data, ref ind, tileTypeData, true);
                }
                else if (command == 1) //Compression instruction? Data should be decompressed before decoding.
                {
                    throw new Exception("Compression instruction in decompressed data!");
                }
                else if (command == 2) //3*3 tiles?
                {
                    writeTile(data, ref ind, tileTypeData, false);
                }
                else //Command 3 is not defined/used in the game? Or it's same as command 1?
                {
                    throw new NotImplementedException();
                }
            }
            Bench.add(string.Format("Decoded {0} bytes into {1} bytes!", stream.Length, SimCity.TileTypeDataLength));
            return tileTypeData;
        }

        private static void writeTile(int data, ref int ind, byte[] tileTypeData, bool is1x1)
        {
            //1*1 and 3*3 tiles are processed pretty much the same way.
            int count = ((data >> 10) & 0x0F) + 1;
            int tileType = data & 0x03FF;
            for (int i = 0; i < count; i++)
            {
                //Find next empty spot i.e. not occupied by a previously written 3*3 command.
                while (tileTypeData.read16bit(ind) > 0)
                {
                    ind += 2;
                }

                if (is1x1)
                {
                    tileTypeData.write16bit(ind, tileType);
                    ind += 2 * 1; //Move past the just written tile (1 tile in current row).
                }
                else
                {
                    //3*3 tile types are numbered sequentially so just increment it after every write.
                    int tt = tileType;
                    for (int y = 0; y < 3; y++)
                    {
                        int ttInd = ind + (y * SimCity.TileTypeDataRowLength);
                        for (int x = 0; x < 3; x++, ttInd += 2, tt++)
                        {
                            tileTypeData.write16bit(ttInd, tt);
                        }
                    }
                    ind += 2 * 3; //Move past the just written tile (3 tiles in current row).
                }
            }
        }
    }
}
