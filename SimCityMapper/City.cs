using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using Mechaskrom.Gif;

namespace SimCityMapper
{
    enum CityOrigin //Where city was created from.
    {
        Single, //RAM or not specified.
        Slot1, //SRAM slot 1.
        Slot2, //SRAM slot 2.
    }

    class City
    {
        public const int WidthInTiles = SimCity.CityWidthInTiles;
        public const int HeightInTiles = SimCity.CityHeightInTiles;

        private const int Width = SimCity.CityWidth;
        private const int Height = SimCity.CityHeight;

        private const int TileWidth = SimCity.TileWidth;
        private const int TileHeight = SimCity.TileHeight;

        //Animated bg tile banks are changed every 12th frame so this should mean that the
        //frame delay in an animated GIF should be (1/60)*12=0.2 seconds or 20 centiseconds.
        private const int GifDelay = 20; //Centiseconds.

        //Add a margin around image of city so there is room for any top tiles (top and left edge) and any text at the bottom.
        private static readonly Size Margin = new Size(TileWidth * 2, TileHeight * 2); //Min 2 tiles around recommended.

        //Remap 4-bit pixels (0-15) to 8 different palettes in an 8-bit palette (0-255). 0 is not remapped (always transparent).
        private static readonly byte[][] BgPaletteRemaps = new byte[][]
        {
            new byte[] {0x00,0x01,0x02,0x03,0x04,0x05,0x06,0x07,0x08,0x09,0x0A,0x0B,0x0C,0x0D,0x0E,0x0F}, //Palette 0.
            new byte[] {0x00,0x11,0x12,0x13,0x14,0x15,0x16,0x17,0x18,0x19,0x1A,0x1B,0x1C,0x1D,0x1E,0x1F}, //Palette 1.
            new byte[] {0x00,0x21,0x22,0x23,0x24,0x25,0x26,0x27,0x28,0x29,0x2A,0x2B,0x2C,0x2D,0x2E,0x2F}, //Palette 2.
            new byte[] {0x00,0x31,0x32,0x33,0x34,0x35,0x36,0x37,0x38,0x39,0x3A,0x3B,0x3C,0x3D,0x3E,0x3F}, //Palette 3.
            new byte[] {0x00,0x41,0x42,0x43,0x44,0x45,0x46,0x47,0x48,0x49,0x4A,0x4B,0x4C,0x4D,0x4E,0x4F}, //Palette 4.
            new byte[] {0x00,0x51,0x52,0x53,0x54,0x55,0x56,0x57,0x58,0x59,0x5A,0x5B,0x5C,0x5D,0x5E,0x5F}, //Palette 5.
            new byte[] {0x00,0x61,0x62,0x63,0x64,0x65,0x66,0x67,0x68,0x69,0x6A,0x6B,0x6C,0x6D,0x6E,0x6F}, //Palette 6.
            new byte[] {0x00,0x71,0x72,0x73,0x74,0x75,0x76,0x77,0x78,0x79,0x7A,0x7B,0x7C,0x7D,0x7E,0x7F}, //Palette 7.
        };

        //Remap fonts (uses three colors for now) to values in the HUD palette (assumed to be in row 8).
        private static readonly byte[] FontCreamRemap = new byte[]
        {
            0x00,0x85,0x84, //transparent, cream white, black.
        };

        private static readonly byte[] FontWhiteRemap = new byte[]
        {
            0x00,0x82,0x84, //transparent, white, black.
        };

        private readonly byte[] mTileTypeData;
        private readonly int mYear;
        private readonly int mMonth;
        private readonly int mPopulation;
        private readonly string mName;
        private readonly string mMapNumber;
        private readonly CityOrigin mOrigin;

        public City(byte[] tileTypeData, int year, int month, int population, string name, string mapNumber, CityOrigin origin)
        {
            if (tileTypeData.Length != SimCity.TileTypeDataLength)
            {
                throw new ArgumentException("Tile type data length should be " + SimCity.TileTypeDataLength + " bytes!");
            }
            mTileTypeData = tileTypeData;
            mYear = year;
            mMonth = month.clip(1, 12); //Make sure month is valid.
            mPopulation = population;
            mName = name;
            mMapNumber = mapNumber;
            mOrigin = origin;
        }

