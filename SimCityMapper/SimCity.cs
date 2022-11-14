using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace SimCityMapper
{
    enum Season
    {
        Winter = 0,
        Spring = 1,
        Summer = 2,
        Autumn = 3,
    }

    //Data extracted from the game and methods used to do it.
    class SimCity
    {
        public const int CityWidthInTiles = 120; //Tiles.
        public const int CityHeightInTiles = 100;

        public const int CityWidth = CityWidthInTiles * TileWidth;
        public const int CityHeight = CityHeightInTiles * TileHeight;

        public const int TileWidth = 8; //Pixels.
        public const int TileHeight = 8;

        public const int TileTypeDataRowLength = CityWidthInTiles * 2;
        public const int TileTypeDataLength = TileTypeDataRowLength * CityHeightInTiles;

        private const int TileCharLength = (TileWidth * TileHeight * 4) / 8; //32 bytes per 4-bit tile.

        public const int TileTopIndexNone = 768; //Indicates that a tile type has no top tile.

        private const int RamTileTypeDataStart = 0x10200; //City tile type data.
        private const int RamPaletteStart = 0x02440; //City palettes.
        private const int RamYearStart = 0x00B53; //Year (2 bytes).
        private const int RamMonthStart = 0x00B55; //Month 1-12 (2 bytes?).
        private const int RamPopulationStart = 0x00BA5; //Population (4 bytes).
        private const int RamCityNameStart = 0x00B5B; //City name (10 bytes, same format as in save RAM).
        private const int RamMapNumberStart = 0x00B27; //3 bytes, one for each digit.

        private const int RomTileDataStart = 0x156A9; //Tile data table.
        private const int RomTileTopDataStart = 0x14F2D; //Tile top data table. Always -0x77C before tile data table?
        //Verified in USA version. Unverified tile data address in other versions:
        //RomTileDataStart: Japan=0x1586D, Europe=0x156B4, France=0x15712, Germany=0x1573C.

        private const int RomAnimatedTilesStart = 0x2B000; //Animated tile banks.
        private const int RomPalettesStart = 0x28000; //Palettes. The transitions between seasons are missing? Calculated?
        private const int RomPalettePreviewStart = 0x286E0; //Palette for the preview.
        private const int RomPaletteMinimapStart = 0x289A0; //Palette for the minimap.
        private const int RomPaletteHudStart = 0x28900; //Palette for the HUD. Also at 0x28B00?

        public static readonly Font Font1 = new Font(SimCityMapper.Properties.Resources.font1);
        public static readonly Font Font2 = new Font(SimCityMapper.Properties.Resources.font2);
        public static readonly Font Font3 = new Font(SimCityMapper.Properties.Resources.font3);

        private static readonly UInt16[][] mPaletteDataBg = new UInt16[][]
        {
            //Data for palette 0-7 i.e. background layer palettes.
            //Palette 0 and 5 are not animated.
            //Palette 1, 2, 3, 4 and 7 are season animated. 4 seasons.
            //Palette 6 is cycle animated (blinking). 4 steps.
            new UInt16[]{ //Winter.
            0x0000, 0x18C6, 0x35AD, 0x4210, 0x0000, 0x1929, 0x4F18, 0x0000, 0x0000, 0x6B5A, 0x35AD, 0x4210, 0x0000, 0x0000, 0x4F18, 0x7FFF,
            0x0000, 0x00CF, 0x035F, 0x6BBD, 0x54C0, 0x0000, 0x2D6B, 0x3EB7, 0x01FF, 0x0018, 0x1884, 0x2194, 0x6EB5, 0x7FBD, 0x6273, 0x00A8,
            0x0000, 0x1063, 0x20A5, 0x354A, 0x294A, 0x5251, 0x6B39, 0x7FFF, 0x5B7B, 0x7CA5, 0x2128, 0x4005, 0x6EB5, 0x7FBD, 0x6273, 0x3231,
            0x0000, 0x0C83, 0x18E6, 0x2128, 0x318C, 0x35ED, 0x4A72, 0x6338, 0x77FE, 0x46B8, 0x31F0, 0x1909, 0x6EB5, 0x7FBD, 0x6273, 0x35B3,
            0x0000, 0x14E7, 0x258C, 0x4212, 0x637E, 0x5EF8, 0x6F7D, 0x7BDF, 0x2591, 0x152E, 0x08C8, 0x577E, 0x6EB5, 0x7FBD, 0x0069, 0x29F0,
            0x0000, 0x001F, 0x74E0, 0x565D, 0x7E4B, 0x07C9, 0x03FF, 0x7FFF, 0x24C6, 0x7FFF, 0x73B1, 0x5ACD, 0x2108, 0x4208, 0x6A53, 0x779F,
            0x0000, 0x18C7, 0x252A, 0x2D8C, 0x35AE, 0x4632, 0x631A, 0x7FFF, 0x001F, 0x001A, 0x0015, 0x0010, 0x3A56, 0x7C16, 0x6810, 0x4C09, //Cycle 0.
            0x0000, 0x0018, 0x006A, 0x7FFF, 0x4699, 0x7CA0, 0x01FF, 0x6B39, 0x49EF, 0x1884, 0x02D6, 0x016E, 0x6EB5, 0x7FBD, 0x6273, 0x00A8,
            },
            new UInt16[]{ //Spring.
            0x0000, 0x18C6, 0x35AD, 0x4210, 0x0000, 0x1929, 0x4F18, 0x0000, 0x0000, 0x6B5A, 0x35AD, 0x4210, 0x0000, 0x0000, 0x4F18, 0x7FFF,
            0x0000, 0x00CF, 0x035F, 0x6BBD, 0x54C0, 0x0000, 0x2D6B, 0x3EB7, 0x01FF, 0x0018, 0x1884, 0x2194, 0x3A56, 0x12E4, 0x7F1F, 0x59D6,
            0x0000, 0x1063, 0x20A5, 0x354A, 0x294A, 0x5251, 0x6B39, 0x7FFF, 0x5B7B, 0x7CA5, 0x2128, 0x4005, 0x3A56, 0x12E4, 0x0A02, 0x3231,
            0x0000, 0x0C83, 0x18E6, 0x2128, 0x318C, 0x35ED, 0x4A72, 0x6338, 0x77FE, 0x46B8, 0x31F0, 0x1909, 0x3A56, 0x12E4, 0x0A02, 0x35B3,
            0x0000, 0x14E7, 0x258C, 0x4212, 0x637E, 0x5EF8, 0x6F7D, 0x7BDF, 0x2591, 0x152E, 0x08C8, 0x577E, 0x3A56, 0x12E4, 0x0069, 0x29F0,
            0x0000, 0x001F, 0x74E0, 0x565D, 0x7E4B, 0x07C9, 0x03FF, 0x7FFF, 0x24C6, 0x7FFF, 0x73B1, 0x5ACD, 0x2108, 0x4208, 0x6A53, 0x779F,
            0x0000, 0x18C7, 0x252A, 0x2D8C, 0x35AE, 0x4632, 0x631A, 0x7FFF, 0x001A, 0x0015, 0x0010, 0x000D, 0x3A56, 0x6810, 0x4C09, 0x3006, //Cycle 1.
            0x0000, 0x0018, 0x006A, 0x7FFF, 0x4699, 0x7CA0, 0x01FF, 0x6B39, 0x49EF, 0x1884, 0x02D6, 0x016E, 0x3A56, 0x02C0, 0x0200, 0x00E0,
            },
            new UInt16[]{ //Summer.
            0x0000, 0x18C6, 0x35AD, 0x4210, 0x0000, 0x1929, 0x4F18, 0x0000, 0x0000, 0x6B5A, 0x35AD, 0x4210, 0x0000, 0x0000, 0x4F18, 0x7FFF,
            0x0000, 0x00CF, 0x035F, 0x6BBD, 0x54C0, 0x0000, 0x2D6B, 0x3EB7, 0x01FF, 0x0018, 0x1884, 0x2194, 0x3A56, 0x03A0, 0x0200, 0x00E0,
            0x0000, 0x1063, 0x20A5, 0x354A, 0x294A, 0x5251, 0x6B39, 0x7FFF, 0x5B7B, 0x7CA5, 0x2128, 0x4005, 0x3A56, 0x03A0, 0x0200, 0x3231,
            0x0000, 0x0C83, 0x18E6, 0x2128, 0x318C, 0x35ED, 0x4A72, 0x6338, 0x77FE, 0x46B8, 0x31F0, 0x1909, 0x3A56, 0x03A0, 0x0200, 0x35B3,
            0x0000, 0x14E7, 0x258C, 0x4212, 0x637E, 0x5EF8, 0x6F7D, 0x7BDF, 0x2591, 0x152E, 0x08C8, 0x577E, 0x3A56, 0x03A0, 0x0069, 0x29F0,
            0x0000, 0x001F, 0x74E0, 0x565D, 0x7E4B, 0x07C9, 0x03FF, 0x7FFF, 0x24C6, 0x7FFF, 0x73B1, 0x5ACD, 0x2108, 0x4208, 0x6A53, 0x779F,
            0x0000, 0x18C7, 0x252A, 0x2D8C, 0x35AE, 0x4632, 0x631A, 0x7FFF, 0x0015, 0x0010, 0x000D, 0x000A, 0x3A56, 0x4C09, 0x3006, 0x2004, //Cycle 2.
            0x0000, 0x0018, 0x006A, 0x7FFF, 0x4699, 0x7CA0, 0x01FF, 0x6B39, 0x49EF, 0x1884, 0x02D6, 0x016E, 0x3A56, 0x03A0, 0x0200, 0x00E0,
            },
            new UInt16[]{ //Autumn.
            0x0000, 0x18C6, 0x35AD, 0x4210, 0x0000, 0x1929, 0x4F18, 0x0000, 0x0000, 0x6B5A, 0x35AD, 0x4210, 0x0000, 0x0000, 0x4F18, 0x7FFF,
            0x0000, 0x00CF, 0x035F, 0x6BBD, 0x54C0, 0x0000, 0x2D6B, 0x3EB7, 0x01FF, 0x0018, 0x1884, 0x2194, 0x3A56, 0x0EB2, 0x152C, 0x00A8,
            0x0000, 0x1063, 0x20A5, 0x354A, 0x294A, 0x5251, 0x6B39, 0x7FFF, 0x5B7B, 0x7CA5, 0x2128, 0x4005, 0x3A56, 0x0EB2, 0x152C, 0x3231,
            0x0000, 0x0C83, 0x18E6, 0x2128, 0x318C, 0x35ED, 0x4A72, 0x6338, 0x77FE, 0x46B8, 0x31F0, 0x1909, 0x3A56, 0x0EB2, 0x152C, 0x35B3,
            0x0000, 0x14E7, 0x258C, 0x4212, 0x637E, 0x5EF8, 0x6F7D, 0x7BDF, 0x2591, 0x152E, 0x08C8, 0x577E, 0x3A56, 0x0EB2, 0x0069, 0x29F0,
            0x0000, 0x001F, 0x74E0, 0x565D, 0x7E4B, 0x07C9, 0x03FF, 0x7FFF, 0x24C6, 0x7FFF, 0x73B1, 0x5ACD, 0x2108, 0x4208, 0x6A53, 0x779F,
            0x0000, 0x18C7, 0x252A, 0x2D8C, 0x35AE, 0x4632, 0x631A, 0x7FFF, 0x0010, 0x000D, 0x000A, 0x0005, 0x3A56, 0x3006, 0x2006, 0x1000, //Cycle 3.
            0x0000, 0x0018, 0x006A, 0x7FFF, 0x4699, 0x7CA0, 0x01FF, 0x6B39, 0x49EF, 0x1884, 0x02D6, 0x016E, 0x3A56, 0x0EB2, 0x152C, 0x00A8,
            },
        };

        private static readonly UInt16[] mPaletteDataPreview =
        {
            //Data for palette used for the preview in the map select screen.
            0x0000, 0x0000, 0x0000, 0x001F, 0x7CA0, 0x037C, 0x73B1, 0x7FFF, 0x0100, 0x7FFF, 0x1220, 0x46B6, 0x0120, 0x4208, 0x6A53, 0x779F
        };

        private static readonly UInt16[] mPaletteDataMinimap =
        {
            //Data for palette used for the minimap.
            0x0000, 0x0000, 0x0000, 0x001F, 0x7CA0, 0x037C, 0x73B1, 0x7FFF, 0x0000, 0x7FFF, 0x73B1, 0x5ACD, 0x14A7, 0x4208, 0x6A53, 0x779F
        };

        private static readonly UInt16[] mPaletteDataHud =
        {
            //Data for palette used for the HUD.
            0x0000, 0x6AB5, 0x7FFF, 0x18CB, 0x0000, 0x5B7F, 0x31D6, 0x1E27, 0x01C0, 0x23FF, 0x7C00, 0x28F4, 0x32FA, 0x3D29, 0x415F, 0x2535
        };

        private static Frame createFrameFromSnes4BitTiles(byte[] data, int dataInd, int columnCount, int rowCount)
        {
            //Create a frame with 4-bit linear tiles from data with SNES 4-bit planar tiles.
            Frame frame = new Frame(columnCount * TileWidth, rowCount * TileHeight);
            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < columnCount; col++, dataInd += TileCharLength)
                {
                    //Convert SNES 4-bit planar tiles to simple 4-bit linear tiles.
                    for (int tileY = 0; tileY < TileHeight; tileY++)
                    {
                        //A 4-bit tile is combined from two 2-bit planar tiles.
                        byte plane1 = data[dataInd + 0 + (tileY * 2)]; //First 2-bit tile.
                        byte plane2 = data[dataInd + 1 + (tileY * 2)];
                        byte plane3 = data[dataInd + 16 + (tileY * 2)]; //Second 2-bit tile.
                        byte plane4 = data[dataInd + 17 + (tileY * 2)];
                        for (int tileX = 0; tileX < TileWidth; tileX++)
                        {
                            byte value = (byte)(
                                (((plane1 >> (7 - tileX)) & 0x01) << 0) |
                                (((plane2 >> (7 - tileX)) & 0x01) << 1) |
                                (((plane3 >> (7 - tileX)) & 0x01) << 2) |
                                (((plane4 >> (7 - tileX)) & 0x01) << 3));

                            frame[(col * TileWidth) + tileX, (row * TileHeight) + tileY] = value;
                        }
                    }
                }
            }
            return frame;
        }

        public static byte[] getTileTypeData(byte[] ram)
        {
            byte[] tileTypeData = new byte[TileTypeDataLength];
            Array.Copy(ram, RamTileTypeDataStart, tileTypeData, 0, tileTypeData.Length);
            return tileTypeData;
        }

        public static int getTileTypeDataRamInd(int tileX, int tileY)
        {
            //Calculate index to tile type data in RAM.
            return RamTileTypeDataStart + (tileY * TileTypeDataRowLength) + (tileX * 2);
        }

        public static int getYear(byte[] ram)
        {
            return ram.read16bit(RamYearStart);
        }

        public static int getMonth(byte[] ram)
        {
            int month = ram[RamMonthStart];
            //Value in RAM can be 13? After 12, 13 is written, but quickly changed to 1 in the game.
            return month == 13 ? 1 : month;
        }

        public static int getPopulation(byte[] ram)
        {
            return ram.read32bit(RamPopulationStart);
        }

        public static string getCityName(byte[] ram)
        {
            return getCityName(ram, RamCityNameStart);
        }

        public static string getCityName(byte[] data, int ind)
        {
            //City name is stored in 10 bytes. First byte is length (0-8 bytes). Last byte is always 0.
            if ((ind + 10) > data.Length)
            {
                throw new ArgumentException("Data is too short to read city name from at specified index!");
            }
            int length = data[ind];
            System.Diagnostics.Debug.Assert(length <= 8, "City name data length is >8!");
            length = Math.Min(length, 8);

            ind++; //Move to the first byte in the name.
            string name = "";
            for (int i = 0; i < length; i++, ind++)
            {
                int c = data[ind];
                if (c <= 0x09) name += (char)('0' + c); //0-9?
                else if (c <= 0x23) name += (char)('A' + (c - 10)); //A-Z?
                else if (c == 0x24) name += ',';
                else if (c == 0x25) name += '.';
                else if (c == 0x26) name += '-';
                else name += ' ';
            }
            return name;
        }

        public static string getMapNumber(byte[] ram)
        {
            string mapNumberDigit3 = ram[RamMapNumberStart + 0].ToString(); //10^0.
            string mapNumberDigit2 = ram[RamMapNumberStart + 1].ToString(); //10^1.
            string mapNumberDigit1 = ram[RamMapNumberStart + 2].ToString(); //10^2.
            return mapNumberDigit1 + mapNumberDigit2 + mapNumberDigit3;
        }

        public static Season getSeason(byte[] ram)
        {
            return toSeason(getMonth(ram));
        }

        public static Season toSeason(int month)
        {
            switch (month)
            {
                case 1: return Season.Winter;
                case 2: return Season.Winter;
                case 3: return Season.Spring;
                case 4: return Season.Spring;
                case 5: return Season.Spring;
                case 6: return Season.Summer;
                case 7: return Season.Summer;
                case 8: return Season.Summer;
                case 9: return Season.Autumn;
                case 10: return Season.Autumn;
                case 11: return Season.Autumn;
                case 12: return Season.Winter;
                default: throw new ArgumentException("Month should be 1-12!");
            }
        }

        public static int toMonth(Season season) //"Convert" season to its middle month.
        {
            switch (season)
            {
                case Season.Winter: return 1; //12,1,2.
                case Season.Spring: return 4; //3,4,5.
                case Season.Summer: return 7; //6,7,8.
                case Season.Autumn: return 10; //9,10,11.
                default: throw new ArgumentException("Unknown season!"); //Should never happen.
            }
        }

        public static string toMonthId(int month)
        {
            //Strings same as in the game.
            switch (month)
            {
                case 1: return "JAN";
                case 2: return "FEB";
                case 3: return "MAR";
                case 4: return "APR";
                case 5: return "MAY";
                case 6: return "JUN";
                case 7: return "JUL";
                case 8: return "AUG";
                case 9: return "SEP";
                case 10: return "OCT";
                case 11: return "NOV";
                case 12: return "DEC";
                default: throw new ArgumentException("Month should be 1-12!");
            }
        }

        public static City getCity(byte[] ram)
        {
            byte[] tileTypeData = getTileTypeData(ram);
            int year = getYear(ram);
            int month = getMonth(ram);
            int population = getPopulation(ram);
            string name = getCityName(ram);
            string mapNumber = getMapNumber(ram);
            return new City(tileTypeData, year, month, population, name, mapNumber, CityOrigin.Single);
        }

        private static Frame mBgTileSheetFrame = null; //Cached bg tile sheet frame.
        public static Frame getBgTileSheetFrame()
        {
            if (mBgTileSheetFrame == null) //Not cached yet?
            {
                mBgTileSheetFrame = new Frame(32 * TileWidth, (32 - 5 + 20) * TileHeight);

                //Embedded file seems slower to read than external. Not really noticeable though,
                //but let's use the external file while debugging.
#if DEBUG
                //Read from external file.
                using (FileStream msComp = File.OpenRead(Program.DebugFolder + @"\SimCityMapper\Resources\bg tile sheet.gz"))
#else
                //Read from embedded resource.
                //3-9 times slower than external file. Still pretty fast though.
                using (MemoryStream msComp = new MemoryStream(SimCityMapper.Properties.Resources.bg_tile_sheet))
#endif
                {
                    using (MemoryStream msDecomp = msComp.getDecompressed())
                    {
                        msDecomp.Seek(0, SeekOrigin.Begin);

                        for (int i = 0, len = (int)msDecomp.Length; i < len; i++)
                        {
                            //Separate the two 4-bit values per byte.
                            byte b = msDecomp.readUInt8();
                            mBgTileSheetFrame[(i * 2) + 0] = (byte)((b >> 0) & 0x0F);
                            mBgTileSheetFrame[(i * 2) + 1] = (byte)((b >> 4) & 0x0F);
                        }

                    }
                }
            }
            return mBgTileSheetFrame;
        }

        private static SnesPalette[] mSnesPaletteBg = null; //Cached SNES bg tiles palette for the 4 seasons.
        private static SnesPalette[] getSnesPalettesBg()
        {
            if (mSnesPaletteBg == null) //Not cached yet?
            {
                mSnesPaletteBg = new SnesPalette[4];
                mSnesPaletteBg[0] = new SnesPalette(mPaletteDataBg[0], 0, 128); //Winter.
                mSnesPaletteBg[1] = new SnesPalette(mPaletteDataBg[1], 0, 128); //Spring.
                mSnesPaletteBg[2] = new SnesPalette(mPaletteDataBg[2], 0, 128); //Summer.
                mSnesPaletteBg[3] = new SnesPalette(mPaletteDataBg[3], 0, 128); //Autumn.
            }
            return mSnesPaletteBg;
        }

        public static SnesColorRow getSnesColorRowBg(Season season, int row)
        {
            return getSnesPalettesBg()[(int)season].getSnesColorRow(row);
        }

        public static SnesColorRow getSnesColorRowBg(int cycle) //Color cycle (0-3) in palette row 6.
        {
            return getSnesPalettesBg()[cycle].getSnesColorRow(6);
        }

        public static SnesColorRow getSnesColorRowPreview()
        {
            return new SnesColorRow(mPaletteDataPreview, 0);
        }

        public static SnesColorRow getSnesColorRowMinimap()
        {
            return new SnesColorRow(mPaletteDataMinimap, 0);
        }

        public static SnesColorRow getSnesColorRowHud()
        {
            return new SnesColorRow(mPaletteDataHud, 0);
        }

        public static void extractTileTypeData()
        {
            //Extract tile type data from ROM. There are two tables, one for the normal (bottom)
            //tiles and one for the extra top tiles. Each table seems to have 958 entries?
            //Format of table-entries seems to be the same as the SNES tile map format?
            //SNES tile map entry: vhfppptt tttttttt
            //v,h = V/H flipping. Not used (always 0) in the tables?
            //f = Priority flag. Sometimes used?
            //p,t = palette and tile index.

            byte[] rom = File.ReadAllBytes(Program.RomPath);

            StringBuilder sb = new StringBuilder();
            StringBuilder sbPal = new StringBuilder();
            StringBuilder sbTile = new StringBuilder();
            StringBuilder sbTileTop = new StringBuilder();
            for (int tileType = 0; tileType < 1024; tileType++) //Tile type (0-1023).
            {
                //Read table entry in ROM.
                int entry = rom.read16bit(SimCity.RomTileDataStart + (tileType * 2));
                int entryTop = rom.read16bit(SimCity.RomTileTopDataStart + (tileType * 2));

                //Read tile index.
                int tileInd = entry & 0x03FF;
                int tileIndTop = entryTop & 0x03FF;

                //Read palette number.
                int palRow = (entry >> 10) & 0x07;
                int palRowTop = (entryTop >> 10) & 0x07;

                //V/H flipping never used?
                System.Diagnostics.Debug.Assert((entry & 0xC000) == 0 && (entryTop & 0xC000) == 0,
                    "V/H flipping bits not 0!");

                //All "normal" top tiles have the same palette as its bottom tile?
                System.Diagnostics.Debug.Assert(palRowTop == palRow || tileIndTop == TileTopIndexNone || tileType > 957,
                    "Extra top tile palette not same as bottom!");

                sb.AppendFormat("case {0}: tileInd={1}; tileTopInd={2}; palRow={3}; miniCol={4}; break; //{5}",
                    tileType, tileInd, tileIndTop, palRow, TileType.getMinimapColor(tileType), TileType.getTileDescription(tileType));
                sb.AppendLine();

                sbPal.AppendFormat("case {0}: return {1};", tileType, palRow);
                sbPal.AppendLine();

                sbTile.AppendFormat("case {0}: return {1};", tileType, tileInd);
                sbTile.AppendLine();

                sbTileTop.AppendFormat("case {0}: return {1};", tileType, tileIndTop);
                sbTileTop.AppendLine();
            }
            File.WriteAllText(Program.TempFolder + "extract tile data out switch.txt", sb.ToString());
            File.WriteAllText(Program.TempFolder + "extract tile data pal switch.txt", sbPal.ToString());
            File.WriteAllText(Program.TempFolder + "extract tile data tile switch.txt", sbTile.ToString());
            File.WriteAllText(Program.TempFolder + "extract tile data tile top switch.txt", sbTileTop.ToString());
        }

        public static void extractTileTypeDataMinimapWrite()
        {
            //This method is for creating the state file that we will later on extract minimap colors from VRAM in.

            //The "minimap.state" is just a saved state of a map loaded in the map select screen.

            //My process to extract what colors tile data uses in the minimap/preview in the map select screen.
            //1. Use this method on a state file saved in the map select screen with a map loaded.
            //2a. Open the written state file in Snes9x and leave the map select screen (go back to start menu).
            //2b. Reenter the map select screen so the game can update VRAM and then save state file.
            //3. Use the extract tile data minimap read method on the state file.

            StateFileSnes9x state = StateFileSnes9x.open(Program.StatesFolder + "minimap.state");
            byte[] ram = state.Ram;

            //Write a 32*32 tile block with all tiles to RAM.
            int tileType = 0;
            for (int tileY = 0; tileY < 32; tileY++)
            {
                for (int tileX = 0; tileX < 32; tileX++, tileType++)
                {
                    int ramInd = getTileTypeDataRamInd(tileX, tileY);
                    ram.write16bit(ramInd, tileType);
                }
            }

            state.save(Program.TempFolder + "extract minimap tileblock.state");
        }

        public static void extractTileTypeDataMinimapRead()
        {
            //This method is for extracting minimap colors from VRAM in the state file created earlier.

            //The minimap/preview in the map select screen is stored in VRAM at 0x4400.
            //It is 16*14 tiles big and uses palette 7. Actual map displayed is at
            //x=0, y=2 and has a size of 120*100 pixels. The area outside is blank/black.

            //Read the VRAM dump. The state file must have been loaded and resaved in Snes9x beforehand.
            StateFileSnes9x state = StateFileSnes9x.open(Program.TempFolder + "extract minimap tileblock.state");
            byte[] vram = state.Vram;

            //Read the 32*32 tile block in the minimap in VRAM.
            Frame miniMap = createFrameFromSnes4BitTiles(vram, 0x4400, 16, 14);
            StringBuilder sb = new StringBuilder();
            StringBuilder sbColMini = new StringBuilder();
            for (int y = 0; y < 32; y++)
            {
                for (int x = 0; x < 32; x++)
                {
                    int tileType = (32 * y) + x;
                    int miniCol = miniMap[x, y + 2]; //Read color index used by tile type.

                    int palRow, tileInd, tileTopInd;
                    TileType.get(tileType, out tileInd, out tileTopInd, out palRow);

                    sb.AppendFormat("case {0}: tileInd={1}; tileTopInd={2}; palRow={3}; miniCol={4}; break; //{5}",
                        tileType, tileInd, tileTopInd, palRow, miniCol, TileType.getTileDescription(tileType));
                    sb.AppendLine();

                    sbColMini.AppendFormat("case {0}: return {1};", tileType, miniCol);
                    sbColMini.AppendLine();
                }
            }
            File.WriteAllText(Program.TempFolder + "extract tile data minimap out switch.txt", sb.ToString());
            File.WriteAllText(Program.TempFolder + "extract tile data minimap col switch.txt", sbColMini.ToString());
        }

        public static void extractPaletteData()
        {
            //Extract palette values from ROM. Palette 6 with the animated cycle is not same as
            //what I'm seeing in the game. The seven animated colors in it are a bit different,
            //probably because they are calculated in the game?

            //So palette 6 needs to be fixed manually afterwards in the code produced by this method.

            byte[] rom = File.ReadAllBytes(Program.RomPath);

            string[] seasons = { "Winter.", "Spring.", "Summer.", "Autumn." };
            StringBuilder sbBg = new StringBuilder();
            int romInd = RomPalettesStart;
            for (int i = 0; i < 4; i++) //4 seasons.
            {
                sbBg.AppendLine("new UInt16[]{ //" + seasons[i]);
                for (int j = 0; j < 8; j++) //8 palette rows.
                {
                    sbBg.Append(extractPaletteDataRow(rom, romInd));
                    romInd += 16 * 2; //16 colors of 15bit rgb (2 bytes) in each row.

                    if (j == 6) //Append 'cycle' to row of the animated palette.
                    {
                        sbBg.Append(" //Cycle " + i + ".");
                    }
                    sbBg.AppendLine();
                }
                sbBg.AppendLine("},");
            }
            File.WriteAllText(Program.TempFolder + "extract palette data rom bg.txt", sbBg.ToString());

            string sbPrev = extractPaletteDataRow(rom, RomPalettePreviewStart);
            File.WriteAllText(Program.TempFolder + "extract palette data rom preview.txt", sbPrev);

            string sbMini = extractPaletteDataRow(rom, RomPaletteMinimapStart);
            File.WriteAllText(Program.TempFolder + "extract palette data rom minimap.txt", sbMini);

            string sbHud = extractPaletteDataRow(rom, RomPaletteHudStart);
            File.WriteAllText(Program.TempFolder + "extract palette data rom hud.txt", sbHud);
        }

        private static string extractPaletteDataRow(byte[] data, int dataInd)
        {
            StringBuilder sb = new StringBuilder();
            for (int k = 0; k < 16; k++, dataInd += 2) //16 colors of 15bit rgb (2 bytes) in each row.
            {
                int rgb15Bit = data.read16bit(dataInd);
                sb.Append("0x" + rgb15Bit.ToString("X4") + ",");
                if (k < 15) //Skip append space if last index.
                {
                    sb.Append(" ");
                }
            }
            return sb.ToString();
        }

        public static void extractBgTileSheet()
        {
            //Extract background tile sheet from ROM and VRAM. Tiles are compressed(?) in ROM,
            //so it's easier to extract them from a state file's VRAM instead. The animated
            //tiles are not compressed in ROM however.

            //Background tile sheet format:
            //32 tiles wide (32*8=256 pixels).
            //47 tiles high (47*8=376 pixels). 27 static rows plus 4*5 animated rowCount.
            //Tile 0-863 is static (first 27 rows).
            //Tile 864-1503 is four banks of animated tiles. Each bank is 160 tiles (5 rows).

            byte[] rom = File.ReadAllBytes(Program.RomPath);
            StateFileSnes9x state = StateFileSnes9x.open(Program.StatesFolder + "paused.state");
            byte[] ram = state.Ram;
            byte[] vram = state.Vram;

            Frame tileFrame = createFrameFromSnes4BitTiles(vram, 0, 32, 32 - 5);
            Frame animFrame = createFrameFromSnes4BitTiles(rom, RomAnimatedTilesStart, 32, 4 * 5);

            Frame sheetFrame = new Frame(tileFrame.Width, tileFrame.Height + animFrame.Height);
            sheetFrame.write(tileFrame, new Point(0, 0));
            sheetFrame.write(animFrame, new Point(0, tileFrame.Height));

            byte[] sheetBytes = new byte[sheetFrame.Length / 2];
            for (int i = 0; i < sheetBytes.Length; i++)
            {
                //Store two 4-bit values per byte to decrease file length.
                byte lo = sheetFrame[(i * 2) + 0];
                byte hi = sheetFrame[(i * 2) + 1];
                sheetBytes[i] = (byte)(lo | (hi << 4));
            }
            File.WriteAllBytes(Program.TempFolder + "extract bg tile sheet.bin", sheetBytes);

            //Write as compressed file.
            MemoryStream ms = new MemoryStream(sheetBytes);
            ms.Seek(0, SeekOrigin.Begin);
            ms.saveCompressed(Program.TempFolder + "extract bg tile sheet.gz");
        }

        public static List<byte[]> getTileTypeDataAllMaps()
        {
            //Tile type data for all maps was dumped from the game with a lua-script for Snes9x.
            byte[] mapSet1 = (Program.FilesFolder + @"lua\MapsTileTypeDataSet1.gz").decompressFileToBytes(); //Map 0-999. Normal versions.
            byte[] mapSet2 = (Program.FilesFolder + @"lua\MapsTileTypeDataSet2.gz").decompressFileToBytes(); //Map 1000-1999. Alternate versions *1.

            List<byte[]> tileTypeData = new List<byte[]>();
            for (int i = 0; i < mapSet1.Length; i += TileTypeDataLength)
            {
                byte[] tt = new byte[TileTypeDataLength];
                Array.Copy(mapSet1, i, tt, 0, tt.Length);
                tileTypeData.Add(tt);
            }
            for (int i = 0; i < mapSet2.Length; i += TileTypeDataLength)
            {
                byte[] tt = new byte[TileTypeDataLength];
                Array.Copy(mapSet2, i, tt, 0, tt.Length);
                tileTypeData.Add(tt);
            }
            return tileTypeData;

            //*1 Alternate maps.
            //There are 1000 normal maps in the game, but the map generator is affected by a
            //few values in work RAM. One of these values at RAM address 0x7E0B2A is set to
            //0xFF by the game at certain instances which will generate an alternate version
            //of the normal map.

            //One of these instances is when you start a new city on a generated map. Which
            //means that if you go back to the map select screen (use goto menu in the save
            //load menu) the alternate version will be generated instead.

            //Another instance is when you power on the game so the map 000 you're used to
            //seeing after entering the map select screen is actually the alternate version
            //of it.

            //The 1000 alternate maps are available in all versions of the game except the
            //japanese version. The 1000 normal maps are the same in all versions though.

            //I usually refer to the 1000 normal maps as #0000-#0999 and the 1000 alternate
            //maps as #1000-#1999.

            //I haven't checked in a debugger, but it seems like the map generator is affected
            //by the values in 0x7E0B27-0x7E0B29 (selected map number digits) and the MSB in
            //0x7E0B2A. Values in 0x7E0B27-0x7E0B29 above 9 (max in the map select screen)
            //will work so this means that actually 2^25 (3*8+1 bits) different maps can be
            //generated.
        }

        public static void createTestRandomCityState()
        {
            //Create a state file with a city containing random tiles to test if the game can always save. Answer is no.
            StateFileSnes9x state = StateFileSnes9x.open(Program.StatesFolder + "paused.state");
            byte[] ram = state.Ram;
            Random rng = new Random(13);
            for (int i = 0, ind = RamTileTypeDataStart; i < TileTypeDataLength; i += 2, ind += 2)
            {
                ram.write16bit(ind, rng.Next(0, 40)); //Unsavable.
                //ram.write16bit(ind, rng.Next(35, 40)); //Takes a while, but saveable.
            }
            state.save(Program.TempFolder + "random.state");
            getCity(ram).saveImage(Season.Spring, false, Program.TempFolder + "random.state image.png");
        }
    }
}
