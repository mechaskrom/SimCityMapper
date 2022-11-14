using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Mechaskrom.Gif;

namespace SimCityMapper
{
    //This is a program I wrote to research/test a few things in the SNES game SimCity.
    //It is also able to output images of whole cities/maps from emulator state files or game save files (32KB battery backed SRAM).
    //Any ROM addresses or data is from the USA version (CRC32 = 8AEDD3A1) unless noted otherwise.
    class Program
    {
        public static readonly string DebugFolder = getDebugBasePath();
        public static readonly string FilesFolder = DebugFolder + @"files\";
        public static readonly string StatesFolder = FilesFolder + @"states\";
        public static readonly string TempFolder = DebugFolder + @"temp\";
        public static readonly string RomPath = DebugFolder + @"roms\SimCity (USA).sfc"; //Unheadered, CRC32 = 8AEDD3A1.

        private static string getDebugBasePath()
        {
            //Return base path used when doing debug tests and saves i.e. running program within visual studio.
            //Use the solution folder and assume it is three levels up from the exe-file in the debug folder.
            //Projects\SimCityMapper\SimCityMapper\bin\Debug
            string exePath = AppDomain.CurrentDomain.BaseDirectory.GetFullPathWithEndingSeparator();
            DirectoryInfo di = Directory.GetParent(exePath); //Get exe folder.
            return di.Parent.Parent.Parent.FullName.GetFullPathWithEndingSeparator(); //Get solution folder.
        }

        static void Main(string[] args)
        {
#if DEBUG
            //****************************************************************************************************************
            //Use these methods to extract data from the game.

            //SimCity.extractTileTypeData();

            //SimCity.extractTileTypeDataMinimapWrite();
            //SimCity.extractTileTypeDataMinimapRead();

            //SimCity.extractPaletteData();

            //SimCity.extractBgTileSheet();

            //****************************************************************************************************************
            //Test different save image methods on an example city.

            //StateFileSnes9x sf = StateFileSnes9x.open(StatesFolder + "example.state"); //Snes9x.
            ////StateFileZsnes sf = StateFileZsnes.open(StatesFolder + "example.zst"); //ZSNES.
            //City city = SaveFileState.open(sf).getCities()[0];
            ////City city = SaveFileSram.open(sf).getCities()[0]; //Test with SRAM in state file.
            //city.saveImage(city.Season, true, TempFolder + "example.state image.png");
            //city.saveImageAnimated(city.Season, true, TempFolder + "example.state image.gif");
            //city.saveImageMinimap(TempFolder + "example.state image minimap.png");
            //city.saveImagePreview(false, TempFolder + "example.state image preview.png");

            //****************************************************************************************************************
            //Test SRAM decompression. A city with many airports is an easy way to make saves use compression.

            //StateFileSnes9x sf = StateFileSnes9x.open(StatesFolder + "airports.state");
            //SaveFileState.open(sf).getCities()[0].saveImage(TempFolder + "airports.state ram image.png");
            //SaveFileSram.open(sf).getCities()[0].saveImage(TempFolder + "airports.state srm image.png");

            //****************************************************************************************************************
            //Test if game can save city with random tiles.

            //SimCity.createTestRandomCityState();

            //****************************************************************************************************************
            //Save image and map info of all maps.

            //City.saveImageMinimapAll();
            //MapInfo.saveMapInfoAll();

            //****************************************************************************************************************
            //Special save image methods for my vgmaps.com (and gamefaqs.com) contribution.

            //City.saveImageMinimapAllSpecialGrid();
            //City.saveImageScenarios();

            //****************************************************************************************************************
#else
            try
            {
                string saveFilePath;
                Season? forceSeason;
                bool doAnimation;
                bool doFooter;
                parseCommandLine(args, out saveFilePath, out forceSeason, out doAnimation, out doFooter);

                SaveFile sf = SaveFile.tryOpen(saveFilePath);
                City[] cities = sf.getCities();
                foreach (City city in cities)
                {
                    string path = saveFilePath + " image";
                    if (city.Origin == CityOrigin.Slot1)
                    {
                        path += " slot 1";
                    }
                    else if (city.Origin == CityOrigin.Slot2)
                    {
                        path += " slot 2";
                    }

                    //Use season in saved city if user didn't specify any.
                    Season season = forceSeason.HasValue ? forceSeason.Value : city.Season;
                    if (doAnimation)
                    {
                        city.saveImageAnimated(season, doFooter, path + ".gif");
                    }
                    else
                    {
                        city.saveImage(season, doFooter, path + ".png");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Couldn't save image! Reason: " + ex.Message);
            }
#endif
            Bench.saveResults(DebugFolder + "log.txt");
        }

        static void parseCommandLine(string[] args, out string saveFilePath, out Season? forceSeason, out bool doAnimation, out bool doFooter)
        {
            try
            {
                if (args.Length < 1)
                {
                    throw new ArgumentException("Too few arguments!");
                }
                saveFilePath = args[0];
                forceSeason = null;
                doAnimation = false;
                doFooter = false;
                for (int i = 1; i < args.Length; i++)
                {
                    string arg = args[i];
                    if (arg == "-s")
                    {
                        i++;
                        string opt = i < args.Length ? args[i] : null;
                        if (opt == "winter") forceSeason = Season.Winter;
                        else if (opt == "spring") forceSeason = Season.Spring;
                        else if (opt == "summer") forceSeason = Season.Summer;
                        else if (opt == "autumn" || opt == "fall") forceSeason = Season.Autumn;
                        else throw new ArgumentException("Unknown or missing season specifier after '-s' argument!");
                    }
                    else if (arg == "-a")
                    {
                        doAnimation = true;
                    }
                    else if (arg == "-f")
                    {
                        doFooter = true;
                    }
                }
            }
            catch (ArgumentException ex)
            {
                string usage = "\n\nUsage: SimCityMapper infile [options]";
                usage += "\n\nInfile is path to a supported file type:";
                usage += "\n-Snes9x emulator savestate.";
                usage += "\n-ZSNES emulator savestate.";
                usage += "\n-32KB battery backup SRAM.";
                usage += "\n\nOptions:";
                usage += "\n-s winter|spring|summer|autumn|fall   Override season in infile.";
                usage += "\n-a   Save as an animated image.";
                usage += "\n-f   Add a footer with some data about the city.";
                usage += "\n\nExample: SimCityMapper \"C:\\my cities\\donuts.srm\" -s autumn -a -f";
                throw new ArgumentException("Couldn't parse command line! Reason: " + ex.Message + usage);
            }
        }
    }
}