        public int Year
        {
            get { return mYear; }
        }

        public int Month
        {
            get { return mMonth; }
        }

        public int Population
        {
            get { return mPopulation; }
        }

        public string Name
        {
            get { return mName; }
        }

        public string MapNumber //000-999. Doesn't distinguish between normal and alternate version. Scenario/practice == 000.
        {
            get { return mMapNumber; }
        }

        public CityOrigin Origin
        {
            get { return mOrigin; }
        }

        public Season Season
        {
            get { return SimCity.toSeason(mMonth); }
        }

        public void saveImage(string savePath)
        {
            saveImage(Season, false, savePath);
        }

        public void saveImage(Season season, bool doFooter, string savePath)
        {
            saveImage(season, false, doFooter, savePath);
        }

        public void saveImageAnimated(string savePath)
        {
            saveImageAnimated(Season, false, savePath);
        }

        public void saveImageAnimated(Season season, bool doFooter, string savePath)
        {
            saveImage(season, true, doFooter, savePath);
        }

        private void saveImage(Season season, bool doAnimation, bool doFooter, string savePath)
        {
            Frame[] bgTileFrames = createBgTileFrames(mTileTypeData, doAnimation);
            if (doFooter)
            {
                drawFooter(bgTileFrames);
            }
            saveImage(bgTileFrames, season, savePath);
        }

        public static void saveImage(byte[] tileTypeData, Season season, bool doAnimation, string savePath)
        {
            Frame[] bgTileFrames = createBgTileFrames(tileTypeData, doAnimation);
            saveImage(bgTileFrames, season, savePath);
        }

        private static void saveImage(Frame[] bgTileFrames, Season season, string savePath)
        {
            if (bgTileFrames.Length > 1) //Multiple frames i.e. it's an animation?
            {
                //Make sure save file path ends with ".gif" if saving city as an animation.
                if (!savePath.EndsWith(".gif", StringComparison.OrdinalIgnoreCase))
                {
                    savePath += ".gif";
                }

                //Animated bg tiles.
                Color[][] animatedPalettes = createAnimatedPalettes(season, true); //8 frames, no seasons.

                //Animated bg tiles and seasons.
                //Maybe looks a bit too weird and produces too big GIF files to be useful, but fun to watch.
                //Color[][] animatedPalettes = createAnimatedPalettes(10, 4, false); //56 frames, looks ok.
                //Color[][] animatedPalettes = createAnimatedPalettes(15, 6, false); //84 frames, looks pretty good.
                //Color[][] animatedPalettes = createAnimatedPalettes(20, 8, false); //112 frames, looks good. Big file though.

                GifWriterCached gif = new GifWriterCached(null, 0);
                for (int i = 0; i < animatedPalettes.Length; i++)
                {
                    gif.addFrame(bgTileFrames[i % bgTileFrames.Length], animatedPalettes[i], GifDelay);
                }
                gif.save(savePath);
            }
            else
            {
                Color[] palette = createStaticPalette(season);
                bgTileFrames[0].save(savePath, palette);
            }
        }

        private void drawFooter(Frame[] bgTileFrames)
        {
            Font font2 = SimCity.Font2;

            string footerLeft = string.Format("SimCity (SNES) - Map {0}, {1}, |~{2}", MapNumber, Year, Population);
            Point dstLeft = new Point(Margin.Width, bgTileFrames[0].Height - font2.CharTileSize.Height);

            foreach (Frame bgTileFrame in bgTileFrames)
            {
                font2.draw(footerLeft, dstLeft, bgTileFrame, FontCreamRemap);
            }
        }

