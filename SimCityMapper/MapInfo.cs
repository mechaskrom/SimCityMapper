using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SimCityMapper
{
    class MapInfo
    {
        private static readonly string MapInfoFolder = Program.TempFolder + @"map info\";

        private const int CityTileCount = City.WidthInTiles * City.HeightInTiles;

        private readonly int mNumber;
        private readonly bool mIsIsland;
        private readonly int mLandCount;
        private readonly int mForestCount;
        private readonly int mShoreCount;
        private readonly int mWaterCount;
        private readonly int mCenterShoreCount;
        private readonly int mCenterWaterCount;

        private MapInfo(int number, bool isIsland, int landCount, int forestCount, int shoreCount, int waterCount, int centerShoreCount, int centerWaterCount)
        {
            mNumber = number;
            mIsIsland = isIsland;
            mLandCount = landCount;
            mForestCount = forestCount;
            mShoreCount = shoreCount;
            mWaterCount = waterCount;
            mCenterShoreCount = centerShoreCount;
            mCenterWaterCount = centerWaterCount;
        }

        public int Number
        {
            get { return mNumber; }
        }

        public bool IsIsland
        {
            get { return mIsIsland; }
        }

        public int LandCount
        {
            get { return mLandCount; }
        }

        public int ForestCount
        {
            get { return mForestCount; }
        }

        public int ShoreCount
        {
            get { return mShoreCount; }
        }

        public int WaterCount
        {
            get { return mWaterCount; }
        }

        public int CenterWaterCount
        {
            get { return mCenterWaterCount; }
        }

        public int CenterShoreCount
        {
            get { return mCenterShoreCount; }
        }

        public int Score
        {
            get
            {
                //Calculate a score in a way designed mainly for scoring mainland (i.e. not island) maps.
                //Add water and shore tiles. Center water and shore tiles are a bit worse
                //so let them affect the score slightly more (60% vs 40%).
                //Subtract result from total tiles just to get a score where higher (instead of lower) is better.

                //You can build on shore tiles, but they are still almost as bad as water tiles because
                //a lot of shore tiles indicates a map has many bodies of water, peninsulas and/or islands
                //which can make city building more difficult.

                int fringeShoreCount = mShoreCount - mCenterShoreCount;
                int fringeWaterCount = mWaterCount - mCenterWaterCount;
                return CityTileCount - //Total tiles minus...
                    ((((mCenterShoreCount + mCenterWaterCount) * 3) + //Center water and shore tiles (60%).
                    ((fringeShoreCount + fringeWaterCount) * 2)) / 5); //Outside center water and shore tiles (40%).
            }
        }

        public static MapInfo create(byte[] tileTypeData, int number)
        {
            //Count number of tile types in each group. All can be built on except the water group.
            //Map generator seems to only use tile types 0-37. Water tile type 3 is never used though, only 1 and 2?
            int landCount = 0;
            int forestCount = 0;
            int shoreCount = 0;
            int waterCount = 0;

            //Also count number of water and shore tiles in the center. In a diamond shape like the land value boost
            //from being near the city center. It's easier to develop zones in this diamond center because of the
            //land value boost so how much water and shore is in there can be interesting info when choosing a map.
            int centerWaterCount = 0;
            int centerShoreCount = 0;

            //Count water tiles in corners. If in more than two corners then it's probably an island map.
            int cornerWaterCount = 0;

            bool[,] isCenterTile = getIsCenterTiles(9); //Get the center diamond shape.
            for (int y = 0, i = 0; y < City.HeightInTiles; y++)
            {
                for (int x = 0; x < City.WidthInTiles; x++, i += 2)
                {
                    int tileType = tileTypeData.read16bit(i) & 0x03FF;
                    if (tileType <= 0) //0 = empty land.
                    {
                        landCount++;
                    }
                    else if (tileType <= 3) //1-3 = water.
                    {
                        waterCount++;

                        if (isCenterTile[y, x]) centerWaterCount++;

                        //Corner water tile?
                        if ((y == 0 && (x == 0 || x == (City.WidthInTiles - 1))) || //NW or NE corner.
                            (y == (City.HeightInTiles - 1) && (x == 0 || x == (City.WidthInTiles - 1)))) //SW or SE corner.
                        {
                            cornerWaterCount++;
                        }
                    }
                    else if (tileType <= 19) //4-19 = shore.
                    {
                        shoreCount++;

                        if (isCenterTile[y, x]) centerShoreCount++;
                    }
                    else if (tileType <= 37) //20-37 = forest.
                    {
                        forestCount++;
                    }
                    else throw new NotImplementedException("Unexpected tile type in generated map!");
                }
            }
            //Make sure we counted all tiles.
            System.Diagnostics.Debug.Assert((landCount + forestCount + shoreCount + waterCount) == CityTileCount);

            bool isIsland = cornerWaterCount > 2;
            return new MapInfo(number, isIsland, landCount, forestCount, shoreCount, waterCount, centerShoreCount, centerWaterCount);
        }

        private static bool[,] getIsCenterTiles(int size)
        {
            //Return a 2D array with tiles considered inside the center diamond shape.
            //Size = tiles between the top/bottom edge and the diamond shape.
            int xStart = (City.WidthInTiles / 2) - 1; //Start at half width.
            int xEnd = City.WidthInTiles / 2;
            int xStartAdd = -1; //Increase diamond width every next line.
            int xEndAdd = 1;
            int yStart = size; //Start and end y row (from top and from bottom) i.e. size of the diamond shape.
            bool[,] isCenterTiles = new bool[City.HeightInTiles, City.WidthInTiles];
            for (int y = yStart; y < (City.HeightInTiles - yStart); y++)
            {
                for (int x = xStart; x <= xEnd; x++)
                {
                    isCenterTiles[y, x] = true;
                }

                if (y == (City.HeightInTiles / 2) - 1) //Reached half height?
                {
                    xStartAdd *= -1; //Decrease diamond width every next line.
                    xEndAdd *= -1;
                }
                else
                {
                    xStart += xStartAdd;
                    xEnd += xEndAdd;
                }
            }
            return isCenterTiles;
        }

        public static void saveMapInfoAll()
        {
            //Save info of all 2000 maps.
            List<MapInfo> miAll = new List<MapInfo>();
            List<byte[]> ttData = SimCity.getTileTypeDataAllMaps();
            for (int i = 0; i < ttData.Count; i++)
            {
                miAll.Add(MapInfo.create(ttData[i], i));
            }

            //Custom list of some maps found so far that look interesting to me.
            int[] numCustom = { 31, 61, 116, 137, 182, 201, 343, 630, 641, 714, 728, 753, 849, 983, 1000, 1112, 1356, 1367, 1492, 1582, 1673, 1906 };
            List<MapInfo> miCustom = miAll.FindAll((x) => numCustom.Contains(x.Number));
            miCustom.Sort((x, y) => x.Number.CompareTo(y.Number));
            saveMapInfoTextAndPreview(miCustom, miCustom.Count, ttData, MapInfoFolder + "Custom.txt", MapInfoFolder + "custom\\");

            //All maps ordered by number.
            saveMapInfoText(miAll, miAll.Count, MapInfoFolder + "All.txt");
            saveMapInfoTextShort(miAll, miAll.Count, MapInfoFolder + "All short.txt");

            //All maps ordered by water count.
            miAll.Sort((x, y) => x.WaterCount.CompareTo(y.WaterCount));
            saveMapInfoText(miAll, miAll.Count, MapInfoFolder + "AllWater.txt");

            //All maps ordered by shore count.
            miAll.Sort((x, y) => x.ShoreCount.CompareTo(y.ShoreCount));
            saveMapInfoText(miAll, miAll.Count, MapInfoFolder + "AllShore.txt");

            //All mainland maps ordered by water count.
            List<MapInfo> miMainland = miAll.FindAll((x) => !x.IsIsland);
            miMainland.Sort((x, y) => x.WaterCount.CompareTo(y.WaterCount));
            saveMapInfoTextAndPreview(miMainland, 25, ttData, MapInfoFolder + "MainlandWaterTop25.txt", MapInfoFolder + "mainland water top25\\");
            saveMapInfoTextAndPreview(miMainland, -25, ttData, MapInfoFolder + "MainlandWaterBot25.txt", MapInfoFolder + "mainland water bot25\\");
            saveMapInfoTextShort(miMainland, 25, MapInfoFolder + "MainlandWaterTop25 short.txt");

            //All mainland maps ordered by shore count.
            miMainland.Sort((x, y) => x.ShoreCount.CompareTo(y.ShoreCount));
            saveMapInfoTextAndPreview(miMainland, 25, ttData, MapInfoFolder + "MainlandShoreTop25.txt", MapInfoFolder + "mainland shore top25\\");
            saveMapInfoTextAndPreview(miMainland, -25, ttData, MapInfoFolder + "MainlandShoreBot25.txt", MapInfoFolder + "mainland shore bot25\\");

            //All mainland maps ordered by center water+shore count.
            miMainland.Sort((x, y) => (x.CenterWaterCount + x.CenterShoreCount).CompareTo(y.CenterWaterCount + y.CenterShoreCount));
            saveMapInfoTextAndPreview(miMainland, 25, ttData, MapInfoFolder + "MainlandCenterTop25.txt", MapInfoFolder + "mainland center top25\\");
            saveMapInfoTextAndPreview(miMainland, -25, ttData, MapInfoFolder + "MainlandCenterBot25.txt", MapInfoFolder + "mainland center bot25\\");

            //All mainland maps ordered by score.
            miMainland.Sort((x, y) => y.Score.CompareTo(x.Score)); //Order descending (higher == better).
            saveMapInfoTextAndPreview(miMainland, 25, ttData, MapInfoFolder + "MainlandScoreTop25.txt", MapInfoFolder + "mainland score top25\\");
            saveMapInfoTextAndPreview(miMainland, -25, ttData, MapInfoFolder + "MainlandScoreBot25.txt", MapInfoFolder + "mainland score bot25\\");

            //All island maps ordered by water count.
            List<MapInfo> miIsland = miAll.FindAll((x) => x.IsIsland);
            miIsland.Sort((x, y) => x.WaterCount.CompareTo(y.WaterCount));
            saveMapInfoTextAndPreview(miIsland, 25, ttData, MapInfoFolder + "IslandWaterTop25.txt", MapInfoFolder + "island water top25\\");
            saveMapInfoTextAndPreview(miIsland, -25, ttData, MapInfoFolder + "IslandWaterBot25.txt", MapInfoFolder + "island water bot25\\");
            saveMapInfoTextShort(miIsland, 25, MapInfoFolder + "IslandWaterTop25 short.txt");

            //All island maps ordered by shore count.
            miIsland.Sort((x, y) => x.ShoreCount.CompareTo(y.ShoreCount));
            saveMapInfoTextAndPreview(miIsland, 25, ttData, MapInfoFolder + "IslandShoreTop25.txt", MapInfoFolder + "island shore top25\\");
            saveMapInfoTextAndPreview(miIsland, -25, ttData, MapInfoFolder + "IslandShoreBot25.txt", MapInfoFolder + "island shore bot25\\");
        }

        private static void saveMapInfoText(List<MapInfo> mapInfos, int count, string savePath)
        {
            saveMapInfoText(mapInfos, count, savePath, true);
        }

        private static void saveMapInfoTextShort(List<MapInfo> mapInfos, int count, string savePath)
        {
            saveMapInfoText(mapInfos, count, savePath, false);
        }

        private static void saveMapInfoText(List<MapInfo> mapInfos, int count, string savePath, bool doExtra)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0,-5}{1,9}{2,9}{3,9}{4,9}{5,9}",
                "Map", "Island", "Land", "Forest", "Shore", "Water");
            if (doExtra) //Add a few extra data columns?
            {
                sb.AppendLine("{0,9}{1,9}{2,9}", "CShore", "CWater", "Score");
            }
            else
            {
                sb.AppendLine();
            }
            //If count is negative then use bottom of list.
            int iStart = count < 0 ? (mapInfos.Count + count) : 0;
            int iEnd = iStart + Math.Abs(count);
            for (int i = iStart; i < iEnd; i++)
            {
                MapInfo mi = mapInfos[i];
                string miNumber = "#" + mi.Number.ToString("D4");
                string miIsland = mi.IsIsland ? "yes" : "no";
                sb.AppendFormat("{0,-5}{1,9}{2,9}{3,9}{4,9}{5,9}",
                    miNumber, miIsland, mi.LandCount, mi.ForestCount, mi.ShoreCount, mi.WaterCount);
                if (doExtra) //Add a few extra data columns?
                {
                    sb.AppendLine("{0,9}{1,9}{2,9}", mi.CenterShoreCount, mi.CenterWaterCount, mi.Score);
                }
                else
                {
                    sb.AppendLine();
                }
            }
            File.WriteAllText(savePath, sb.ToString());
        }

        private static void saveMapInfoPreviews(List<MapInfo> mapInfos, int count, List<byte[]> tileTypeData, string saveFolderPath)
        {
            Directory.CreateDirectory(saveFolderPath);
            //If count is negative then use bottom of list.
            int iStart = count < 0 ? (mapInfos.Count + count) : 0;
            int iEnd = iStart + Math.Abs(count);
            for (int i = iStart; i < iEnd; i++)
            {
                string savePath = saveFolderPath + i + " preview " + mapInfos[i].Number.ToString("D4") + ".png";
                City.saveImageMinimap(tileTypeData[mapInfos[i].Number], true, true, savePath);
            }
        }

        private static void saveMapInfoTextAndPreview(List<MapInfo> mapInfos, int count, List<byte[]> tileTypeData,
            string savePathText, string saveFolderPathPreview)
        {
            saveMapInfoText(mapInfos, count, savePathText);
            saveMapInfoPreviews(mapInfos, count, tileTypeData, saveFolderPathPreview);
        }
    }
}