        private void drawFooterSpecialScenarios(Frame[] bgTileFrames, string title) //Special for my vgmaps.com contribution.
        {
            Font font2 = SimCity.Font2;

            string footerLeft = string.Format("SimCity (SNES) - {0}, {1}, |~{2}", title, Year, Population);
            Point dstLeft = new Point(Margin.Width, bgTileFrames[0].Height - font2.CharTileSize.Height);

            string footerRight = "Compiled 20211229 by mechaskrom@gmail.com";
            Point dstRight = new Point(
                bgTileFrames[0].Width - (font2.CharTileSize.Width * footerRight.Length) - Margin.Width,
                bgTileFrames[0].Height - font2.CharTileSize.Height);

            foreach (Frame bgTileFrame in bgTileFrames)
            {
                font2.draw(footerLeft, dstLeft, bgTileFrame, FontCreamRemap);
                font2.draw(footerRight, dstRight, bgTileFrame, FontCreamRemap);
            }
        }

        private static void drawFooterSpecialMinimaps(Frame frame, bool isAltSet) //Special for my vgmaps.com contribution.
        {
            Font font2 = SimCity.Font2;

            if (isAltSet)
            {
                string text = "SimCity (SNES) alternate version maps. Unavailable in the japanese version of the game.";
                text += "\nStart the map's normal version and then return to the map select screen to load the alternate version.";
                text += "\nOr set value at RAM address 0x7E0B2A to 0xFF before loading the map in the map select screen.";

                Point dstLeft = new Point(0, frame.Height - (font2.CharTileSize.Height * 3));
                font2.draw(text, dstLeft, frame, FontCreamRemap);
            }
            else
            {
                Point dstLeft = new Point(0, frame.Height - (font2.CharTileSize.Height * 1));
                font2.draw("SimCity (SNES) normal version maps.", dstLeft, frame, FontCreamRemap);
            }

            string footerRight = "Compiled 20211229 by mechaskrom@gmail.com";
            Point dstRight = new Point(
                frame.Width - (font2.CharTileSize.Width * footerRight.Length),
                frame.Height - font2.CharTileSize.Height);
            font2.draw(footerRight, dstRight, frame, FontCreamRemap);
        }

        public void saveImageMinimap(string savePath)
        {
            saveImageMinimap(mTileTypeData, false, false, savePath);
        }

        public void saveImagePreview(bool fixForest, string savePath)
        {
            saveImageMinimap(mTileTypeData, true, fixForest, savePath);
        }

        public static void saveImageMinimap(byte[] tileTypeData, bool doPreview, bool fixForest, string savePath)
        {
            Frame minimapFrame = getMinimapFrame(tileTypeData, doPreview, fixForest);
            Color[] palette = createMinimapPalette(doPreview);
            minimapFrame.save(savePath, palette);
        }

        private static Frame getMinimapFrame(byte[] tileTypeData, bool doPreview, bool fixForest)
        {
            //Minimap: shown when checking things in the city e.g. traffic, land value, crime, etc.
            //Preview: shown in the map select screen. Pretty much same as the minimap, but uses
            //a slightly different palette to change the color of land and forest tiles.

            Frame minimapFrame = new Frame(WidthInTiles, HeightInTiles);
            for (int i = 0; i < minimapFrame.Length; i++)
            {
                int tileType = tileTypeData.read16bit((i * 2)) & 0x03FF;
                int miniCol = TileType.getMinimapColor(tileType);

                //Forest tiles in the preview are a bit weird as some use the same color as water tiles.
                //I wonder if that was intentional or a mistake? Let's change it and see how it looks.
                if (doPreview && fixForest &&
                    tileType >= 20 && tileType <= 37 && miniCol == 13) //Forest tile with water color?
                {
                    //8, 10 and 12 are green colors. Only 8, 10 and 13 are used by forest tiles.
                    //So it seems most likely that 13 was meant to be 12?
                    miniCol = 12;
                }
                minimapFrame[i] = (byte)miniCol;
            }
            return minimapFrame;
        }

        public static void saveImageMinimapAll()
        {
            //Save a minimap of all 2000 maps.
            string folderSavePath = Program.TempFolder + "minimaps\\";
            Directory.CreateDirectory(folderSavePath);
            Console.WriteLine("Saving 2000 minimaps. This may take a while.");
            List<byte[]> tileTypeData = SimCity.getTileTypeDataAllMaps();
            for (int i = 0; i < tileTypeData.Count; i++)
            {
                saveImageMinimap(tileTypeData[i], true, true, folderSavePath + i.ToString("D4") + " image preview.png");
                Console.Write("\r" + (i + 1).ToString() + "/2000");
            }
        }

        public static void saveImageMinimapAllSpecialGrid() //Special for my vgmaps.com contribution.
        {
            //Save a minimap of all 2000 maps in a grid split into two images (normal and alternate versions).
            List<byte[]> tileTypeData = SimCity.getTileTypeDataAllMaps();
            Color[] palette = createMinimapPalette(true);
            //10*100 minimaps with a 2 tiles spacing between them.
            int widthPerMinimap = WidthInTiles + (TileWidth * 2);
            int heightPerMinimap = HeightInTiles + (TileHeight * 2);
            Frame frame = new Frame((widthPerMinimap * 10) - (TileWidth * 2), heightPerMinimap * 100 + (TileHeight * 2));
            Font font2 = SimCity.Font2;
            Point dstPos = new Point(0, TileHeight * 2);
            for (int i = 0; i < tileTypeData.Count; i++)
            {
                Frame minimapFrame = getMinimapFrame(tileTypeData[i], true, true);
                frame.write(minimapFrame, dstPos);
                //Draw the map number above the top left corner.
                font2.draw(i.ToString("D4"), dstPos.getOffset(0, -font2.CharTileSize.Height), frame, FontCreamRemap);
                if (i == 999) //0000-0999 (normal versions) done?
                {
                    //Save the first image and start the second one.
                    drawFooterSpecialMinimaps(frame, false);
                    frame.save(Program.TempFolder + "minimaps grid 1.png", palette);
                    dstPos = new Point(0, TileHeight * 2);

                    //The second frame needs a bit more space at the bottom for some extra text lines.
                    frame = new Frame(frame.Width, frame.Height + (TileHeight * 2));
                }
                else if (i % 10 == 9) //Start a new row? 10 minimaps per row.
                {
                    dstPos.X = 0;
                    dstPos.Y += heightPerMinimap;
                }
                else
                {
                    dstPos.X += widthPerMinimap;
                }
            }
            drawFooterSpecialMinimaps(frame, true);
            frame.save(Program.TempFolder + "minimaps grid 2.png", palette);
        }

        public static void saveImageScenarios() //Special for my vgmaps.com contribution.
        {
            //Save images of all scenarios from states I saved at the start of them.

            string folderSavePath = Program.TempFolder + "scenarios\\";
            Directory.CreateDirectory(folderSavePath);

            string[] scNames = { "Bern", "Boston", "Detroit", "Free", "Las Vegas", "Practice", "Rio de Janeiro", "San Francisco", "Tokyo" };
            foreach (string scName in scNames)
            {
                string statePath = Program.FilesFolder + "scenarios\\sc " + scName + ".state";
                StateFileSnes9x state = StateFileSnes9x.open(statePath);

                string savePath = folderSavePath + scName;
                SaveFileState ss = SaveFileState.open(state);
                ss.getCities()[0].saveImageScenario(Season.Summer, false, scName, savePath + ".png");
                ss.getCities()[0].saveImageScenario(Season.Summer, true, scName, savePath + ".gif");
            }

            //I wonder how the scenarios are stored in the ROM? Most(?) logical would be that their tile type
            //data is stored compressed in the same way as cities saved to the 32KB cartridge battery SRAM.
            //I tried searching for this compressed tile type data in the ROM, but found nothing.
            //Scenarios use a different kind of compression?
        }

        private void saveImageScenario(Season season, bool doAnimation, string title, string savePath)
        {
            Frame[] bgTileFrames = createBgTileFrames(mTileTypeData, doAnimation);
            drawFooterSpecialScenarios(bgTileFrames, title);
            saveImage(bgTileFrames, season, savePath);
        }

        private static Frame[] createBgTileFrames(byte[] tileTypeData, bool doAnimation)
        {
            Frame[] tileFrames = new Frame[doAnimation ? 4 : 1];
            for (int i = 0; i < tileFrames.Length; i++)
            {
                tileFrames[i] = new Frame(Width + (Margin.Width * 2), Height + (Margin.Height * 2));
            }

            Frame bgTileSheet = SimCity.getBgTileSheetFrame();

            byte[] tileBuffer = new byte[TileWidth * TileHeight];
            Rectangle srcRect = new Rectangle(0, 0, TileWidth, TileHeight);
            Point dstPos = new Point(Margin.Width, Margin.Height); //Start after margin.
            int ttDataInd = 0;
            for (int tileY = 0; tileY < HeightInTiles; tileY++, dstPos.Y += TileHeight)
            {
                dstPos.X = Margin.Width; //Start after margin.
                for (int tileX = 0; tileX < WidthInTiles; tileX++, dstPos.X += TileWidth, ttDataInd += 2)
                {
                    int tileType = tileTypeData.read16bit(ttDataInd);

                    int palRow, tileInd, tileTopInd;
                    TileType.get(tileType, out tileInd, out tileTopInd, out palRow);

                    drawBgTile(tileInd, bgTileSheet, dstPos, palRow, tileFrames);

                    if (tileTopInd != 768) //Tile type has a top tile?
                    {
                        drawBgTile(tileTopInd, bgTileSheet, dstPos.getOffset(-TileWidth, -TileHeight), palRow, tileFrames);
                    }
                }
            }
            return tileFrames;
        }

        private static void drawBgTile(int tileInd, Frame bgTileSheet, Point dstPos, int palRow, Frame[] frames)
        {
            Rectangle srcRect = new Rectangle((tileInd % 32) * TileWidth, (tileInd / 32) * TileHeight, TileWidth, TileHeight);
            int srcRectYAdd;
            if (tileInd < 864) //Static tile?
            {
                srcRectYAdd = 0; //Repeat same tile.
            }
            else //Animated tile.
            {
                srcRectYAdd = 5 * TileHeight; //Go to next tile bank. 5 rows per bank.
            }

            for (int i = 0; i < frames.Length; i++)
            {
                frames[i].draw(bgTileSheet, srcRect, dstPos, BgPaletteRemaps[palRow]);

                srcRect.Y += srcRectYAdd;
                if (i % 4 == 3) //4 banks of animated tiles. Wrap around every 4th frame.
                {
                    srcRect.Y = (tileInd / 32) * TileHeight;
                }
            }
        }

        private static Color[] createStaticPalette(Season season)
        {
            //Create palette with bg tile colors in row 0-7 and HUD colors in row 8.
            Color[][] palettes = createPalettes(1); //Static == 1 "animated" frame.

            writePaletteSeason(palettes, season, 0); //Bg tiles row 0.
            writePaletteSeason(palettes, season, 1); //Bg tiles row 1.
            writePaletteSeason(palettes, season, 2); //Bg tiles row 2.
            writePaletteSeason(palettes, season, 3); //Bg tiles row 3.
            writePaletteSeason(palettes, season, 4); //Bg tiles row 4.
            writePaletteSeason(palettes, season, 5); //Bg tiles row 5.
            writePaletteSeason(palettes, (Season)0, 6); //Bg tiles row 6. Always cycle 0 regardless of season.
            writePaletteSeason(palettes, season, 7); //Bg tiles row 7.
            writePaletteHud(palettes, 8); //HUD row 8.

            return palettes[0];
        }

        private static Color[][] createAnimatedPalettes(Season season, bool do8CycleStages)
        {
            //Create palettes where row 6 is animated, but season never changes.

            //Animation frame count should be a multiple of 4 to loop perfectly with tile animations.
            //Palette 6 cycles normally through 7 steps, but we can cheat it to 8 for a better multiple with 4.
            int frameCount = MiscExt.LeastCommonMultiple(4, do8CycleStages ? 8 : 7);
            Color[][] palettes = createPalettes(frameCount);

            writePaletteSeason(palettes, season, 0); //Bg tiles row 0.
            writePaletteSeason(palettes, season, 1); //Bg tiles row 1.
            writePaletteSeason(palettes, season, 2); //Bg tiles row 2.
            writePaletteSeason(palettes, season, 3); //Bg tiles row 3.
            writePaletteSeason(palettes, season, 4); //Bg tiles row 4.
            writePaletteSeason(palettes, season, 5); //Bg tiles row 5.
            writePaletteAnimationCycle(palettes, do8CycleStages, 6); //Bg tiles row 6.
            writePaletteSeason(palettes, season, 7); //Bg tiles row 7.
            writePaletteHud(palettes, 8); //HUD row 8.

            return palettes;
        }

        private static Color[][] createAnimatedPalettes(int seasonFrames, int transitionFrames, bool do8CycleStages)
        {
            //seasonFrames = number of frames that season is static.
            //transitionFrames = number of frames in the transition to the next season.
            //These are added and multiplied with 4 seasons to get the total number of animated palettes.

            //One year is about 107 seconds at the fastest setting in the game. At 0.2 seconds per
            //GIF frame (speed of tile animation) that would mean about 107/0.2=535 frames to cover
            //a full year in a GIF. Probably over 100MB in size so not really feasible.

            //I tested some shorter years like 56 or 112 frames, but still quite large files and it looks
            //a bit weird with seasons changing quickly while other things are animated at normal speed.
            //I'll probably just leave this method here for creating animated seasons. Pretty fun to
            //watch, but not very useful? Maybe in the future if some kind of video output is used
            //instead of animated GIFs?

            int frameCount = MiscExt.LeastCommonMultiple(4, do8CycleStages ? 8 : 7);
            frameCount = MiscExt.LeastCommonMultiple(frameCount, (seasonFrames + transitionFrames) * 4);
            Color[][] palettes = createPalettes(frameCount);

            writePaletteSeason(palettes, 0, 0); //Bg tiles row 0.
            writePaletteAnimationSeason(palettes, seasonFrames, transitionFrames, 1); //Bg tiles row 1.
            writePaletteAnimationSeason(palettes, seasonFrames, transitionFrames, 2); //Bg tiles row 2.
            writePaletteAnimationSeason(palettes, seasonFrames, transitionFrames, 3); //Bg tiles row 3.
            writePaletteAnimationSeason(palettes, seasonFrames, transitionFrames, 4); //Bg tiles row 4.
            writePaletteSeason(palettes, 0, 5); //Bg tiles row 5.
            writePaletteAnimationCycle(palettes, do8CycleStages, 6); //Bg tiles row 6.
            writePaletteAnimationSeason(palettes, seasonFrames, transitionFrames, 7); //Bg tiles row 7.
            writePaletteHud(palettes, 8); //HUD row 8.

            return palettes;
        }

        private static Color[] createMinimapPalette(bool doPreview)
        {
            //Create palette with preview or minimap colors in row 0 and HUD colors in row 8 (for the font remap tables).
            Color[][] palettes = createPalettes(1); //Static == 1 "animated" frame.

            writePaletteMinimap(palettes, doPreview, 0); //Minimap row 0.
            writePaletteHud(palettes, 8); //HUD row 8.

            return palettes[0];
        }

        private static Color[][] createPalettes(int count)
        {
            //Returns 'count' palettes each with 256 colors.
            if (count > 500)
            {
                //Throw if a very high count was specified (e.g. trying to create a really long animation).
                //It will probably work, but probably also not a very good idea.
                throw new ArgumentException(count + " palettes requested which is maybe a bit too much?!");
            }
            Color[][] paletteFrames = new Color[count][]; //Count * 256 colors.
            for (int i = 0; i < paletteFrames.Length; i++)
            {
                paletteFrames[i] = new Color[256];
            }
            return paletteFrames;
        }

        private static void writePaletteSeason(Color[][] palettes, Season season, int row)
        {
            writePaletteSeason(palettes, season, row, row);
        }

        private static void writePaletteSeason(Color[][] palettes, Season season, int srcRow, int dstRow)
        {
            ColorRow colorRow = SimCity.getSnesColorRowBg(season, srcRow).toColorRow();
            for (int i = 0; i < palettes.Length; i++)
            {
                writeColorRow(palettes[i], colorRow, dstRow);
            }
        }

        private static void writePaletteAnimationSeason(Color[][] palettes, int seasonFrames, int transitionFrames, int row)
        {
            writePaletteAnimationSeason(palettes, seasonFrames, transitionFrames, row, row);
        }

        private static void writePaletteAnimationSeason(Color[][] palettes, int seasonFrames, int transitionFrames, int srcRow, int dstRow)
        {
            if ((seasonFrames + transitionFrames) * 4 != palettes.Length)
            {
                throw new ArgumentException("Specified season and transition frame counts doesn't match number of palettes!");
            }

            ColorRow[] seasonRows = new ColorRow[]
            {
                SimCity.getSnesColorRowBg(Season.Winter, srcRow).toColorRow(),
                SimCity.getSnesColorRowBg(Season.Spring, srcRow).toColorRow(),
                SimCity.getSnesColorRowBg(Season.Summer, srcRow).toColorRow(),
                SimCity.getSnesColorRowBg(Season.Autumn, srcRow).toColorRow(),
            };

            for (int nSeas = 0, nFrame = 0; nSeas < 4; nSeas++) //Four seasons.
            {
                for (int i = 0; i < seasonFrames; i++, nFrame++) //Frames to show season.
                {
                    writeColorRow(palettes[nFrame], seasonRows[nSeas], dstRow);
                }

                if (transitionFrames > 0) //Frames to do the transition to the next season.
                {
                    ColorRow fromRow = seasonRows[nSeas];
                    ColorRow toRow = seasonRows[(nSeas + 1) % seasonRows.Length]; //Wrap around (autumn -> winter).
                    ColorRow[] transitionRows = createPaletteTransition(fromRow, toRow, transitionFrames);
                    for (int i = 0; i < transitionFrames; i++, nFrame++)
                    {
                        writeColorRow(palettes[nFrame], transitionRows[i], dstRow);
                    }
                }
            }
        }

        private static void writePaletteAnimationCycle(Color[][] palettes, bool do8CycleStages, int dstRow)
        {
            //Bg row 6 cycles repeatedly the same colors.

            //I frame advanced the game and each stage is displayed for 12 frames except
            //the start which is displayed for 24 frames (i.e. its repeated once).
            //Tile banks are also displayed for 12 frames so this should mean that the
            //frame delay in a GIF should be (1/60)*12=0.2 seconds or 20 centiseconds.

            //Unfortunately this means that we need 4*7 animation frames (4 tile banks
            //and 7 palette banks) to loop perfectly in a GIF.
            //I'm tempted to cheat a bit and repeat the end stage one more time to get 8 palette
            //banks. Then you would only need 8 frames (a multiple of both 4 and 8) in a GIF.

            ColorRow rowStep0 = SimCity.getSnesColorRowBg(0).toColorRow();
            ColorRow rowStep1 = SimCity.getSnesColorRowBg(1).toColorRow();
            ColorRow rowStep2 = SimCity.getSnesColorRowBg(2).toColorRow();
            ColorRow rowStep3 = SimCity.getSnesColorRowBg(3).toColorRow();

            ColorRow[] rows;
            if (do8CycleStages) //Cheat 8 stages for a better multiple with tile animations.
            {
                rows = new ColorRow[]
                {
                    rowStep0, //Start (brightest).
                    rowStep1,
                    rowStep2,
                    rowStep3, //End (darkest).
                    rowStep3, //End (darkest). <- Insert an extra end stage.
                    rowStep2,
                    rowStep1,
                    rowStep0, //Start (brightest).
                };
            }
            else //The game uses 7 stages. Not a good multiple with tile animations.
            {
                rows = new ColorRow[]
                {
                    rowStep0, //Start (brightest).
                    rowStep1,
                    rowStep2,
                    rowStep3, //End (darkest).
                    rowStep2,
                    rowStep1,
                    rowStep0, //Start (brightest).
                };
            }

            for (int i = 0; i < palettes.Length; i++)
            {
                ColorRow colorRow = rows[i % rows.Length];
                writeColorRow(palettes[i], colorRow, dstRow);
            }
        }

        private static void writePaletteHud(Color[][] palettes, int dstRow)
        {
            //Writes the HUD (upper main menu, left building icons, text, etc. shown ingame) palette.
            //It looks like row 5 is never used by any bg tiles so we could write the HUD palette
            //there to keep the total number of palette rows needed at 8. Meaning we would only
            //need a 7-bit color depth in a GIF (8 rows * 16 colors = 128 colors total).
            //But I tested 7-bit vs 8-bit GIF in both short and long animations and it only reduced
            //file size with a few percent so not really worth it. It's probably better to just
            //use 8-bit GIF and write the HUD palette to a row after the 8 rows used by bg tiles.
            ColorRow colorRow = SimCity.getSnesColorRowHud().toColorRow();
            for (int i = 0; i < palettes.Length; i++)
            {
                writeColorRow(palettes[i], colorRow, dstRow);
            }
        }

        private static void writePaletteMinimap(Color[][] palettes, bool doPreview, int dstRow)
        {
            //Writes the minimap or preview palette.
            SnesColorRow snesColorRow = doPreview ? SimCity.getSnesColorRowPreview() : SimCity.getSnesColorRowMinimap();
            ColorRow colorRow = snesColorRow.toColorRow();
            for (int i = 0; i < palettes.Length; i++)
            {
                writeColorRow(palettes[i], colorRow, dstRow);
            }
        }

        private static void writeColorRow(Color[] palette, ColorRow srcRow, int dstRow) //Write a row (16 colors) to palette (256 colors).
        {
            Array.Copy(srcRow.Entries, 0, palette, dstRow * ColorRow.Length, ColorRow.Length);
        }

        private static ColorRow[] createPaletteTransition(ColorRow fromRow, ColorRow toRow, int stages)
        {
            //Creates a linear transition between the 'from' and 'to' color rows.
            //This is not how the game transitions between seasons, but it looks pretty good and is simpler.

            if (stages < 1)
            {
                throw new ArgumentException("Stages must be 1 or larger!");
            }

            ColorRow[] stageRows = new ColorRow[stages];

            //Calculate how much to add per stage per color channel.
            float[,] adds = new float[ColorRow.Length, 3]; //3 channels per color (rgb).
            float addsCount = stages + 1;
            for (int i = 0; i < ColorRow.Length; i++)
            {
                adds[i, 0] = (toRow[i].R - fromRow[i].R) / addsCount;
                adds[i, 1] = (toRow[i].G - fromRow[i].G) / addsCount;
                adds[i, 2] = (toRow[i].B - fromRow[i].B) / addsCount;
            }

            for (int i = 1; i <= stageRows.Length; i++)
            {
                ColorRow row = new ColorRow();
                for (int j = 0; j < ColorRow.Length; j++)
                {
                    row[j] = Color.FromArgb(
                        fromRow[j].R + (int)((adds[j, 0] * i) + 0.5f),
                        fromRow[j].G + (int)((adds[j, 1] * i) + 0.5f),
                        fromRow[j].B + (int)((adds[j, 2] * i) + 0.5f));
                }
                stageRows[i - 1] = row;
            }
            return stageRows;
        }
    }
}
