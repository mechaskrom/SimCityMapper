using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimCityMapper
{
    //Data about tile types (the small tiles that form a city).

    //Some tiles have an extra top tile up left of it in BG1 layer.
    //The tile and the extra top tile is in the same location. BG1 is just scrolled
    //with an 8,8 offset compared to BG2. Pretty clever!
    //Tile index 768 (all transparent) is used if there is no extra top tile.

    class TileType
    {
        public static void get(int tileType, out int tileInd, out int tileTopInd, out int palRow)
        {
            int dummy;
            get(tileType, out tileInd, out tileTopInd, out palRow, out dummy);
        }

        public static int getMinimapColor(int tileType)
        {
            int dummy;
            int miniCol;
            get(tileType, out dummy, out dummy, out dummy, out miniCol);
            return miniCol;
        }

        public static void get(int tileType, out int tileInd, out int tileTopInd, out int palRow, out int miniCol)
        {
            //Returns data about tile type.
            //Tile index = char tile index in VRAM.
            //Tile top index = char tile index in VRAM for any top tile (768 if none).
            //Palette row = palette row (0-7).
            //Minimap color = color index (0-15) in palette used to color tile in a minimap.
            switch (tileType & 0x03FF) //Only tile type 0-1023 valid. 958-1023 is unused (garbage) though.
            {
                case 0: tileInd = 677; tileTopInd = 768; palRow = 1; miniCol = 11; break; //EmptyLand
                case 1: tileInd = 880; tileTopInd = 768; palRow = 1; miniCol = 13; break; //Water_NoCoast_A
                case 2: tileInd = 880; tileTopInd = 768; palRow = 1; miniCol = 13; break; //Water_NoCoast_B
                case 3: tileInd = 880; tileTopInd = 768; palRow = 1; miniCol = 13; break; //Water_NoCoast_C
                case 4: tileInd = 864; tileTopInd = 768; palRow = 1; miniCol = 11; break; //Water_CoastNW_A
                case 5: tileInd = 865; tileTopInd = 768; palRow = 1; miniCol = 11; break; //Water_CoastN_A
                case 6: tileInd = 866; tileTopInd = 768; palRow = 1; miniCol = 11; break; //Water_CoastNE_A
                case 7: tileInd = 867; tileTopInd = 768; palRow = 1; miniCol = 11; break; //Water_CoastW_A
                case 8: tileInd = 868; tileTopInd = 768; palRow = 1; miniCol = 11; break; //Water_CoastE_A
                case 9: tileInd = 869; tileTopInd = 768; palRow = 1; miniCol = 11; break; //Water_CoastSW_A
                case 10: tileInd = 870; tileTopInd = 768; palRow = 1; miniCol = 11; break; //Water_CoastS_A
                case 11: tileInd = 871; tileTopInd = 768; palRow = 1; miniCol = 11; break; //Water_CoastSE_A
                case 12: tileInd = 872; tileTopInd = 768; palRow = 1; miniCol = 11; break; //Water_CoastNW_B
                case 13: tileInd = 873; tileTopInd = 768; palRow = 1; miniCol = 11; break; //Water_CoastN_B
                case 14: tileInd = 874; tileTopInd = 768; palRow = 1; miniCol = 11; break; //Water_CoastNE_B
                case 15: tileInd = 875; tileTopInd = 768; palRow = 1; miniCol = 11; break; //Water_CoastW_B
                case 16: tileInd = 876; tileTopInd = 768; palRow = 1; miniCol = 11; break; //Water_CoastE_B
                case 17: tileInd = 877; tileTopInd = 768; palRow = 1; miniCol = 11; break; //Water_CoastSW_B
                case 18: tileInd = 878; tileTopInd = 768; palRow = 1; miniCol = 11; break; //Water_CoastS_B
                case 19: tileInd = 879; tileTopInd = 768; palRow = 1; miniCol = 11; break; //Water_CoastSE_B
                case 20: tileInd = 654; tileTopInd = 768; palRow = 7; miniCol = 8; break; //Forest_EdgeNW_A
                case 21: tileInd = 655; tileTopInd = 768; palRow = 7; miniCol = 11; break; //Forest_EdgeN_A
                case 22: tileInd = 656; tileTopInd = 768; palRow = 7; miniCol = 10; break; //Forest_EdgeNE_A
                case 23: tileInd = 657; tileTopInd = 768; palRow = 7; miniCol = 11; break; //Forest_EdgeW_A
                case 24: tileInd = 658; tileTopInd = 768; palRow = 7; miniCol = 13; break; //Forest_NoEdge_A
                case 25: tileInd = 659; tileTopInd = 768; palRow = 7; miniCol = 11; break; //Forest_EdgeE_A
                case 26: tileInd = 660; tileTopInd = 768; palRow = 7; miniCol = 8; break; //Forest_EdgeSW_A
                case 27: tileInd = 661; tileTopInd = 768; palRow = 7; miniCol = 11; break; //Forest_EdgeS_A
                case 28: tileInd = 662; tileTopInd = 768; palRow = 7; miniCol = 10; break; //Forest_EdgeSE_A
                case 29: tileInd = 663; tileTopInd = 768; palRow = 7; miniCol = 11; break; //Forest_EdgeNW_B
                case 30: tileInd = 664; tileTopInd = 768; palRow = 7; miniCol = 10; break; //Forest_EdgeN_B
                case 31: tileInd = 665; tileTopInd = 768; palRow = 7; miniCol = 13; break; //Forest_EdgeNE_B
                case 32: tileInd = 666; tileTopInd = 768; palRow = 7; miniCol = 8; break; //Forest_EdgeW_B
                case 33: tileInd = 667; tileTopInd = 768; palRow = 7; miniCol = 10; break; //Forest_NoEdge_B
                case 34: tileInd = 668; tileTopInd = 768; palRow = 7; miniCol = 10; break; //Forest_EdgeE_B
                case 35: tileInd = 669; tileTopInd = 768; palRow = 7; miniCol = 11; break; //Forest_EdgeSW_B
                case 36: tileInd = 670; tileTopInd = 768; palRow = 7; miniCol = 10; break; //Forest_EdgeS_B
                case 37: tileInd = 671; tileTopInd = 768; palRow = 7; miniCol = 13; break; //Forest_EdgeSE_B
                case 38: tileInd = 672; tileTopInd = 768; palRow = 7; miniCol = 14; break; //SmallPark_NoTree
                case 39: tileInd = 673; tileTopInd = 768; palRow = 1; miniCol = 14; break; //SmallPark_Tree
                case 40: tileInd = 674; tileTopInd = 768; palRow = 1; miniCol = 11; break; //Rubble_A
                case 41: tileInd = 675; tileTopInd = 768; palRow = 1; miniCol = 11; break; //Rubble_B
                case 42: tileInd = 678; tileTopInd = 768; palRow = 1; miniCol = 11; break; //Rubble_C
                case 43: tileInd = 679; tileTopInd = 768; palRow = 1; miniCol = 11; break; //Rubble_D
                case 44: tileInd = 680; tileTopInd = 768; palRow = 1; miniCol = 11; break; //Rubble_E
                case 45: tileInd = 681; tileTopInd = 768; palRow = 1; miniCol = 11; break; //Rubble_F
                case 46: tileInd = 15; tileTopInd = 768; palRow = 0; miniCol = 11; break; //(Unused)
                case 47: tileInd = 15; tileTopInd = 768; palRow = 0; miniCol = 11; break; //(Unused)
                case 48: tileInd = 698; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Road_LineEW_Water
                case 49: tileInd = 699; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Road_LineNS_Water
                case 50: tileInd = 700; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Road_LineEW
                case 51: tileInd = 701; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Road_LineNS
                case 52: tileInd = 702; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Road_CornerSW
                case 53: tileInd = 703; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Road_CornerNW
                case 54: tileInd = 704; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Road_CornerNE
                case 55: tileInd = 705; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Road_CornerSE
                case 56: tileInd = 706; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Road_TeeS
                case 57: tileInd = 707; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Road_TeeW
                case 58: tileInd = 708; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Road_IeeN
                case 59: tileInd = 709; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Road_TeeE
                case 60: tileInd = 710; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Road_4WayCross
                case 61: tileInd = 711; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Road_LineEW_PowerLine
                case 62: tileInd = 712; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Road_LineNS_PowerLine
                case 63: tileInd = 15; tileTopInd = 768; palRow = 0; miniCol = 8; break; //(Unused)
                case 64: tileInd = 887; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Road_LineEW_Water_LowTraffic
                case 65: tileInd = 888; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Road_LineNS_Water_LowTraffic
                case 66: tileInd = 889; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Road_LineEW_LowTraffic
                case 67: tileInd = 890; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Road_LineNS_LowTraffic
                case 68: tileInd = 891; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Road_CornerSW_LowTraFfic
                case 69: tileInd = 892; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Road_CornerNW_LowTraffic
                case 70: tileInd = 893; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Road_CornerNE_LowTraffic
                case 71: tileInd = 894; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Road_CornerSE_LowTraffic
                case 72: tileInd = 895; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Road_TeeS_LowTraffic
                case 73: tileInd = 896; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Road_TeeW_LowTraffic
                case 74: tileInd = 897; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Road_TeeN_LowTraffic
                case 75: tileInd = 898; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Road_TeeE_LowTraffic
                case 76: tileInd = 899; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Road_4WayCross_LowTraffic
                case 77: tileInd = 900; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Road_LineEW_PowerLine_LowTraffic
                case 78: tileInd = 901; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Road_LineNS_PowerLine_LowTraffic
                case 79: tileInd = 15; tileTopInd = 768; palRow = 0; miniCol = 8; break; //(Unused)
                case 80: tileInd = 902; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Road_LineEW_Water_HighTraffic
                case 81: tileInd = 903; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Road_LineNS_Water_HighTraffic
                case 82: tileInd = 904; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Road_LineEW_HighTraffic
                case 83: tileInd = 905; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Road_LineNS_HighTraffic
                case 84: tileInd = 906; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Road_CornerSW_HighTraffic
                case 85: tileInd = 907; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Road_CornerNW_HighTraffic
                case 86: tileInd = 908; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Road_CornerNE_HighTraffic
                case 87: tileInd = 909; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Road_CornerSE_HighTraffic
                case 88: tileInd = 910; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Road_TeeS_HighTraffic
                case 89: tileInd = 911; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Road_TeeW_HighTraffic
                case 90: tileInd = 912; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Road_TeeN_HighTraffic
                case 91: tileInd = 913; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Road_TeeE_HighTraffic
                case 92: tileInd = 914; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Road_4WayCross_HighTraffic
                case 93: tileInd = 915; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Road_LineEW_PowerLine_HighTraffic
                case 94: tileInd = 916; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Road_LineNS_PowerLine_HighTraffic
                case 95: tileInd = 15; tileTopInd = 768; palRow = 0; miniCol = 8; break; //(Unused)
                case 96: tileInd = 883; tileTopInd = 768; palRow = 1; miniCol = 11; break; //PowerLine_LineEW_Water
                case 97: tileInd = 884; tileTopInd = 768; palRow = 1; miniCol = 11; break; //PowerLine_LineNS_Water
                case 98: tileInd = 713; tileTopInd = 768; palRow = 1; miniCol = 11; break; //PowerLine_LineEW
                case 99: tileInd = 714; tileTopInd = 768; palRow = 1; miniCol = 11; break; //PowerLine_LineNS
                case 100: tileInd = 715; tileTopInd = 768; palRow = 1; miniCol = 11; break; //PowerLine_CornerSW
                case 101: tileInd = 716; tileTopInd = 768; palRow = 1; miniCol = 11; break; //PowerLine_CornerNW
                case 102: tileInd = 717; tileTopInd = 768; palRow = 1; miniCol = 11; break; //PowerLine_CornerNE
                case 103: tileInd = 718; tileTopInd = 768; palRow = 1; miniCol = 11; break; //PowerLine_CornerSE
                case 104: tileInd = 719; tileTopInd = 768; palRow = 1; miniCol = 11; break; //PowerLine_TeeS
                case 105: tileInd = 720; tileTopInd = 768; palRow = 1; miniCol = 11; break; //PowerLine_TeeW
                case 106: tileInd = 721; tileTopInd = 768; palRow = 1; miniCol = 11; break; //PowerLine_TeeN
                case 107: tileInd = 722; tileTopInd = 768; palRow = 1; miniCol = 11; break; //PowerLine_TeeE
                case 108: tileInd = 723; tileTopInd = 768; palRow = 1; miniCol = 11; break; //PowerLine_4WayCross
                case 109: tileInd = 724; tileTopInd = 768; palRow = 1; miniCol = 11; break; //PowerLine_LineNS_Rail
                case 110: tileInd = 725; tileTopInd = 768; palRow = 1; miniCol = 11; break; //PowerLine_LineEW_Rail
                case 111: tileInd = 15; tileTopInd = 768; palRow = 0; miniCol = 11; break; //(Unused)
                case 112: tileInd = 881; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Rail_LineEW_Water
                case 113: tileInd = 882; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Rail_LineNS_Water
                case 114: tileInd = 726; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Rail_LineEW
                case 115: tileInd = 727; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Rail_LineNS
                case 116: tileInd = 728; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Rail_CornerSW
                case 117: tileInd = 729; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Rail_CornerNW
                case 118: tileInd = 730; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Rail_CornerNE
                case 119: tileInd = 731; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Rail_CornerSE
                case 120: tileInd = 732; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Rail_TeeS
                case 121: tileInd = 733; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Rail_TeeW
                case 122: tileInd = 734; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Rail_TeeN
                case 123: tileInd = 735; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Rail_TeeE
                case 124: tileInd = 736; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Rail_4WayCross
                case 125: tileInd = 737; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Rail_LineEW_Road
                case 126: tileInd = 738; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Rail_LineNS_Road
                case 127: tileInd = 885; tileTopInd = 768; palRow = 1; miniCol = 11; break; //Fire
                case 128: tileInd = 0; tileTopInd = 768; palRow = 1; miniCol = 12; break; //Residential_Undeveloped_TopLeft
                case 129: tileInd = 1; tileTopInd = 768; palRow = 1; miniCol = 3; break; //Residential_Undeveloped_TopCenter
                case 130: tileInd = 2; tileTopInd = 768; palRow = 1; miniCol = 3; break; //Residential_Undeveloped_TopRight
                case 131: tileInd = 3; tileTopInd = 768; palRow = 1; miniCol = 12; break; //Residential_Undeveloped_CenterLeft
                case 132: tileInd = 4; tileTopInd = 768; palRow = 1; miniCol = 3; break; //Residential_Undeveloped_Center
                case 133: tileInd = 5; tileTopInd = 768; palRow = 1; miniCol = 3; break; //Residential_Undeveloped_CenterRight
                case 134: tileInd = 6; tileTopInd = 768; palRow = 1; miniCol = 12; break; //Residential_Undeveloped_BottomLeft
                case 135: tileInd = 7; tileTopInd = 768; palRow = 1; miniCol = 12; break; //Residential_Undeveloped_BottomCenter
                case 136: tileInd = 8; tileTopInd = 768; palRow = 1; miniCol = 12; break; //Residential_Undeveloped_BottomRight
                case 137: tileInd = 9; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage0LowHouse_A
                case 138: tileInd = 10; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage0LowHouse_B
                case 139: tileInd = 11; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage0LowHouse_C
                case 140: tileInd = 12; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage0MidHouse_A
                case 141: tileInd = 13; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage0MidHouse_B
                case 142: tileInd = 14; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage0MidHouse_C
                case 143: tileInd = 15; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage0UpperHouse_A
                case 144: tileInd = 16; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage0UpperHouse_B
                case 145: tileInd = 17; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage0UpperHouse_C
                case 146: tileInd = 18; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage0HighHouse_A
                case 147: tileInd = 19; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage0HighHouse_B
                case 148: tileInd = 20; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage0HighHouse_C
                case 149: tileInd = 21; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage1Low_TopLeft
                case 150: tileInd = 22; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage1Low_TopCenter
                case 151: tileInd = 23; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage1Low_TopRight
                case 152: tileInd = 25; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage1Low_CenterLeft
                case 153: tileInd = 26; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage1Low_Center
                case 154: tileInd = 27; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage1Low_CenterRight
                case 155: tileInd = 27; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage1Low_BottomLeft
                case 156: tileInd = 28; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage1Low_BottomCenter
                case 157: tileInd = 29; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage1Low_BottomRight
                case 158: tileInd = 30; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage2Low_TopLeft
                case 159: tileInd = 31; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage2Low_TopCenter
                case 160: tileInd = 32; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage2Low_TopRight
                case 161: tileInd = 33; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage2Low_CenterLeft
                case 162: tileInd = 34; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage2Low_Center
                case 163: tileInd = 35; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage2Low_CenterRight
                case 164: tileInd = 36; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage2Low_BottomLeft
                case 165: tileInd = 37; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage2Low_BottomCenter
                case 166: tileInd = 38; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage2Low_BottomRight
                case 167: tileInd = 39; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage3Low_TopLeft
                case 168: tileInd = 40; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage3Low_TopCenter
                case 169: tileInd = 41; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage3Low_TopRight
                case 170: tileInd = 42; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage3Low_CenterLeft
                case 171: tileInd = 43; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage3Low_Center
                case 172: tileInd = 44; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage3Low_CenterRight
                case 173: tileInd = 45; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage3Low_BottomLeft
                case 174: tileInd = 46; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage3Low_BottomCenter
                case 175: tileInd = 47; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage3Low_BottomRight
                case 176: tileInd = 48; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage4Low_TopLeft
                case 177: tileInd = 49; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage4Low_TopCenter
                case 178: tileInd = 50; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage4Low_TopRight
                case 179: tileInd = 51; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage4Low_CenterLeft
                case 180: tileInd = 52; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage4Low_Center
                case 181: tileInd = 53; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage4Low_CenterRight
                case 182: tileInd = 54; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage4Low_BottomLeft
                case 183: tileInd = 55; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage4Low_BottomCenter
                case 184: tileInd = 56; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage4Low_BottomRight
                case 185: tileInd = 57; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage1Mid_TopLeft
                case 186: tileInd = 58; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage1Mid_TopCenter
                case 187: tileInd = 59; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage1Mid_TopRight
                case 188: tileInd = 60; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage1Mid_CenterLeft
                case 189: tileInd = 61; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage1Mid_Center
                case 190: tileInd = 62; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage1Mid_CenterRight
                case 191: tileInd = 63; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage1Mid_BottomLeft
                case 192: tileInd = 64; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage1Mid_BottomCenter
                case 193: tileInd = 65; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage1Mid_BottomRight
                case 194: tileInd = 66; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage2Mid_TopLeft
                case 195: tileInd = 67; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage2Mid_TopCenter
                case 196: tileInd = 68; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage2Mid_TopRight
                case 197: tileInd = 69; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage2Mid_CenterLeft
                case 198: tileInd = 70; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage2Mid_Center
                case 199: tileInd = 71; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage2Mid_CenterRight
                case 200: tileInd = 72; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage2Mid_BottomLeft
                case 201: tileInd = 73; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage2Mid_BottomCenter
                case 202: tileInd = 74; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage2Mid_BottomRight
                case 203: tileInd = 75; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage3Mid_TopLeft
                case 204: tileInd = 76; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage3Mid_TopCenter
                case 205: tileInd = 77; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage3Mid_TopRight
                case 206: tileInd = 78; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage3Mid_CenterLeft
                case 207: tileInd = 79; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage3Mid_Center
                case 208: tileInd = 80; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage3Mid_CenterRight
                case 209: tileInd = 81; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage3Mid_BottomLeft
                case 210: tileInd = 82; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage3Mid_BottomCenter
                case 211: tileInd = 83; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage3Mid_BottomRight
                case 212: tileInd = 84; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage4Mid_TopLeft
                case 213: tileInd = 85; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage4Mid_TopCenter
                case 214: tileInd = 86; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage4Mid_TopRight
                case 215: tileInd = 87; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage4Mid_CenterLeft
                case 216: tileInd = 88; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage4Mid_Center
                case 217: tileInd = 89; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage4Mid_CenterRight
                case 218: tileInd = 90; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage4Mid_BottomLeft
                case 219: tileInd = 91; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage4Mid_BottomCenter
                case 220: tileInd = 92; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage4Mid_BottomRight
                case 221: tileInd = 93; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage1Upper_TopLeft
                case 222: tileInd = 94; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage1Upper_TopCenter
                case 223: tileInd = 95; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage1Upper_TopRight
                case 224: tileInd = 96; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage1Upper_CenterLeft
                case 225: tileInd = 97; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage1Upper_Center
                case 226: tileInd = 98; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage1Upper_CenterRight
                case 227: tileInd = 99; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage1Upper_BottomLeft
                case 228: tileInd = 100; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage1Upper_BottomCenter
                case 229: tileInd = 101; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage1Upper_BottomRight
                case 230: tileInd = 102; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage2Upper_TopLeft
                case 231: tileInd = 103; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage2Upper_TopCenter
                case 232: tileInd = 104; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage2Upper_TopRight
                case 233: tileInd = 105; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage2Upper_CenterLeft
                case 234: tileInd = 106; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage2Upper_Center
                case 235: tileInd = 107; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage2Upper_CenterRight
                case 236: tileInd = 108; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage2Upper_BottomLeft
                case 237: tileInd = 109; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage2Upper_BottomCenter
                case 238: tileInd = 110; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage2Upper_BottomRight
                case 239: tileInd = 111; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage3Upper_TopLeft
                case 240: tileInd = 112; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage3Upper_TopCenter
                case 241: tileInd = 113; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage3Upper_TopRight
                case 242: tileInd = 114; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage3Upper_CenterLeft
                case 243: tileInd = 115; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage3Upper_Center
                case 244: tileInd = 116; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage3Upper_CenterRight
                case 245: tileInd = 117; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage3Upper_BottomLeft
                case 246: tileInd = 118; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage3Upper_BottomCenter
                case 247: tileInd = 119; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage3Upper_BottomRight
                case 248: tileInd = 121; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage4Upper_TopLeft
                case 249: tileInd = 122; tileTopInd = 120; palRow = 4; miniCol = 3; break; //Residential_Stage4Upper_TopCenter
                case 250: tileInd = 123; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage4Upper_TopRight
                case 251: tileInd = 124; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage4Upper_CenterLeft
                case 252: tileInd = 125; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage4Upper_Center
                case 253: tileInd = 126; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage4Upper_CenterRight
                case 254: tileInd = 127; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage4Upper_BottomLeft
                case 255: tileInd = 128; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage4Upper_BottomCenter
                case 256: tileInd = 129; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage4Upper_BottomRight
                case 257: tileInd = 130; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage1High_TopLeft
                case 258: tileInd = 131; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage1High_TopCenter
                case 259: tileInd = 132; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage1High_TopRight
                case 260: tileInd = 133; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage1High_CenterLeft
                case 261: tileInd = 134; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage1High_Center
                case 262: tileInd = 135; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage1High_CenterRight
                case 263: tileInd = 136; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage1High_BottomLeft
                case 264: tileInd = 137; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage1High_BottomCenter
                case 265: tileInd = 138; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage1High_BottomRight
                case 266: tileInd = 139; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage2High_TopLeft
                case 267: tileInd = 140; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage2High_TopCenter
                case 268: tileInd = 141; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage2High_TopRight
                case 269: tileInd = 142; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage2High_CenterLeft
                case 270: tileInd = 143; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage2High_Center
                case 271: tileInd = 144; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage2High_CenterRight
                case 272: tileInd = 145; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage2High_BottomLeft
                case 273: tileInd = 146; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage2High_BottomCenter
                case 274: tileInd = 147; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage2High_BottomRight
                case 275: tileInd = 150; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage3High_TopLeft
                case 276: tileInd = 151; tileTopInd = 148; palRow = 4; miniCol = 3; break; //Residential_Stage3High_TopCenter
                case 277: tileInd = 152; tileTopInd = 149; palRow = 4; miniCol = 3; break; //Residential_Stage3High_TopRight
                case 278: tileInd = 153; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage3High_CenterLeft
                case 279: tileInd = 154; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage3High_Center
                case 280: tileInd = 155; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage3High_CenterRight
                case 281: tileInd = 156; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage3High_BottomLeft
                case 282: tileInd = 157; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage3High_BottomCenter
                case 283: tileInd = 158; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage3High_BottomRight
                case 284: tileInd = 161; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage4High_TopLeft
                case 285: tileInd = 162; tileTopInd = 159; palRow = 4; miniCol = 3; break; //Residential_Stage4High_TopCenter
                case 286: tileInd = 163; tileTopInd = 160; palRow = 4; miniCol = 3; break; //Residential_Stage4High_TopRight
                case 287: tileInd = 164; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage4High_CenterLeft
                case 288: tileInd = 165; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage4High_Center
                case 289: tileInd = 166; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Stage4High_CenterRight
                case 290: tileInd = 167; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage4High_BottomLeft
                case 291: tileInd = 168; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage4High_BottomCenter
                case 292: tileInd = 169; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Stage4High_BottomRight
                case 293: tileInd = 739; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Hospital_TopLeft
                case 294: tileInd = 740; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Hospital_TopCenter
                case 295: tileInd = 741; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Hospital_TopRight
                case 296: tileInd = 742; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Hospital_CenterLeft
                case 297: tileInd = 743; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Hospital_Center
                case 298: tileInd = 744; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_Hospital_CenterRight
                case 299: tileInd = 745; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Hospital_BottomLeft
                case 300: tileInd = 746; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Hospital_BottomCenter
                case 301: tileInd = 747; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_Hospital_BottomRight
                case 302: tileInd = 748; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_School_TopLeft
                case 303: tileInd = 749; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_School_TopCenter
                case 304: tileInd = 750; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_School_TopRight
                case 305: tileInd = 751; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_School_CenterLeft
                case 306: tileInd = 752; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_School_Center
                case 307: tileInd = 753; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_School_CenterRight
                case 308: tileInd = 754; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_School_BottomLeft
                case 309: tileInd = 755; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_School_BottomCenter
                case 310: tileInd = 756; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_School_BottomRight
                case 311: tileInd = 170; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Undeveloped_TopLeft
                case 312: tileInd = 171; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Undeveloped_TopCenter
                case 313: tileInd = 172; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Undeveloped_TopRight
                case 314: tileInd = 173; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Undeveloped_CenterLeft
                case 315: tileInd = 174; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Undeveloped_Center
                case 316: tileInd = 175; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Undeveloped_CenterRight
                case 317: tileInd = 176; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Undeveloped_BottomLeft
                case 318: tileInd = 177; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Undeveloped_BottomCenter
                case 319: tileInd = 178; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Undeveloped_BottomRight
                case 320: tileInd = 179; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage1Low_TopLeft
                case 321: tileInd = 180; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage1Low_TopCenter
                case 322: tileInd = 181; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage1Low_TopRight
                case 323: tileInd = 182; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage1Low_CenterLeft
                case 324: tileInd = 183; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage1Low_Center
                case 325: tileInd = 184; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage1Low_CenterRight
                case 326: tileInd = 185; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage1Low_BottomLeft
                case 327: tileInd = 186; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage1Low_BottomCenter
                case 328: tileInd = 187; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage1Low_BottomRight
                case 329: tileInd = 188; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage2Low_TopLeft
                case 330: tileInd = 189; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage2Low_TopCenter
                case 331: tileInd = 190; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage2Low_TopRight
                case 332: tileInd = 191; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage2Low_CenterLeft
                case 333: tileInd = 192; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage2Low_Center
                case 334: tileInd = 193; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage2Low_CenterRight
                case 335: tileInd = 194; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage2Low_BottomLeft
                case 336: tileInd = 195; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage2Low_BottomCenter
                case 337: tileInd = 196; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage2Low_BottomRight
                case 338: tileInd = 197; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage3Low_TopLeft
                case 339: tileInd = 198; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage3Low_TopCenter
                case 340: tileInd = 199; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage3Low_TopRight
                case 341: tileInd = 200; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage3Low_CenterLeft
                case 342: tileInd = 201; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage3Low_Center
                case 343: tileInd = 202; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage3Low_CenterRight
                case 344: tileInd = 203; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage3Low_BottomLeft
                case 345: tileInd = 204; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage3Low_BottomCenter
                case 346: tileInd = 205; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage3Low_BottomRight
                case 347: tileInd = 206; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage4Low_TopLeft
                case 348: tileInd = 207; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage4Low_TopCenter
                case 349: tileInd = 208; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage4Low_TopRight
                case 350: tileInd = 209; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage4Low_CenterLeft
                case 351: tileInd = 210; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage4Low_Center
                case 352: tileInd = 211; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage4Low_CenterRight
                case 353: tileInd = 212; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage4Low_BottomLeft
                case 354: tileInd = 213; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage4Low_BottomCenter
                case 355: tileInd = 214; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage4Low_BottomRight
                case 356: tileInd = 216; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage5Low_TopLeft
                case 357: tileInd = 217; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage5Low_TopCenter
                case 358: tileInd = 218; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage5Low_TopRight
                case 359: tileInd = 219; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage5Low_CenterLeft
                case 360: tileInd = 220; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage5Low_Center
                case 361: tileInd = 221; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage5Low_CenterRight
                case 362: tileInd = 222; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage5Low_BottomLeft
                case 363: tileInd = 223; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage5Low_BottomCenter
                case 364: tileInd = 224; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage5Low_BottomRight
                case 365: tileInd = 225; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage1Mid_TopLeft
                case 366: tileInd = 226; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage1Mid_TopCenter
                case 367: tileInd = 227; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage1Mid_TopRight
                case 368: tileInd = 228; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage1Mid_CenterLeft
                case 369: tileInd = 229; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage1Mid_Center
                case 370: tileInd = 230; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage1Mid_CenterRight
                case 371: tileInd = 231; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage1Mid_BottomLeft
                case 372: tileInd = 232; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage1Mid_BottomCenter
                case 373: tileInd = 233; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage1Mid_BottomRight
                case 374: tileInd = 234; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage2Mid_TopLeft
                case 375: tileInd = 235; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage2Mid_TopCenter
                case 376: tileInd = 236; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage2Mid_TopRight
                case 377: tileInd = 237; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage2Mid_CenterLeft
                case 378: tileInd = 238; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage2Mid_Center
                case 379: tileInd = 239; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage2Mid_CenterRight
                case 380: tileInd = 240; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage2Mid_BottomLeft
                case 381: tileInd = 241; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage2Mid_BottomCenter
                case 382: tileInd = 242; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage2Mid_BottomRight
                case 383: tileInd = 243; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage3Mid_TopLeft
                case 384: tileInd = 244; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage3Mid_TopCenter
                case 385: tileInd = 245; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage3Mid_TopRight
                case 386: tileInd = 246; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage3Mid_CenterLeft
                case 387: tileInd = 247; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commerclal_Stage3Mid_Center
                case 388: tileInd = 248; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage3Mid_CenterRight
                case 389: tileInd = 249; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage3Mid_BottomLeft
                case 390: tileInd = 250; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage3Mid_BottomCenter
                case 391: tileInd = 251; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage3Mid_BottomRight
                case 392: tileInd = 254; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage4Mid_TopLeft
                case 393: tileInd = 255; tileTopInd = 252; palRow = 2; miniCol = 4; break; //Commercial_Stage4Mid_TopCenter
                case 394: tileInd = 256; tileTopInd = 253; palRow = 2; miniCol = 4; break; //Commercial_Stage4Mid_TopRight
                case 395: tileInd = 257; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage4Mid_CenterLeft
                case 396: tileInd = 258; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage4Mid_Center
                case 397: tileInd = 259; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage4Mid_CenterRight
                case 398: tileInd = 260; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage4Mid_BottomLeft
                case 399: tileInd = 261; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage4Mid_BottomCenter
                case 400: tileInd = 262; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage4Mid_BottomRight
                case 401: tileInd = 265; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage5Mid_TopLeft
                case 402: tileInd = 266; tileTopInd = 263; palRow = 2; miniCol = 4; break; //Commercial_Stage5Mid_TopCenter
                case 403: tileInd = 267; tileTopInd = 264; palRow = 2; miniCol = 4; break; //Commercial_Stage5Mid_TopRight
                case 404: tileInd = 268; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage5Mid_CenterLeft
                case 405: tileInd = 269; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage5Mid_Center
                case 406: tileInd = 270; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage5Mid_CenterRight
                case 407: tileInd = 271; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commerclal_Stage5Mid_BottomLeft
                case 408: tileInd = 272; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage5Mid_BottomCenter
                case 409: tileInd = 273; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage5Mid_BottomRight
                case 410: tileInd = 274; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage1Upper_TopLeft
                case 411: tileInd = 275; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage1Upper_TopCenter
                case 412: tileInd = 276; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage1Upper_TopRight
                case 413: tileInd = 277; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage1Upper_CenterLeft
                case 414: tileInd = 278; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage1Upper_Center
                case 415: tileInd = 279; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commerclal_Stage1Upper_CenterRight
                case 416: tileInd = 280; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage1Upper_BottomLeft
                case 417: tileInd = 281; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage1Upper_BottomCenter
                case 418: tileInd = 282; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage1Upper_BottomRight
                case 419: tileInd = 283; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage2Upper_TopLeft
                case 420: tileInd = 284; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage2Upper_TopCenter
                case 421: tileInd = 285; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage2Upper_TopRight
                case 422: tileInd = 286; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage2Upper_CenterLeft
                case 423: tileInd = 287; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage2Upper_Center
                case 424: tileInd = 288; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage2Upper_CenterRight
                case 425: tileInd = 289; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage2Upper_BottomLeft
                case 426: tileInd = 290; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage2Upper_BottomCenter
                case 427: tileInd = 291; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage2Upper_BottomRight
                case 428: tileInd = 292; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage3Upper_TopLeft
                case 429: tileInd = 293; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage3Upper_TopCenter
                case 430: tileInd = 294; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage3Upper_TopRight
                case 431: tileInd = 295; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage3Upper_CenterLeft
                case 432: tileInd = 296; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage3Upper_Center
                case 433: tileInd = 297; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage3Upper_CenterRight
                case 434: tileInd = 298; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage3Upper_BottomLeft
                case 435: tileInd = 299; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage3Upper_BottomCenter
                case 436: tileInd = 300; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage3Upper_BottomRight
                case 437: tileInd = 303; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage4Upper_TopLeft
                case 438: tileInd = 304; tileTopInd = 301; palRow = 2; miniCol = 4; break; //Commercial_Stage4Upper_TopCenter
                case 439: tileInd = 305; tileTopInd = 302; palRow = 2; miniCol = 4; break; //Commercial_Stage4Upper_TopRight
                case 440: tileInd = 306; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage4Upper_CenterLeft
                case 441: tileInd = 307; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage4Upper_Center
                case 442: tileInd = 308; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage4Upper_CenterRight
                case 443: tileInd = 309; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage4Upper_BottomLeft
                case 444: tileInd = 310; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage4Upper_BottomCenter
                case 445: tileInd = 311; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage4Upper_BottomRight
                case 446: tileInd = 314; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage5Upper_TopLeft
                case 447: tileInd = 315; tileTopInd = 312; palRow = 2; miniCol = 4; break; //Commercial_Stage5Upper_TopCenter
                case 448: tileInd = 316; tileTopInd = 313; palRow = 2; miniCol = 4; break; //Commercial_Stage5Upper_TopRight
                case 449: tileInd = 317; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage5Upper_CenterLeft
                case 450: tileInd = 318; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage5Upper_Center
                case 451: tileInd = 319; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage5Upper_CenterRight
                case 452: tileInd = 320; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage5Upper_BottomLeft
                case 453: tileInd = 321; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_StageSUpper_BottomCenter
                case 454: tileInd = 322; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage5Upper_BottomRight
                case 455: tileInd = 323; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage1High_TopLeft
                case 456: tileInd = 324; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage1High_TopCenter
                case 457: tileInd = 325; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage1High_TopRight
                case 458: tileInd = 326; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage1High_CenterLeft
                case 459: tileInd = 327; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage1High_Center
                case 460: tileInd = 328; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage1High_CenterRight
                case 461: tileInd = 329; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage1High_BottomLeft
                case 462: tileInd = 330; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage1High_BottomCenter
                case 463: tileInd = 331; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage1High_BottomRight
                case 464: tileInd = 332; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage2High_TopLeft
                case 465: tileInd = 333; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage2High_TopCenter
                case 466: tileInd = 334; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage2High_TopRight
                case 467: tileInd = 335; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage2High_CenterLeft
                case 468: tileInd = 336; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage2High_Center
                case 469: tileInd = 337; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage2High_CenterRight
                case 470: tileInd = 338; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage2High_BottomLeft
                case 471: tileInd = 339; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage2High_BottomCenter
                case 472: tileInd = 340; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage2High_BottomRight
                case 473: tileInd = 343; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage3High_TopLeft
                case 474: tileInd = 344; tileTopInd = 341; palRow = 2; miniCol = 4; break; //Commercial_Stage3High_TopCenter
                case 475: tileInd = 345; tileTopInd = 342; palRow = 2; miniCol = 4; break; //Commercial_Stage3High_TopRight
                case 476: tileInd = 346; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage3High_CenterLeft
                case 477: tileInd = 347; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage3High_Center
                case 478: tileInd = 348; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage3High_CenterRight
                case 479: tileInd = 349; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage3High_BottomLeft
                case 480: tileInd = 350; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage3High_BottomCenter
                case 481: tileInd = 351; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage3High_BottomRight
                case 482: tileInd = 354; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage4High_TopLeft
                case 483: tileInd = 355; tileTopInd = 352; palRow = 2; miniCol = 4; break; //Commercial_Stage4High_TopCenter
                case 484: tileInd = 356; tileTopInd = 353; palRow = 2; miniCol = 4; break; //Commercial_Stage4High_TopRight
                case 485: tileInd = 357; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage4High_CenterLeft
                case 486: tileInd = 358; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage4High_Center
                case 487: tileInd = 359; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage4High_CenterRight
                case 488: tileInd = 360; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage4High_BottomLeft
                case 489: tileInd = 361; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage4High_BottomCenter
                case 490: tileInd = 362; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage4High_BottomRight
                case 491: tileInd = 365; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage5High_TopLeft
                case 492: tileInd = 366; tileTopInd = 363; palRow = 2; miniCol = 4; break; //Commercial_Stage5High_TopCenter
                case 493: tileInd = 367; tileTopInd = 364; palRow = 2; miniCol = 4; break; //Commercial_Stage5High_TopRight
                case 494: tileInd = 368; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage5High_CenterLeft
                case 495: tileInd = 369; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage5High_Center
                case 496: tileInd = 370; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_Stage5High_CenterRight
                case 497: tileInd = 371; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage5High_BottomLeft
                case 498: tileInd = 372; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage5High_BottomCenter
                case 499: tileInd = 373; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_Stage5High_BottomRight
                case 500: tileInd = 374; tileTopInd = 768; palRow = 1; miniCol = 12; break; //Industrial_Undeveloped_TopLeft
                case 501: tileInd = 375; tileTopInd = 768; palRow = 1; miniCol = 5; break; //Industrial_Undeveloped_TopCenter
                case 502: tileInd = 376; tileTopInd = 768; palRow = 1; miniCol = 5; break; //Industrial_Undeveloped_TopRight
                case 503: tileInd = 377; tileTopInd = 768; palRow = 1; miniCol = 12; break; //Industrial_Undeveloped_CenterLeft
                case 504: tileInd = 378; tileTopInd = 768; palRow = 1; miniCol = 5; break; //Industrial_Undeveloped_Center
                case 505: tileInd = 379; tileTopInd = 768; palRow = 1; miniCol = 5; break; //Industrial_Undeveloped_CenterRight
                case 506: tileInd = 380; tileTopInd = 768; palRow = 1; miniCol = 12; break; //Industrial_Undeveloped_BottomLeft
                case 507: tileInd = 381; tileTopInd = 768; palRow = 1; miniCol = 12; break; //Industrial_Undeveloped_BottomCenter
                case 508: tileInd = 382; tileTopInd = 768; palRow = 1; miniCol = 12; break; //Industrial_Undeveloped_BottomRight
                case 509: tileInd = 383; tileTopInd = 768; palRow = 3; miniCol = 12; break; //Industrial_Stage1Low_TopLeft
                case 510: tileInd = 384; tileTopInd = 768; palRow = 3; miniCol = 5; break; //Industrial_Stage1Low_TopCenter
                case 511: tileInd = 385; tileTopInd = 768; palRow = 3; miniCol = 5; break; //Industrial_Stage1Low_TopRight
                case 512: tileInd = 386; tileTopInd = 768; palRow = 3; miniCol = 12; break; //Industrial_Stage1Low_CenterLeft
                case 513: tileInd = 917; tileTopInd = 768; palRow = 3; miniCol = 5; break; //Industrial_Stage1Low_Center
                case 514: tileInd = 918; tileTopInd = 768; palRow = 3; miniCol = 5; break; //Industrial_Stage1Low_CenterRight
                case 515: tileInd = 387; tileTopInd = 768; palRow = 3; miniCol = 12; break; //Industrial_Stage1Low_BottomLeft
                case 516: tileInd = 919; tileTopInd = 768; palRow = 3; miniCol = 12; break; //Industrial_Stage1Low_BottomCenter
                case 517: tileInd = 388; tileTopInd = 768; palRow = 3; miniCol = 12; break; //Industrial_Stage1Low_BottomRight
                case 518: tileInd = 389; tileTopInd = 768; palRow = 3; miniCol = 12; break; //Industrial_Stage2Low_TopLeft
                case 519: tileInd = 390; tileTopInd = 768; palRow = 3; miniCol = 5; break; //Industrial_Stage2Low_TopCenter
                case 520: tileInd = 391; tileTopInd = 768; palRow = 3; miniCol = 5; break; //Industrial_Stage2Low_TopRight
                case 521: tileInd = 392; tileTopInd = 768; palRow = 3; miniCol = 12; break; //Industrial_Stage2Low_CenterLeft
                case 522: tileInd = 920; tileTopInd = 768; palRow = 3; miniCol = 5; break; //Industrial_Stage2Low_Center
                case 523: tileInd = 393; tileTopInd = 768; palRow = 3; miniCol = 5; break; //Industrial_Stage2Low_CenterRight
                case 524: tileInd = 394; tileTopInd = 768; palRow = 3; miniCol = 12; break; //Industrial_Stage2Low_BottomLeft
                case 525: tileInd = 921; tileTopInd = 768; palRow = 3; miniCol = 12; break; //Industrial_Stage2Low_BottomCenter
                case 526: tileInd = 395; tileTopInd = 768; palRow = 3; miniCol = 12; break; //Industrial_Stage2Low_BottomRight
                case 527: tileInd = 922; tileTopInd = 768; palRow = 3; miniCol = 12; break; //Industrial_Stage3Low_TopLeft
                case 528: tileInd = 923; tileTopInd = 768; palRow = 3; miniCol = 5; break; //Industrial_Stage3Low_TopCenter
                case 529: tileInd = 924; tileTopInd = 768; palRow = 3; miniCol = 5; break; //Industrial_Stage3Low_TopRight
                case 530: tileInd = 925; tileTopInd = 768; palRow = 3; miniCol = 12; break; //Industrial_Stage3Low_CenterLeft
                case 531: tileInd = 926; tileTopInd = 768; palRow = 3; miniCol = 5; break; //Industrial_Stage3Low_Center
                case 532: tileInd = 396; tileTopInd = 768; palRow = 3; miniCol = 5; break; //Industrial_Stage3Low_CenterRight
                case 533: tileInd = 397; tileTopInd = 768; palRow = 3; miniCol = 12; break; //Industrial_Stage3Low_BottomLeft
                case 534: tileInd = 398; tileTopInd = 768; palRow = 3; miniCol = 12; break; //Industrial_Stage3Low_BottomCenter
                case 535: tileInd = 399; tileTopInd = 768; palRow = 3; miniCol = 12; break; //Industrial_Stage3Low_BottomRight
                case 536: tileInd = 401; tileTopInd = 927; palRow = 3; miniCol = 12; break; //Industrial_Stage4Low_TopLeft
                case 537: tileInd = 402; tileTopInd = 928; palRow = 3; miniCol = 5; break; //Industrial_Stage4Low_TopCenter
                case 538: tileInd = 403; tileTopInd = 929; palRow = 3; miniCol = 5; break; //Industrial_Stage4Low_TopRight
                case 539: tileInd = 404; tileTopInd = 400; palRow = 3; miniCol = 12; break; //Industrial_Stage4Low_CenterLeft
                case 540: tileInd = 405; tileTopInd = 768; palRow = 3; miniCol = 5; break; //Industrial_Stage4Low_Center
                case 541: tileInd = 406; tileTopInd = 768; palRow = 3; miniCol = 5; break; //Industrial_Stage4Low_CenterRight
                case 542: tileInd = 407; tileTopInd = 768; palRow = 3; miniCol = 12; break; //Industrial_Stage4Low_BottomLeft
                case 543: tileInd = 408; tileTopInd = 768; palRow = 3; miniCol = 12; break; //Industrial_Stage4Low_BottomCenter
                case 544: tileInd = 409; tileTopInd = 768; palRow = 3; miniCol = 12; break; //Industrial_Stage4Low_BottomRight
                case 545: tileInd = 410; tileTopInd = 768; palRow = 3; miniCol = 12; break; //Industrial_Stage1High_TopLeft
                case 546: tileInd = 411; tileTopInd = 768; palRow = 3; miniCol = 5; break; //Industrial_Stage1High_TopCenter
                case 547: tileInd = 412; tileTopInd = 768; palRow = 3; miniCol = 5; break; //Industrial_Stage1High_TopRight
                case 548: tileInd = 413; tileTopInd = 768; palRow = 3; miniCol = 12; break; //Industrial_Stage1High_CenterLeft
                case 549: tileInd = 930; tileTopInd = 768; palRow = 3; miniCol = 5; break; //Industrial_Stage1High_Center
                case 550: tileInd = 414; tileTopInd = 768; palRow = 3; miniCol = 5; break; //Industrial_Stage1High_CenterRight
                case 551: tileInd = 931; tileTopInd = 768; palRow = 3; miniCol = 12; break; //Industrial_Stage1High_BottomLeft
                case 552: tileInd = 415; tileTopInd = 768; palRow = 3; miniCol = 12; break; //Industrial_Stage1High_BottomCenter
                case 553: tileInd = 416; tileTopInd = 768; palRow = 3; miniCol = 12; break; //Industrial_Stage1High_BottomRight
                case 554: tileInd = 417; tileTopInd = 768; palRow = 3; miniCol = 12; break; //Industrial_Stage2High_TopLeft
                case 555: tileInd = 418; tileTopInd = 768; palRow = 3; miniCol = 5; break; //Industrial_Stage2High_TopCenter
                case 556: tileInd = 419; tileTopInd = 768; palRow = 3; miniCol = 5; break; //Industrial_Stage2High_TopRight
                case 557: tileInd = 420; tileTopInd = 768; palRow = 3; miniCol = 12; break; //Industrial_Stage2High_CenterLeft
                case 558: tileInd = 932; tileTopInd = 768; palRow = 3; miniCol = 5; break; //Industrial_Stage2High_Center
                case 559: tileInd = 421; tileTopInd = 768; palRow = 3; miniCol = 5; break; //Industrial_Stage2High_CenterRight
                case 560: tileInd = 933; tileTopInd = 768; palRow = 3; miniCol = 12; break; //Industrial_Stage2High_BottomLeft
                case 561: tileInd = 934; tileTopInd = 768; palRow = 3; miniCol = 12; break; //Industrial_Stage2High_BottomCenter
                case 562: tileInd = 422; tileTopInd = 768; palRow = 3; miniCol = 12; break; //Industrial_Stage2High_BottomRight
                case 563: tileInd = 936; tileTopInd = 768; palRow = 3; miniCol = 12; break; //Industrial_Stage3High_TopLeft
                case 564: tileInd = 937; tileTopInd = 768; palRow = 3; miniCol = 5; break; //Industrial_Stage3High_TopCenter
                case 565: tileInd = 423; tileTopInd = 935; palRow = 3; miniCol = 5; break; //Industrial_Stage3High_TopRight
                case 566: tileInd = 938; tileTopInd = 768; palRow = 3; miniCol = 12; break; //Industrial_Stage3High_CenterLeft
                case 567: tileInd = 939; tileTopInd = 768; palRow = 3; miniCol = 5; break; //Industrial_Stage3High_Center
                case 568: tileInd = 424; tileTopInd = 768; palRow = 3; miniCol = 5; break; //Industrial_Stage3High_CenterRight
                case 569: tileInd = 940; tileTopInd = 768; palRow = 3; miniCol = 12; break; //Industrial_Stage3High_BottomLeft
                case 570: tileInd = 941; tileTopInd = 768; palRow = 3; miniCol = 12; break; //Industrial_Stage3High_BottomCenter
                case 571: tileInd = 425; tileTopInd = 768; palRow = 3; miniCol = 12; break; //Industrial_Stage3High_BottomRight
                case 572: tileInd = 427; tileTopInd = 768; palRow = 3; miniCol = 12; break; //Industrial_Stage4High_TopLeft
                case 573: tileInd = 943; tileTopInd = 426; palRow = 3; miniCol = 5; break; //Industrial_Stage4High_TopCenter
                case 574: tileInd = 428; tileTopInd = 942; palRow = 3; miniCol = 5; break; //Industrial_Stage4High_TopRight
                case 575: tileInd = 944; tileTopInd = 768; palRow = 3; miniCol = 12; break; //Industrial_Stage4High_CenterLeft
                case 576: tileInd = 945; tileTopInd = 768; palRow = 3; miniCol = 5; break; //Industrial_Stage4High_Center
                case 577: tileInd = 429; tileTopInd = 768; palRow = 3; miniCol = 5; break; //Industrial_Stage4High_CenterRight
                case 578: tileInd = 946; tileTopInd = 768; palRow = 3; miniCol = 12; break; //Industrial_Stage4High_BottomLeft
                case 579: tileInd = 430; tileTopInd = 768; palRow = 3; miniCol = 12; break; //Industrial_Stage4High_BottomCenter
                case 580: tileInd = 431; tileTopInd = 768; palRow = 3; miniCol = 12; break; //Industrial_Stage4High_BottomRight
                case 581: tileInd = 443; tileTopInd = 768; palRow = 2; miniCol = 12; break; //PoliceStation_TopLeft
                case 582: tileInd = 444; tileTopInd = 768; palRow = 2; miniCol = 6; break; //PoliceStation_TopCenter
                case 583: tileInd = 445; tileTopInd = 768; palRow = 2; miniCol = 6; break; //PoliceStation_TopRight
                case 584: tileInd = 446; tileTopInd = 768; palRow = 2; miniCol = 12; break; //PoliceStation_CenterLeft
                case 585: tileInd = 447; tileTopInd = 768; palRow = 2; miniCol = 6; break; //PoliceStation_Center
                case 586: tileInd = 448; tileTopInd = 768; palRow = 2; miniCol = 6; break; //PoliceStation_CenterRight
                case 587: tileInd = 449; tileTopInd = 768; palRow = 2; miniCol = 12; break; //PoliceStation_BottomLeft
                case 588: tileInd = 450; tileTopInd = 768; palRow = 2; miniCol = 12; break; //PoliceStation_BottomCenter
                case 589: tileInd = 451; tileTopInd = 768; palRow = 2; miniCol = 12; break; //PoliceStation_BottomRight
                case 590: tileInd = 434; tileTopInd = 768; palRow = 1; miniCol = 12; break; //FireDepartment_TopLeft
                case 591: tileInd = 435; tileTopInd = 432; palRow = 1; miniCol = 6; break; //FireDepartment_TopCenter
                case 592: tileInd = 436; tileTopInd = 433; palRow = 1; miniCol = 6; break; //FireDepartment_TopRight
                case 593: tileInd = 437; tileTopInd = 768; palRow = 1; miniCol = 12; break; //FireDepartment_CenterLeft
                case 594: tileInd = 438; tileTopInd = 768; palRow = 1; miniCol = 6; break; //FireDepartment_Center
                case 595: tileInd = 439; tileTopInd = 768; palRow = 1; miniCol = 6; break; //FireDepartment_CenterRight
                case 596: tileInd = 440; tileTopInd = 768; palRow = 1; miniCol = 12; break; //FireDepartment_BottomLeft
                case 597: tileInd = 441; tileTopInd = 768; palRow = 1; miniCol = 12; break; //FireDepartment_BottomCenter
                case 598: tileInd = 442; tileTopInd = 768; palRow = 1; miniCol = 12; break; //FireDepartment_BottomRight
                case 599: tileInd = 953; tileTopInd = 768; palRow = 2; miniCol = 12; break; //StadiumOpen_TopLeft
                case 600: tileInd = 954; tileTopInd = 768; palRow = 2; miniCol = 6; break; //StadiumOpen_TopCenterL
                case 601: tileInd = 955; tileTopInd = 484; palRow = 2; miniCol = 6; break; //StadiumOpen_TopCenterR
                case 602: tileInd = 485; tileTopInd = 768; palRow = 2; miniCol = 6; break; //StadiumOpen_TopRight
                case 603: tileInd = 956; tileTopInd = 768; palRow = 2; miniCol = 12; break; //StadiumOpen_UpperLeft
                case 604: tileInd = 957; tileTopInd = 768; palRow = 2; miniCol = 6; break; //StadiumOpen_UpperCenterL
                case 605: tileInd = 958; tileTopInd = 768; palRow = 2; miniCol = 6; break; //StadiumOpen_UpperCenterR
                case 606: tileInd = 486; tileTopInd = 768; palRow = 2; miniCol = 6; break; //StadimnOpen_UpperRight
                case 607: tileInd = 959; tileTopInd = 768; palRow = 2; miniCol = 12; break; //StadiumOpen_LowerLeft
                case 608: tileInd = 960; tileTopInd = 768; palRow = 2; miniCol = 6; break; //StadiumOpen_LowerCenterL
                case 609: tileInd = 961; tileTopInd = 768; palRow = 2; miniCol = 6; break; //StadiumOpen_LowerCenterR
                case 610: tileInd = 487; tileTopInd = 768; palRow = 2; miniCol = 6; break; //StadiumOpen_LowerRight
                case 611: tileInd = 962; tileTopInd = 768; palRow = 2; miniCol = 12; break; //StadiumOpen_BottomLeft
                case 612: tileInd = 963; tileTopInd = 768; palRow = 2; miniCol = 12; break; //StadiumOpen_BottomCenterL
                case 613: tileInd = 964; tileTopInd = 768; palRow = 2; miniCol = 12; break; //StadiumOpen_BottomCenterR
                case 614: tileInd = 488; tileTopInd = 768; palRow = 2; miniCol = 12; break; //StadiumOpen_BottomRight
                case 615: tileInd = 965; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Seaport_TopLeft
                case 616: tileInd = 489; tileTopInd = 768; palRow = 2; miniCol = 6; break; //Seaport_TopCenterL
                case 617: tileInd = 490; tileTopInd = 768; palRow = 2; miniCol = 6; break; //Seaport_TopCenterR
                case 618: tileInd = 491; tileTopInd = 768; palRow = 2; miniCol = 6; break; //Seaport_TopRight
                case 619: tileInd = 492; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Seaport_UpperLeft
                case 620: tileInd = 493; tileTopInd = 768; palRow = 2; miniCol = 6; break; //Seaport_UpperCenterL
                case 621: tileInd = 494; tileTopInd = 768; palRow = 2; miniCol = 6; break; //Seaport_UpperCenterR
                case 622: tileInd = 495; tileTopInd = 768; palRow = 2; miniCol = 6; break; //Seaport_UpperRight
                case 623: tileInd = 496; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Seaport_LowerLeft
                case 624: tileInd = 497; tileTopInd = 768; palRow = 2; miniCol = 6; break; //Seaport_LowerCenterL
                case 625: tileInd = 498; tileTopInd = 768; palRow = 2; miniCol = 6; break; //Seaport_LowerCenterR
                case 626: tileInd = 499; tileTopInd = 768; palRow = 2; miniCol = 6; break; //Seaport_LowerRight
                case 627: tileInd = 500; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Seaport_BottomLeft
                case 628: tileInd = 966; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Seaport_BottomCenterL
                case 629: tileInd = 967; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Seaport_BottomCenterR
                case 630: tileInd = 968; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Seaport_BottomRight
                case 631: tileInd = 456; tileTopInd = 768; palRow = 6; miniCol = 12; break; //NuclearPlant_TopLeft
                case 632: tileInd = 457; tileTopInd = 452; palRow = 6; miniCol = 6; break; //NuclearPlant_TopCenterL
                case 633: tileInd = 458; tileTopInd = 453; palRow = 6; miniCol = 6; break; //NuclearPlant_TopCenterR
                case 634: tileInd = 459; tileTopInd = 454; palRow = 6; miniCol = 6; break; //NuclearPlant_TopRight
                case 635: tileInd = 460; tileTopInd = 455; palRow = 6; miniCol = 12; break; //NuclearPlant_UpperLeft
                case 636: tileInd = 461; tileTopInd = 768; palRow = 6; miniCol = 6; break; //NuclearPlant_UpperCenterL
                case 637: tileInd = 462; tileTopInd = 768; palRow = 6; miniCol = 6; break; //NuclearPlant_UpperCenterR
                case 638: tileInd = 463; tileTopInd = 768; palRow = 6; miniCol = 6; break; //NuclearPlant_UpperRight
                case 639: tileInd = 464; tileTopInd = 768; palRow = 6; miniCol = 12; break; //NuclearPlant_LowerLeft
                case 640: tileInd = 465; tileTopInd = 768; palRow = 6; miniCol = 6; break; //NuclearPlant_LowerCenterL
                case 641: tileInd = 466; tileTopInd = 768; palRow = 6; miniCol = 6; break; //NuclearPlant_LowerCenterR
                case 642: tileInd = 467; tileTopInd = 768; palRow = 6; miniCol = 6; break; //NuclearPlant_LowerRight
                case 643: tileInd = 468; tileTopInd = 768; palRow = 6; miniCol = 12; break; //NuclearPlant_BottomLeft
                case 644: tileInd = 469; tileTopInd = 768; palRow = 6; miniCol = 12; break; //NuclearPlant_BottomCenterL
                case 645: tileInd = 470; tileTopInd = 768; palRow = 6; miniCol = 12; break; //NuclearPlant_BottomCenterR
                case 646: tileInd = 471; tileTopInd = 768; palRow = 6; miniCol = 12; break; //NuclearPlant_BottomRight
                case 647: tileInd = 947; tileTopInd = 768; palRow = 6; miniCol = 12; break; //CoalPlant_TopLeft
                case 648: tileInd = 948; tileTopInd = 472; palRow = 6; miniCol = 6; break; //CoalPlant_TopCenterL
                case 649: tileInd = 949; tileTopInd = 473; palRow = 6; miniCol = 6; break; //CoalPlant_TopCenterR
                case 650: tileInd = 474; tileTopInd = 768; palRow = 6; miniCol = 6; break; //CoalPlant_TopRight
                case 651: tileInd = 950; tileTopInd = 768; palRow = 6; miniCol = 12; break; //CoalPlant_UpperLeft
                case 652: tileInd = 951; tileTopInd = 768; palRow = 6; miniCol = 6; break; //CoalPlant_UpperCenterL
                case 653: tileInd = 952; tileTopInd = 768; palRow = 6; miniCol = 6; break; //CoalPlant_UpperCenterR
                case 654: tileInd = 475; tileTopInd = 768; palRow = 6; miniCol = 6; break; //CoalPlant_UpperRight
                case 655: tileInd = 476; tileTopInd = 768; palRow = 6; miniCol = 12; break; //CoalPlant_LowerLeft
                case 656: tileInd = 477; tileTopInd = 768; palRow = 6; miniCol = 6; break; //CoalPlant_LowerCenterL
                case 657: tileInd = 478; tileTopInd = 768; palRow = 6; miniCol = 6; break; //CoalPlant_LowerCenterR
                case 658: tileInd = 479; tileTopInd = 768; palRow = 6; miniCol = 6; break; //CoalPlant_LowerRight
                case 659: tileInd = 480; tileTopInd = 768; palRow = 6; miniCol = 12; break; //CoalPlant_BottomLeft
                case 660: tileInd = 481; tileTopInd = 768; palRow = 6; miniCol = 12; break; //CoalPlant_BottomCenterL
                case 661: tileInd = 482; tileTopInd = 768; palRow = 6; miniCol = 12; break; //CoalPlant_BottomCenterR
                case 662: tileInd = 483; tileTopInd = 768; palRow = 6; miniCol = 12; break; //CoalPlant_BottomRight
                case 663: tileInd = 501; tileTopInd = 768; palRow = 6; miniCol = 6; break; //Airport_TopLeft
                case 664: tileInd = 502; tileTopInd = 768; palRow = 6; miniCol = 6; break; //Airport_TopRow2
                case 665: tileInd = 503; tileTopInd = 768; palRow = 6; miniCol = 6; break; //Airport_TopRow3
                case 666: tileInd = 504; tileTopInd = 768; palRow = 6; miniCol = 6; break; //Airport_TopRow4
                case 667: tileInd = 505; tileTopInd = 768; palRow = 6; miniCol = 6; break; //Airport_TopRow5
                case 668: tileInd = 506; tileTopInd = 768; palRow = 6; miniCol = 6; break; //Airport_TopRight
                case 669: tileInd = 507; tileTopInd = 768; palRow = 6; miniCol = 6; break; //Airport_2ndRowLeft
                case 670: tileInd = 508; tileTopInd = 768; palRow = 6; miniCol = 6; break; //Airport_2ndRow2
                case 671: tileInd = 509; tileTopInd = 768; palRow = 6; miniCol = 6; break; //Airport_2ndRow3
                case 672: tileInd = 510; tileTopInd = 768; palRow = 6; miniCol = 6; break; //Airport_2ndRow4
                case 673: tileInd = 511; tileTopInd = 768; palRow = 6; miniCol = 6; break; //Airport_2ndRow5
                case 674: tileInd = 512; tileTopInd = 768; palRow = 6; miniCol = 6; break; //Airport_2ndRowRight
                case 675: tileInd = 513; tileTopInd = 768; palRow = 6; miniCol = 6; break; //Airport_3rdRowLeft
                case 676: tileInd = 514; tileTopInd = 768; palRow = 6; miniCol = 6; break; //Airport_3rdRow2
                case 677: tileInd = 515; tileTopInd = 768; palRow = 6; miniCol = 6; break; //Airport_3rdRow3
                case 678: tileInd = 516; tileTopInd = 768; palRow = 6; miniCol = 6; break; //Airport_3rdRow4
                case 679: tileInd = 517; tileTopInd = 768; palRow = 6; miniCol = 6; break; //Airport_3rdRow5
                case 680: tileInd = 518; tileTopInd = 768; palRow = 6; miniCol = 6; break; //Airport_3rdRowRight
                case 681: tileInd = 519; tileTopInd = 768; palRow = 6; miniCol = 6; break; //Airport_4thRowLeft
                case 682: tileInd = 520; tileTopInd = 768; palRow = 6; miniCol = 6; break; //Airport_4thRow2
                case 683: tileInd = 521; tileTopInd = 768; palRow = 6; miniCol = 6; break; //Airport_4thRow3
                case 684: tileInd = 522; tileTopInd = 768; palRow = 6; miniCol = 6; break; //Airport_4thRow4
                case 685: tileInd = 523; tileTopInd = 768; palRow = 6; miniCol = 6; break; //Airport_4thRow5
                case 686: tileInd = 524; tileTopInd = 768; palRow = 6; miniCol = 6; break; //Airport_4thRowRight
                case 687: tileInd = 525; tileTopInd = 768; palRow = 6; miniCol = 6; break; //Airport_5thRowLeft
                case 688: tileInd = 526; tileTopInd = 768; palRow = 6; miniCol = 6; break; //Airport_5thRow2
                case 689: tileInd = 527; tileTopInd = 768; palRow = 6; miniCol = 6; break; //Airport_5thRow3
                case 690: tileInd = 528; tileTopInd = 768; palRow = 6; miniCol = 6; break; //Airport_5thRow4
                case 691: tileInd = 969; tileTopInd = 768; palRow = 6; miniCol = 6; break; //Airport_5thRow5
                case 692: tileInd = 529; tileTopInd = 768; palRow = 6; miniCol = 6; break; //Airport_5thRowRight
                case 693: tileInd = 530; tileTopInd = 768; palRow = 6; miniCol = 6; break; //Airport_BottomLeft
                case 694: tileInd = 531; tileTopInd = 768; palRow = 6; miniCol = 6; break; //Airport_BottomRow2
                case 695: tileInd = 532; tileTopInd = 768; palRow = 6; miniCol = 6; break; //Airport_BottomRow3
                case 696: tileInd = 533; tileTopInd = 768; palRow = 6; miniCol = 6; break; //Airport_BottomRow4
                case 697: tileInd = 534; tileTopInd = 768; palRow = 6; miniCol = 6; break; //Airport_BottomRow5
                case 698: tileInd = 535; tileTopInd = 768; palRow = 6; miniCol = 6; break; //Airport_BottomRight
                case 699: tileInd = 536; tileTopInd = 768; palRow = 7; miniCol = 12; break; //MayorsHouse_Stage1_TopLeft
                case 700: tileInd = 537; tileTopInd = 768; palRow = 7; miniCol = 6; break; //MayorsHouse_Stage1_TopCenter
                case 701: tileInd = 538; tileTopInd = 768; palRow = 7; miniCol = 6; break; //MayorsHouse_Stage1_TopRight
                case 702: tileInd = 539; tileTopInd = 768; palRow = 7; miniCol = 12; break; //MayorsHouse_Stage1_CenterLeft
                case 703: tileInd = 540; tileTopInd = 768; palRow = 7; miniCol = 6; break; //HayarsHouse_Stage1_Center
                case 704: tileInd = 541; tileTopInd = 768; palRow = 7; miniCol = 6; break; //MayorsHouse_Stage1_CenterRight
                case 705: tileInd = 542; tileTopInd = 768; palRow = 7; miniCol = 12; break; //MayorsHouse_Stage1_BottomLeft
                case 706: tileInd = 543; tileTopInd = 768; palRow = 7; miniCol = 12; break; //MayorsHouse_Stage1_BottomCenter
                case 707: tileInd = 544; tileTopInd = 768; palRow = 7; miniCol = 12; break; //MayorsHouse_Stage1_BottomRight
                case 708: tileInd = 545; tileTopInd = 768; palRow = 7; miniCol = 12; break; //MayorsHouse_Stage2_TopLeft
                case 709: tileInd = 546; tileTopInd = 768; palRow = 7; miniCol = 6; break; //MayorsHouse_Stage2_TopCenter
                case 710: tileInd = 547; tileTopInd = 768; palRow = 7; miniCol = 6; break; //MayorsHouse_Stage2_TopRight
                case 711: tileInd = 548; tileTopInd = 768; palRow = 7; miniCol = 12; break; //MayorsHouse_Stage2_CenterLeft
                case 712: tileInd = 549; tileTopInd = 768; palRow = 7; miniCol = 6; break; //MayorsHouse_Stage2_Center
                case 713: tileInd = 550; tileTopInd = 768; palRow = 7; miniCol = 6; break; //MayorsHouse_Stage2_CenterRight
                case 714: tileInd = 551; tileTopInd = 768; palRow = 7; miniCol = 12; break; //MayorsHouse_Stage2_BottomLeft
                case 715: tileInd = 552; tileTopInd = 768; palRow = 7; miniCol = 12; break; //MayorsHouse_Stage2_BottomCenter
                case 716: tileInd = 553; tileTopInd = 768; palRow = 7; miniCol = 12; break; //MayorsHouse_Stage2_BottomRight
                case 717: tileInd = 554; tileTopInd = 768; palRow = 7; miniCol = 12; break; //MayorsHouse_Stage3_TopLeft
                case 718: tileInd = 555; tileTopInd = 768; palRow = 7; miniCol = 6; break; //MayorsHouse_Stage3_TopCenter
                case 719: tileInd = 556; tileTopInd = 768; palRow = 7; miniCol = 6; break; //MayorsHouse_Stage3_TopRight
                case 720: tileInd = 557; tileTopInd = 768; palRow = 7; miniCol = 12; break; //MayorsHouse_Stage3_CenterLeft
                case 721: tileInd = 558; tileTopInd = 768; palRow = 7; miniCol = 6; break; //MayorsHouse_Stage3_Center
                case 722: tileInd = 559; tileTopInd = 768; palRow = 7; miniCol = 6; break; //MayorsHouse_Stage3_CenterRight
                case 723: tileInd = 560; tileTopInd = 768; palRow = 7; miniCol = 12; break; //MayorsHouse_Stage3_BottomLeft
                case 724: tileInd = 561; tileTopInd = 768; palRow = 7; miniCol = 12; break; //MayorsHouse_Stage3_BottomCenter
                case 725: tileInd = 562; tileTopInd = 768; palRow = 7; miniCol = 12; break; //MayorsHouse_Stage3_BottomRight
                case 726: tileInd = 563; tileTopInd = 768; palRow = 7; miniCol = 12; break; //MayorsHouse_Stage4_TopLeft
                case 727: tileInd = 564; tileTopInd = 768; palRow = 7; miniCol = 6; break; //MayorsHouse_Stage4_TopCenter
                case 728: tileInd = 565; tileTopInd = 768; palRow = 7; miniCol = 6; break; //MayorsHouse_Stage4_TopRight
                case 729: tileInd = 566; tileTopInd = 768; palRow = 7; miniCol = 12; break; //MayorsHouse_Stage4_CenterLeft
                case 730: tileInd = 567; tileTopInd = 768; palRow = 7; miniCol = 6; break; //MayorsHouse_Stage4_Center
                case 731: tileInd = 568; tileTopInd = 768; palRow = 7; miniCol = 6; break; //MayorsHouse_Stage4_CenterRight
                case 732: tileInd = 569; tileTopInd = 768; palRow = 7; miniCol = 12; break; //MayorsHouse_Stage4_BottomLeft
                case 733: tileInd = 570; tileTopInd = 768; palRow = 7; miniCol = 12; break; //MayorsHouse_Stage4_BottomCenter
                case 734: tileInd = 571; tileTopInd = 768; palRow = 7; miniCol = 12; break; //MayorsHouse_Stage4_BottomRight
                case 735: tileInd = 572; tileTopInd = 768; palRow = 1; miniCol = 12; break; //Bank_TopLeft
                case 736: tileInd = 573; tileTopInd = 768; palRow = 1; miniCol = 6; break; //Bank_TopCenter
                case 737: tileInd = 574; tileTopInd = 768; palRow = 1; miniCol = 6; break; //Bank_TopRight
                case 738: tileInd = 575; tileTopInd = 768; palRow = 1; miniCol = 12; break; //Bank_CenterLeft
                case 739: tileInd = 576; tileTopInd = 768; palRow = 1; miniCol = 6; break; //Bank_Center
                case 740: tileInd = 577; tileTopInd = 768; palRow = 1; miniCol = 6; break; //Bank_CenterRight
                case 741: tileInd = 578; tileTopInd = 768; palRow = 1; miniCol = 12; break; //Bank_BottomLeft
                case 742: tileInd = 579; tileTopInd = 768; palRow = 1; miniCol = 12; break; //Bank_BottomCenter
                case 743: tileInd = 580; tileTopInd = 768; palRow = 1; miniCol = 12; break; //Bank_BottomRight
                case 744: tileInd = 972; tileTopInd = 768; palRow = 7; miniCol = 12; break; //AmusementPark_TopLeft
                case 745: tileInd = 973; tileTopInd = 970; palRow = 7; miniCol = 6; break; //AmusementPark_TopCenter
                case 746: tileInd = 581; tileTopInd = 971; palRow = 7; miniCol = 6; break; //AmusementPark_TopRight
                case 747: tileInd = 974; tileTopInd = 768; palRow = 7; miniCol = 12; break; //AmusementPark_CenterLeft
                case 748: tileInd = 975; tileTopInd = 768; palRow = 7; miniCol = 6; break; //AmusementPark_Center
                case 749: tileInd = 976; tileTopInd = 768; palRow = 7; miniCol = 6; break; //AmusementPark_CenterRight
                case 750: tileInd = 977; tileTopInd = 768; palRow = 7; miniCol = 12; break; //AmusementPark_BottomLeft
                case 751: tileInd = 978; tileTopInd = 768; palRow = 7; miniCol = 12; break; //AmusementPark_BottomCenter
                case 752: tileInd = 979; tileTopInd = 768; palRow = 7; miniCol = 12; break; //AmusementPark_BottomRight
                case 753: tileInd = 582; tileTopInd = 768; palRow = 7; miniCol = 12; break; //Zoo_TopLeft
                case 754: tileInd = 583; tileTopInd = 768; palRow = 7; miniCol = 6; break; //Zoo_TopCenter
                case 755: tileInd = 980; tileTopInd = 768; palRow = 7; miniCol = 6; break; //Zoo_TopRight
                case 756: tileInd = 981; tileTopInd = 768; palRow = 7; miniCol = 12; break; //Zoo_CenterLeft
                case 757: tileInd = 982; tileTopInd = 768; palRow = 7; miniCol = 6; break; //Zoo_Center
                case 758: tileInd = 983; tileTopInd = 768; palRow = 7; miniCol = 6; break; //Zoo_CenterRight
                case 759: tileInd = 984; tileTopInd = 768; palRow = 7; miniCol = 12; break; //Zoo_BottomLeft
                case 760: tileInd = 985; tileTopInd = 768; palRow = 7; miniCol = 12; break; //Zoo_BottomCenter
                case 761: tileInd = 986; tileTopInd = 768; palRow = 7; miniCol = 12; break; //Zoo_BottomRight
                case 762: tileInd = 988; tileTopInd = 768; palRow = 7; miniCol = 12; break; //Casino_TopLeft
                case 763: tileInd = 989; tileTopInd = 987; palRow = 7; miniCol = 6; break; //Casino_TopCenter
                case 764: tileInd = 990; tileTopInd = 768; palRow = 7; miniCol = 6; break; //Casino_TopRight
                case 765: tileInd = 584; tileTopInd = 768; palRow = 7; miniCol = 12; break; //Casino_CenterLeft
                case 766: tileInd = 991; tileTopInd = 768; palRow = 7; miniCol = 6; break; //Casino_Center
                case 767: tileInd = 992; tileTopInd = 768; palRow = 7; miniCol = 6; break; //Casino_CenterRight
                case 768: tileInd = 993; tileTopInd = 768; palRow = 7; miniCol = 12; break; //Casino_BottomLeft
                case 769: tileInd = 585; tileTopInd = 768; palRow = 7; miniCol = 12; break; //Casino_BottomCenter
                case 770: tileInd = 994; tileTopInd = 768; palRow = 7; miniCol = 12; break; //Casino_BottomRight
                case 771: tileInd = 995; tileTopInd = 768; palRow = 2; miniCol = 12; break; //PoliceHQ_TopLeft
                case 772: tileInd = 996; tileTopInd = 768; palRow = 2; miniCol = 6; break; //PoliceHQ_TopCenter
                case 773: tileInd = 586; tileTopInd = 768; palRow = 2; miniCol = 6; break; //PoliceHQ_TopRight
                case 774: tileInd = 997; tileTopInd = 768; palRow = 2; miniCol = 12; break; //PoliceHQ_CenterLeft
                case 775: tileInd = 998; tileTopInd = 768; palRow = 2; miniCol = 6; break; //PoliceHQ_Center
                case 776: tileInd = 587; tileTopInd = 768; palRow = 2; miniCol = 6; break; //PoliceHQ_CenterRight
                case 777: tileInd = 588; tileTopInd = 768; palRow = 2; miniCol = 12; break; //PoliceHQ_BottomLeft
                case 778: tileInd = 589; tileTopInd = 768; palRow = 2; miniCol = 12; break; //PoliceHQ_BottomCenter
                case 779: tileInd = 590; tileTopInd = 768; palRow = 2; miniCol = 12; break; //PoliceHQ_BottomRight
                case 780: tileInd = 999; tileTopInd = 768; palRow = 1; miniCol = 12; break; //FireHQ_TopLeft
                case 781: tileInd = 1000; tileTopInd = 591; palRow = 1; miniCol = 6; break; //FireHQ_TopCenter
                case 782: tileInd = 593; tileTopInd = 592; palRow = 1; miniCol = 6; break; //FireHQ_TopRight
                case 783: tileInd = 1001; tileTopInd = 768; palRow = 1; miniCol = 12; break; //FireHQ_CenterLeft
                case 784: tileInd = 1002; tileTopInd = 768; palRow = 1; miniCol = 6; break; //FireHQ_Center
                case 785: tileInd = 594; tileTopInd = 768; palRow = 1; miniCol = 6; break; //FireHQ_CenterRight
                case 786: tileInd = 595; tileTopInd = 768; palRow = 1; miniCol = 12; break; //FireHQ_BottomLeft
                case 787: tileInd = 596; tileTopInd = 768; palRow = 1; miniCol = 12; break; //FireHQ_BottomCenter
                case 788: tileInd = 597; tileTopInd = 768; palRow = 1; miniCol = 12; break; //FireHQ_BottomRight
                case 789: tileInd = 598; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Fountain_TopLeft
                case 790: tileInd = 1003; tileTopInd = 768; palRow = 2; miniCol = 6; break; //Fountain_TopCenter
                case 791: tileInd = 599; tileTopInd = 768; palRow = 2; miniCol = 6; break; //Fountain_TopRight
                case 792: tileInd = 1004; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Fountain_CenterLeft
                case 793: tileInd = 1005; tileTopInd = 768; palRow = 2; miniCol = 6; break; //Fountain_Center
                case 794: tileInd = 1006; tileTopInd = 768; palRow = 2; miniCol = 6; break; //Fountain_CenterRight
                case 795: tileInd = 600; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Fountain_BottomLeft
                case 796: tileInd = 1007; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Fountain_BottomCenter
                case 797: tileInd = 601; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Fountain_BottomRight
                case 798: tileInd = 606; tileTopInd = 602; palRow = 7; miniCol = 12; break; //MarioStatue_TopLeft
                case 799: tileInd = 607; tileTopInd = 603; palRow = 7; miniCol = 6; break; //MarioStatue_TopCenter
                case 800: tileInd = 608; tileTopInd = 604; palRow = 7; miniCol = 6; break; //MarioStatue_TopRight
                case 801: tileInd = 609; tileTopInd = 605; palRow = 7; miniCol = 12; break; //MarioStatue_CenterLeft
                case 802: tileInd = 610; tileTopInd = 768; palRow = 7; miniCol = 6; break; //MarioStatue_Center
                case 803: tileInd = 611; tileTopInd = 768; palRow = 7; miniCol = 6; break; //MarioStatue_CenterRight
                case 804: tileInd = 612; tileTopInd = 768; palRow = 7; miniCol = 12; break; //MarioStatue_BottomLeft
                case 805: tileInd = 613; tileTopInd = 768; palRow = 7; miniCol = 12; break; //MarioStatue_BottomCenter
                case 806: tileInd = 614; tileTopInd = 768; palRow = 7; miniCol = 12; break; //MarioStatue_BottomRight
                case 807: tileInd = 1017; tileTopInd = 768; palRow = 7; miniCol = 12; break; //Expo_TopLeft
                case 808: tileInd = 861; tileTopInd = 859; palRow = 7; miniCol = 6; break; //Expo_TopCenter
                case 809: tileInd = 1020; tileTopInd = 860; palRow = 7; miniCol = 6; break; //Expo_TopRight
                case 810: tileInd = 1018; tileTopInd = 768; palRow = 7; miniCol = 12; break; //Expo_CenterLeft
                case 811: tileInd = 862; tileTopInd = 768; palRow = 7; miniCol = 6; break; //Expo_Center
                case 812: tileInd = 1021; tileTopInd = 768; palRow = 7; miniCol = 6; break; //Expo_CenterRight
                case 813: tileInd = 1019; tileTopInd = 768; palRow = 7; miniCol = 12; break; //Expo_BottomLeft
                case 814: tileInd = 863; tileTopInd = 768; palRow = 7; miniCol = 12; break; //Expo_BottomCenter
                case 815: tileInd = 1022; tileTopInd = 768; palRow = 7; miniCol = 12; break; //Expo_BottomRight
                case 816: tileInd = 1008; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Windmill_TopLeft
                case 817: tileInd = 1009; tileTopInd = 768; palRow = 2; miniCol = 6; break; //Windmill_TopCenter
                case 818: tileInd = 1010; tileTopInd = 768; palRow = 2; miniCol = 6; break; //Windmill_TopRight
                case 819: tileInd = 1011; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Windmill_CenterLeft
                case 820: tileInd = 1012; tileTopInd = 768; palRow = 2; miniCol = 6; break; //Windmill_Center
                case 821: tileInd = 1013; tileTopInd = 768; palRow = 2; miniCol = 6; break; //Windmill_CenterRight
                case 822: tileInd = 626; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Windmill_BottomLeft
                case 823: tileInd = 1014; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Windmill_BottomCenter
                case 824: tileInd = 1015; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Windmill_BottomRight
                case 825: tileInd = 627; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Library_TopLeft
                case 826: tileInd = 628; tileTopInd = 768; palRow = 2; miniCol = 6; break; //Library_TopCenter
                case 827: tileInd = 629; tileTopInd = 768; palRow = 2; miniCol = 6; break; //Library_TopRight
                case 828: tileInd = 630; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Library_CenterLeft
                case 829: tileInd = 631; tileTopInd = 768; palRow = 2; miniCol = 6; break; //Library_Center
                case 830: tileInd = 632; tileTopInd = 768; palRow = 2; miniCol = 6; break; //Library_CenterRight
                case 831: tileInd = 633; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Library_BottomLeft
                case 832: tileInd = 634; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Library_BottomCenter
                case 833: tileInd = 635; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Library_BottomRight
                case 834: tileInd = 636; tileTopInd = 768; palRow = 7; miniCol = 12; break; //LargePark_TopLeft
                case 835: tileInd = 637; tileTopInd = 768; palRow = 7; miniCol = 6; break; //LargePark_TopCenter
                case 836: tileInd = 638; tileTopInd = 768; palRow = 7; miniCol = 6; break; //LargePark_TopRight
                case 837: tileInd = 639; tileTopInd = 768; palRow = 7; miniCol = 12; break; //LargePark_CenterLeft
                case 838: tileInd = 640; tileTopInd = 768; palRow = 7; miniCol = 6; break; //LargePark_Center
                case 839: tileInd = 641; tileTopInd = 768; palRow = 7; miniCol = 6; break; //LargePark_CenterRight
                case 840: tileInd = 642; tileTopInd = 768; palRow = 7; miniCol = 12; break; //LargePark_BottomLeft
                case 841: tileInd = 643; tileTopInd = 768; palRow = 7; miniCol = 12; break; //LargePark_BottomCenter
                case 842: tileInd = 644; tileTopInd = 768; palRow = 7; miniCol = 12; break; //LargePark_BottomRight
                case 843: tileInd = 645; tileTopInd = 768; palRow = 2; miniCol = 12; break; //TrainStation_TopLeft
                case 844: tileInd = 646; tileTopInd = 768; palRow = 2; miniCol = 6; break; //TrainStation_TopCenter
                case 845: tileInd = 647; tileTopInd = 768; palRow = 2; miniCol = 6; break; //TrainStation_TopRight
                case 846: tileInd = 648; tileTopInd = 768; palRow = 2; miniCol = 12; break; //TrainStation_CenterLeft
                case 847: tileInd = 649; tileTopInd = 768; palRow = 2; miniCol = 6; break; //TrainStation_Center
                case 848: tileInd = 650; tileTopInd = 768; palRow = 2; miniCol = 6; break; //TrainStation_CenterRight
                case 849: tileInd = 651; tileTopInd = 768; palRow = 2; miniCol = 12; break; //TrainStation_BottomLeft
                case 850: tileInd = 652; tileTopInd = 768; palRow = 2; miniCol = 12; break; //TrainStation_BottomCenter
                case 851: tileInd = 653; tileTopInd = 768; palRow = 2; miniCol = 12; break; //TrainStation_BottomRight
                case 852: tileInd = 880; tileTopInd = 768; palRow = 1; miniCol = 13; break; //Road_DrawbridgeClear_A
                case 853: tileInd = 880; tileTopInd = 768; palRow = 1; miniCol = 13; break; //Road_DrawbridgeClear_B
                case 854: tileInd = 682; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Road_DrawbridgeEW_TopLeft
                case 855: tileInd = 683; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Road_DrawbridgeEW_BottomLeft
                case 856: tileInd = 684; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Road_DrawbridgeEW_TopRight
                case 857: tileInd = 685; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Road_DrawbridgeEW_BottomRight
                case 858: tileInd = 686; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Road_DrawbridgeNS_BottomLeft
                case 859: tileInd = 687; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Road_DrawbridgeNS_BottomRight
                case 860: tileInd = 688; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Road_DrawbridgeNS_TopLeft
                case 861: tileInd = 689; tileTopInd = 768; palRow = 1; miniCol = 8; break; //Road_DrawbridgeNS_TopRight
                case 862: tileInd = 0; tileTopInd = 768; palRow = 0; miniCol = 8; break; //(Unused)
                case 863: tileInd = 0; tileTopInd = 768; palRow = 0; miniCol = 8; break; //(Unused)
                case 864: tileInd = 694; tileTopInd = 768; palRow = 1; miniCol = 8; break; //(Unused)
                case 865: tileInd = 695; tileTopInd = 768; palRow = 1; miniCol = 8; break; //(Unused)
                case 866: tileInd = 696; tileTopInd = 768; palRow = 1; miniCol = 8; break; //(Unused)
                case 867: tileInd = 697; tileTopInd = 768; palRow = 1; miniCol = 8; break; //(Unused)
                case 868: tileInd = 676; tileTopInd = 768; palRow = 6; miniCol = 11; break; //Fallout
                case 869: tileInd = 1016; tileTopInd = 768; palRow = 1; miniCol = 11; break; //FloodedLand
                case 870: tileInd = 757; tileTopInd = 768; palRow = 2; miniCol = 12; break; //StadiumDomed_TopLeft
                case 871: tileInd = 758; tileTopInd = 768; palRow = 2; miniCol = 6; break; //StadiumDomed_TopCenterL
                case 872: tileInd = 759; tileTopInd = 768; palRow = 2; miniCol = 6; break; //StadiumDomed_TopCenterR
                case 873: tileInd = 760; tileTopInd = 768; palRow = 2; miniCol = 6; break; //StadiumDomed_TopRight
                case 874: tileInd = 761; tileTopInd = 768; palRow = 2; miniCol = 12; break; //StadiumDomed_UpperLeft
                case 875: tileInd = 762; tileTopInd = 768; palRow = 2; miniCol = 6; break; //StadiumDomed_UpperCenterL
                case 876: tileInd = 763; tileTopInd = 768; palRow = 2; miniCol = 6; break; //StadiumDomed_UpperCenterR
                case 877: tileInd = 764; tileTopInd = 768; palRow = 2; miniCol = 6; break; //StadiumDomed_UpperRight
                case 878: tileInd = 765; tileTopInd = 768; palRow = 2; miniCol = 12; break; //StadiumDomed_LowerLeft
                case 879: tileInd = 766; tileTopInd = 768; palRow = 2; miniCol = 6; break; //StadiumDomed_LowerCenterL
                case 880: tileInd = 767; tileTopInd = 768; palRow = 2; miniCol = 6; break; //StadiumDomed_LowerCenterR
                case 881: tileInd = 770; tileTopInd = 768; palRow = 2; miniCol = 6; break; //StadiumDomed_LowerRight
                case 882: tileInd = 771; tileTopInd = 768; palRow = 2; miniCol = 12; break; //StadiumDomed_BottomLeft
                case 883: tileInd = 772; tileTopInd = 768; palRow = 2; miniCol = 12; break; //StadiumDomed_BottomCenterL
                case 884: tileInd = 773; tileTopInd = 768; palRow = 2; miniCol = 12; break; //StadiumDomed_BottomCenterR
                case 885: tileInd = 774; tileTopInd = 768; palRow = 2; miniCol = 12; break; //StadiumDomed_BottomRight
                case 886: tileInd = 778; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_TopEW_WestHalf_TopLeft
                case 887: tileInd = 779; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_TopEW_WestHalf_TopCenter
                case 888: tileInd = 780; tileTopInd = 775; palRow = 4; miniCol = 3; break; //Residential_TopEW_WestHalf_TopRight
                case 889: tileInd = 784; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_TopEW_WestHalf_CenterLeft
                case 890: tileInd = 785; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_TopEW_WestHalf_Center
                case 891: tileInd = 786; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_TopEW_WestHalf_CenterRight
                case 892: tileInd = 790; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_TopEW_WestHalf_BottomLeft
                case 893: tileInd = 791; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_TopEW_WestHalf_BottomCenter
                case 894: tileInd = 792; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_TopEW_WestHalf_BottomRight
                case 895: tileInd = 781; tileTopInd = 776; palRow = 4; miniCol = 3; break; //Residential_TopEW_EastHalf_TopLeft
                case 896: tileInd = 782; tileTopInd = 777; palRow = 4; miniCol = 3; break; //Residential_TopEW_EastHalf_TopCenter
                case 897: tileInd = 783; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_TopEW_EastHalf_TopRight
                case 898: tileInd = 787; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_TopEW_EastHalf_CenterLeft
                case 899: tileInd = 788; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_TopEW_EastHalf_Center
                case 900: tileInd = 789; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_TopEW_EastHalf_CenterRight
                case 901: tileInd = 793; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_TopEW_EastHalf_BottomLeft
                case 902: tileInd = 794; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_TopEW_EastHalf_BottomCenter
                case 903: tileInd = 795; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_TopEW_EastHalf_BottomRight
                case 904: tileInd = 798; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_TopNS_NorthHalf_TopLeft
                case 905: tileInd = 799; tileTopInd = 796; palRow = 4; miniCol = 3; break; //Residential_TopNS_NorthHalf_TopCenter
                case 906: tileInd = 800; tileTopInd = 797; palRow = 4; miniCol = 3; break; //Residential_TopNS_NorthHalf_TopRight
                case 907: tileInd = 801; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_TopNS_NorthHalf_CenterLeft
                case 908: tileInd = 802; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_TopNS_NorthHalf_Center
                case 909: tileInd = 803; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_TopNS_NorthHalf_CenterRight
                case 910: tileInd = 804; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_TopNS_NorthHalf_BottomLeft
                case 911: tileInd = 805; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_TopNS_NorthHalf_BottomCenter
                case 912: tileInd = 806; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_TopNS_NorthHalf_BottomRight
                case 913: tileInd = 807; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_TopNS_SouthHalf_TopLeft
                case 914: tileInd = 808; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_TopNS_SouthHalf_TopCenter
                case 915: tileInd = 809; tileTopInd = 768; palRow = 4; miniCol = 3; break; //Residential_TopNS_SouthHalf_TopRight
                case 916: tileInd = 810; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_TopNS_SouthHalf_CenterLeft
                case 917: tileInd = 811; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_TopNS_SouthHalf_Center
                case 918: tileInd = 812; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_TopNS_SouthHalf_CenterRight
                case 919: tileInd = 813; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_TopNS_SouthHalf_BottomLeft
                case 920: tileInd = 814; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_TopNS_SouthHalf_BottomCenter
                case 921: tileInd = 815; tileTopInd = 768; palRow = 4; miniCol = 12; break; //Residential_TopNS_SouthHalf_BottomRight
                case 922: tileInd = 821; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_TopEW_WestHalf_TopLeft
                case 923: tileInd = 822; tileTopInd = 816; palRow = 2; miniCol = 4; break; //Commercial_TopEW_WestHalf_TopCenter
                case 924: tileInd = 823; tileTopInd = 817; palRow = 2; miniCol = 4; break; //Commercial_TopEW_WestHalf_TopRight
                case 925: tileInd = 827; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_IOpEW_WestHalf_CenterLeft
                case 926: tileInd = 828; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_TopEW_WestHalf_Center
                case 927: tileInd = 829; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_TopEW_WestHalf_CenterRight
                case 928: tileInd = 833; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_TopEW_WestHalf_BottomLeft
                case 929: tileInd = 834; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_TopEW_WestHalf_BottomCenter
                case 930: tileInd = 835; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_TopEW_WestHalf_BottomRight
                case 931: tileInd = 824; tileTopInd = 818; palRow = 2; miniCol = 4; break; //Commercial_TopEW_EastHalf_TopLeft
                case 932: tileInd = 825; tileTopInd = 819; palRow = 2; miniCol = 4; break; //Commercial_TopEW_EastHalf_TopCenter
                case 933: tileInd = 826; tileTopInd = 820; palRow = 2; miniCol = 4; break; //Commercial_TopEW_EastHalf_TopRight
                case 934: tileInd = 830; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_TopEW_EastHalf_CenterLeft
                case 935: tileInd = 831; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_TopEW_EastHalf_Center
                case 936: tileInd = 832; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_TopEW_EastHalf_CenterRight
                case 937: tileInd = 836; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_TopEW_EastHalf_BottomLeft
                case 938: tileInd = 837; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_TopEW_EastHalf_BottomCenter
                case 939: tileInd = 838; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_TopEW_EastHalf_BottomRight
                case 940: tileInd = 841; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_TopNS_NorthHalf_TopLeft
                case 941: tileInd = 842; tileTopInd = 839; palRow = 2; miniCol = 4; break; //Commercial_TopNS_NorthHalf_TopCenter
                case 942: tileInd = 843; tileTopInd = 840; palRow = 2; miniCol = 4; break; //Commercial_TopNS_NorthHalf_TopRight
                case 943: tileInd = 844; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_TopNS_NorthHalf_CenterLeft
                case 944: tileInd = 845; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_TopNS_NorthHalf_Center
                case 945: tileInd = 846; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_TopNS_NorthHalf_CenterRight
                case 946: tileInd = 847; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_TopNS_NorthHalf_BottomLeft
                case 947: tileInd = 848; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_TopNS_NorthHalf_BottomCenter
                case 948: tileInd = 849; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_TopNS_NorthHalf_BottomRight
                case 949: tileInd = 850; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_TopNS_SouthHalf_TopLeft
                case 950: tileInd = 851; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_TopNS_SouthHalf_TopCenter
                case 951: tileInd = 852; tileTopInd = 768; palRow = 2; miniCol = 4; break; //Commercial_TopNS_SouthHalf_TopRight
                case 952: tileInd = 853; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_TopNS_SouthHalf_CenterLeft
                case 953: tileInd = 854; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_TopNS_SouthHalf_Center
                case 954: tileInd = 855; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_TopNS_SouthHalf_CenterRight
                case 955: tileInd = 856; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_TopNS_SouthHalf_BottomLeft
                case 956: tileInd = 857; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_TopNS_SouthHalf_BottomCenter
                case 957: tileInd = 858; tileTopInd = 768; palRow = 2; miniCol = 12; break; //Commercial_TopNS_SouthHalf_BottomRight
                case 958: tileInd = 331; tileTopInd = 677; palRow = 0; miniCol = 11; break; //(Unused)
                case 959: tileInd = 331; tileTopInd = 880; palRow = 0; miniCol = 11; break; //(Unused)
                case 960: tileInd = 331; tileTopInd = 880; palRow = 0; miniCol = 11; break; //(Unused)
                case 961: tileInd = 331; tileTopInd = 880; palRow = 0; miniCol = 11; break; //(Unused)
                case 962: tileInd = 331; tileTopInd = 864; palRow = 0; miniCol = 11; break; //(Unused)
                case 963: tileInd = 331; tileTopInd = 865; palRow = 0; miniCol = 11; break; //(Unused)
                case 964: tileInd = 331; tileTopInd = 866; palRow = 0; miniCol = 11; break; //(Unused)
                case 965: tileInd = 331; tileTopInd = 867; palRow = 0; miniCol = 11; break; //(Unused)
                case 966: tileInd = 331; tileTopInd = 868; palRow = 0; miniCol = 11; break; //(Unused)
                case 967: tileInd = 331; tileTopInd = 869; palRow = 0; miniCol = 11; break; //(Unused)
                case 968: tileInd = 331; tileTopInd = 870; palRow = 0; miniCol = 11; break; //(Unused)
                case 969: tileInd = 331; tileTopInd = 871; palRow = 0; miniCol = 11; break; //(Unused)
                case 970: tileInd = 331; tileTopInd = 872; palRow = 0; miniCol = 11; break; //(Unused)
                case 971: tileInd = 331; tileTopInd = 873; palRow = 0; miniCol = 11; break; //(Unused)
                case 972: tileInd = 331; tileTopInd = 874; palRow = 0; miniCol = 11; break; //(Unused)
                case 973: tileInd = 331; tileTopInd = 875; palRow = 0; miniCol = 11; break; //(Unused)
                case 974: tileInd = 331; tileTopInd = 876; palRow = 0; miniCol = 11; break; //(Unused)
                case 975: tileInd = 331; tileTopInd = 877; palRow = 0; miniCol = 11; break; //(Unused)
                case 976: tileInd = 331; tileTopInd = 878; palRow = 0; miniCol = 11; break; //(Unused)
                case 977: tileInd = 331; tileTopInd = 879; palRow = 0; miniCol = 11; break; //(Unused)
                case 978: tileInd = 331; tileTopInd = 654; palRow = 0; miniCol = 11; break; //(Unused)
                case 979: tileInd = 331; tileTopInd = 655; palRow = 0; miniCol = 11; break; //(Unused)
                case 980: tileInd = 331; tileTopInd = 656; palRow = 0; miniCol = 11; break; //(Unused)
                case 981: tileInd = 331; tileTopInd = 657; palRow = 0; miniCol = 11; break; //(Unused)
                case 982: tileInd = 331; tileTopInd = 658; palRow = 0; miniCol = 11; break; //(Unused)
                case 983: tileInd = 331; tileTopInd = 659; palRow = 0; miniCol = 11; break; //(Unused)
                case 984: tileInd = 331; tileTopInd = 660; palRow = 0; miniCol = 11; break; //(Unused)
                case 985: tileInd = 331; tileTopInd = 661; palRow = 0; miniCol = 11; break; //(Unused)
                case 986: tileInd = 331; tileTopInd = 662; palRow = 0; miniCol = 11; break; //(Unused)
                case 987: tileInd = 331; tileTopInd = 663; palRow = 0; miniCol = 11; break; //(Unused)
                case 988: tileInd = 331; tileTopInd = 664; palRow = 0; miniCol = 11; break; //(Unused)
                case 989: tileInd = 331; tileTopInd = 665; palRow = 0; miniCol = 11; break; //(Unused)
                case 990: tileInd = 331; tileTopInd = 666; palRow = 0; miniCol = 11; break; //(Unused)
                case 991: tileInd = 331; tileTopInd = 667; palRow = 0; miniCol = 11; break; //(Unused)
                case 992: tileInd = 331; tileTopInd = 668; palRow = 0; miniCol = 11; break; //(Unused)
                case 993: tileInd = 331; tileTopInd = 669; palRow = 0; miniCol = 11; break; //(Unused)
                case 994: tileInd = 331; tileTopInd = 670; palRow = 0; miniCol = 11; break; //(Unused)
                case 995: tileInd = 331; tileTopInd = 671; palRow = 0; miniCol = 11; break; //(Unused)
                case 996: tileInd = 331; tileTopInd = 672; palRow = 0; miniCol = 11; break; //(Unused)
                case 997: tileInd = 331; tileTopInd = 673; palRow = 0; miniCol = 11; break; //(Unused)
                case 998: tileInd = 331; tileTopInd = 674; palRow = 0; miniCol = 11; break; //(Unused)
                case 999: tileInd = 331; tileTopInd = 675; palRow = 0; miniCol = 11; break; //(Unused)
                case 1000: tileInd = 331; tileTopInd = 678; palRow = 0; miniCol = 11; break; //(Unused)
                case 1001: tileInd = 331; tileTopInd = 679; palRow = 0; miniCol = 11; break; //(Unused)
                case 1002: tileInd = 331; tileTopInd = 680; palRow = 0; miniCol = 11; break; //(Unused)
                case 1003: tileInd = 331; tileTopInd = 681; palRow = 0; miniCol = 11; break; //(Unused)
                case 1004: tileInd = 331; tileTopInd = 15; palRow = 0; miniCol = 11; break; //(Unused)
                case 1005: tileInd = 331; tileTopInd = 15; palRow = 0; miniCol = 11; break; //(Unused)
                case 1006: tileInd = 331; tileTopInd = 698; palRow = 0; miniCol = 11; break; //(Unused)
                case 1007: tileInd = 331; tileTopInd = 699; palRow = 0; miniCol = 11; break; //(Unused)
                case 1008: tileInd = 331; tileTopInd = 700; palRow = 0; miniCol = 11; break; //(Unused)
                case 1009: tileInd = 331; tileTopInd = 701; palRow = 0; miniCol = 11; break; //(Unused)
                case 1010: tileInd = 331; tileTopInd = 702; palRow = 0; miniCol = 11; break; //(Unused)
                case 1011: tileInd = 331; tileTopInd = 703; palRow = 0; miniCol = 11; break; //(Unused)
                case 1012: tileInd = 331; tileTopInd = 704; palRow = 0; miniCol = 11; break; //(Unused)
                case 1013: tileInd = 331; tileTopInd = 705; palRow = 0; miniCol = 11; break; //(Unused)
                case 1014: tileInd = 331; tileTopInd = 706; palRow = 0; miniCol = 11; break; //(Unused)
                case 1015: tileInd = 331; tileTopInd = 707; palRow = 0; miniCol = 11; break; //(Unused)
                case 1016: tileInd = 331; tileTopInd = 708; palRow = 0; miniCol = 11; break; //(Unused)
                case 1017: tileInd = 331; tileTopInd = 709; palRow = 0; miniCol = 11; break; //(Unused)
                case 1018: tileInd = 331; tileTopInd = 710; palRow = 0; miniCol = 11; break; //(Unused)
                case 1019: tileInd = 331; tileTopInd = 711; palRow = 0; miniCol = 11; break; //(Unused)
                case 1020: tileInd = 331; tileTopInd = 712; palRow = 0; miniCol = 11; break; //(Unused)
                case 1021: tileInd = 331; tileTopInd = 15; palRow = 0; miniCol = 11; break; //(Unused)
                case 1022: tileInd = 331; tileTopInd = 887; palRow = 0; miniCol = 11; break; //(Unused)
                case 1023: tileInd = 331; tileTopInd = 888; palRow = 0; miniCol = 11; break; //(Unused)
                default: throw new ArgumentException(); //Should never happen.
            }
        }

        public static string getTileDescription(int tileType)
        {
            //Return description of tile type.

            //Based on "SimCity - Save RAM Guide - Super Nintendo - By aacroke": https://gamefaqs.gamespot.com/snes/588657-simcity/faqs/73744
            //I used an OCR to read the text in the images: http://www.structurise.com/screenshot-ocr/
            //Pretty good, but it still missed a lot that I had to fix manually.
            //Also there were some mistakes in aacroke's guide e.g. school and hospital was mixed up.
            //This list probably still contains a few errors so be aware.

            switch (tileType & 0x03FF) //Only tile type 0-1023 valid. 958-1023 is unused (garbage) though.
            {
                case 0: return "EmptyLand";
                case 1: return "Water_NoCoast_A";
                case 2: return "Water_NoCoast_B";
                case 3: return "Water_NoCoast_C";
                case 4: return "Water_CoastNW_A";
                case 5: return "Water_CoastN_A";
                case 6: return "Water_CoastNE_A";
                case 7: return "Water_CoastW_A";
                case 8: return "Water_CoastE_A";
                case 9: return "Water_CoastSW_A";
                case 10: return "Water_CoastS_A";
                case 11: return "Water_CoastSE_A";
                case 12: return "Water_CoastNW_B";
                case 13: return "Water_CoastN_B";
                case 14: return "Water_CoastNE_B";
                case 15: return "Water_CoastW_B";
                case 16: return "Water_CoastE_B";
                case 17: return "Water_CoastSW_B";
                case 18: return "Water_CoastS_B";
                case 19: return "Water_CoastSE_B";
                case 20: return "Forest_EdgeNW_A";
                case 21: return "Forest_EdgeN_A";
                case 22: return "Forest_EdgeNE_A";
                case 23: return "Forest_EdgeW_A";
                case 24: return "Forest_NoEdge_A";
                case 25: return "Forest_EdgeE_A";
                case 26: return "Forest_EdgeSW_A";
                case 27: return "Forest_EdgeS_A";
                case 28: return "Forest_EdgeSE_A";
                case 29: return "Forest_EdgeNW_B";
                case 30: return "Forest_EdgeN_B";
                case 31: return "Forest_EdgeNE_B";
                case 32: return "Forest_EdgeW_B";
                case 33: return "Forest_NoEdge_B";
                case 34: return "Forest_EdgeE_B";
                case 35: return "Forest_EdgeSW_B";
                case 36: return "Forest_EdgeS_B";
                case 37: return "Forest_EdgeSE_B";
                case 38: return "SmallPark_NoTree";
                case 39: return "SmallPark_Tree";
                case 40: return "Rubble_A";
                case 41: return "Rubble_B";
                case 42: return "Rubble_C";
                case 43: return "Rubble_D";
                case 44: return "Rubble_E";
                case 45: return "Rubble_F";
                case 46: return "(Unused)";
                case 47: return "(Unused)";
                case 48: return "Road_LineEW_Water";
                case 49: return "Road_LineNS_Water";
                case 50: return "Road_LineEW";
                case 51: return "Road_LineNS";
                case 52: return "Road_CornerSW";
                case 53: return "Road_CornerNW";
                case 54: return "Road_CornerNE";
                case 55: return "Road_CornerSE";
                case 56: return "Road_TeeS";
                case 57: return "Road_TeeW";
                case 58: return "Road_IeeN";
                case 59: return "Road_TeeE";
                case 60: return "Road_4WayCross";
                case 61: return "Road_LineEW_PowerLine";
                case 62: return "Road_LineNS_PowerLine";
                case 63: return "(Unused)";
                case 64: return "Road_LineEW_Water_LowTraffic";
                case 65: return "Road_LineNS_Water_LowTraffic";
                case 66: return "Road_LineEW_LowTraffic";
                case 67: return "Road_LineNS_LowTraffic";
                case 68: return "Road_CornerSW_LowTraFfic";
                case 69: return "Road_CornerNW_LowTraffic";
                case 70: return "Road_CornerNE_LowTraffic";
                case 71: return "Road_CornerSE_LowTraffic";
                case 72: return "Road_TeeS_LowTraffic";
                case 73: return "Road_TeeW_LowTraffic";
                case 74: return "Road_TeeN_LowTraffic";
                case 75: return "Road_TeeE_LowTraffic";
                case 76: return "Road_4WayCross_LowTraffic";
                case 77: return "Road_LineEW_PowerLine_LowTraffic";
                case 78: return "Road_LineNS_PowerLine_LowTraffic";
                case 79: return "(Unused)";
                case 80: return "Road_LineEW_Water_HighTraffic";
                case 81: return "Road_LineNS_Water_HighTraffic";
                case 82: return "Road_LineEW_HighTraffic";
                case 83: return "Road_LineNS_HighTraffic";
                case 84: return "Road_CornerSW_HighTraffic";
                case 85: return "Road_CornerNW_HighTraffic";
                case 86: return "Road_CornerNE_HighTraffic";
                case 87: return "Road_CornerSE_HighTraffic";
                case 88: return "Road_TeeS_HighTraffic";
                case 89: return "Road_TeeW_HighTraffic";
                case 90: return "Road_TeeN_HighTraffic";
                case 91: return "Road_TeeE_HighTraffic";
                case 92: return "Road_4WayCross_HighTraffic";
                case 93: return "Road_LineEW_PowerLine_HighTraffic";
                case 94: return "Road_LineNS_PowerLine_HighTraffic";
                case 95: return "(Unused)";
                case 96: return "PowerLine_LineEW_Water";
                case 97: return "PowerLine_LineNS_Water";
                case 98: return "PowerLine_LineEW";
                case 99: return "PowerLine_LineNS";
                case 100: return "PowerLine_CornerSW";
                case 101: return "PowerLine_CornerNW";
                case 102: return "PowerLine_CornerNE";
                case 103: return "PowerLine_CornerSE";
                case 104: return "PowerLine_TeeS";
                case 105: return "PowerLine_TeeW";
                case 106: return "PowerLine_TeeN";
                case 107: return "PowerLine_TeeE";
                case 108: return "PowerLine_4WayCross";
                case 109: return "PowerLine_LineNS_Rail";
                case 110: return "PowerLine_LineEW_Rail";
                case 111: return "(Unused)";
                case 112: return "Rail_LineEW_Water";
                case 113: return "Rail_LineNS_Water";
                case 114: return "Rail_LineEW";
                case 115: return "Rail_LineNS";
                case 116: return "Rail_CornerSW";
                case 117: return "Rail_CornerNW";
                case 118: return "Rail_CornerNE";
                case 119: return "Rail_CornerSE";
                case 120: return "Rail_TeeS";
                case 121: return "Rail_TeeW";
                case 122: return "Rail_TeeN";
                case 123: return "Rail_TeeE";
                case 124: return "Rail_4WayCross";
                case 125: return "Rail_LineEW_Road";
                case 126: return "Rail_LineNS_Road";
                case 127: return "Fire";
                case 128: return "Residential_Undeveloped_TopLeft";
                case 129: return "Residential_Undeveloped_TopCenter";
                case 130: return "Residential_Undeveloped_TopRight";
                case 131: return "Residential_Undeveloped_CenterLeft";
                case 132: return "Residential_Undeveloped_Center";
                case 133: return "Residential_Undeveloped_CenterRight";
                case 134: return "Residential_Undeveloped_BottomLeft";
                case 135: return "Residential_Undeveloped_BottomCenter";
                case 136: return "Residential_Undeveloped_BottomRight";
                case 137: return "Residential_Stage0LowHouse_A";
                case 138: return "Residential_Stage0LowHouse_B";
                case 139: return "Residential_Stage0LowHouse_C";
                case 140: return "Residential_Stage0MidHouse_A";
                case 141: return "Residential_Stage0MidHouse_B";
                case 142: return "Residential_Stage0MidHouse_C";
                case 143: return "Residential_Stage0UpperHouse_A";
                case 144: return "Residential_Stage0UpperHouse_B";
                case 145: return "Residential_Stage0UpperHouse_C";
                case 146: return "Residential_Stage0HighHouse_A";
                case 147: return "Residential_Stage0HighHouse_B";
                case 148: return "Residential_Stage0HighHouse_C";
                case 149: return "Residential_Stage1Low_TopLeft";
                case 150: return "Residential_Stage1Low_TopCenter";
                case 151: return "Residential_Stage1Low_TopRight";
                case 152: return "Residential_Stage1Low_CenterLeft";
                case 153: return "Residential_Stage1Low_Center";
                case 154: return "Residential_Stage1Low_CenterRight";
                case 155: return "Residential_Stage1Low_BottomLeft";
                case 156: return "Residential_Stage1Low_BottomCenter";
                case 157: return "Residential_Stage1Low_BottomRight";
                case 158: return "Residential_Stage2Low_TopLeft";
                case 159: return "Residential_Stage2Low_TopCenter";
                case 160: return "Residential_Stage2Low_TopRight";
                case 161: return "Residential_Stage2Low_CenterLeft";
                case 162: return "Residential_Stage2Low_Center";
                case 163: return "Residential_Stage2Low_CenterRight";
                case 164: return "Residential_Stage2Low_BottomLeft";
                case 165: return "Residential_Stage2Low_BottomCenter";
                case 166: return "Residential_Stage2Low_BottomRight";
                case 167: return "Residential_Stage3Low_TopLeft";
                case 168: return "Residential_Stage3Low_TopCenter";
                case 169: return "Residential_Stage3Low_TopRight";
                case 170: return "Residential_Stage3Low_CenterLeft";
                case 171: return "Residential_Stage3Low_Center";
                case 172: return "Residential_Stage3Low_CenterRight";
                case 173: return "Residential_Stage3Low_BottomLeft";
                case 174: return "Residential_Stage3Low_BottomCenter";
                case 175: return "Residential_Stage3Low_BottomRight";
                case 176: return "Residential_Stage4Low_TopLeft";
                case 177: return "Residential_Stage4Low_TopCenter";
                case 178: return "Residential_Stage4Low_TopRight";
                case 179: return "Residential_Stage4Low_CenterLeft";
                case 180: return "Residential_Stage4Low_Center";
                case 181: return "Residential_Stage4Low_CenterRight";
                case 182: return "Residential_Stage4Low_BottomLeft";
                case 183: return "Residential_Stage4Low_BottomCenter";
                case 184: return "Residential_Stage4Low_BottomRight";
                case 185: return "Residential_Stage1Mid_TopLeft";
                case 186: return "Residential_Stage1Mid_TopCenter";
                case 187: return "Residential_Stage1Mid_TopRight";
                case 188: return "Residential_Stage1Mid_CenterLeft";
                case 189: return "Residential_Stage1Mid_Center";
                case 190: return "Residential_Stage1Mid_CenterRight";
                case 191: return "Residential_Stage1Mid_BottomLeft";
                case 192: return "Residential_Stage1Mid_BottomCenter";
                case 193: return "Residential_Stage1Mid_BottomRight";
                case 194: return "Residential_Stage2Mid_TopLeft";
                case 195: return "Residential_Stage2Mid_TopCenter";
                case 196: return "Residential_Stage2Mid_TopRight";
                case 197: return "Residential_Stage2Mid_CenterLeft";
                case 198: return "Residential_Stage2Mid_Center";
                case 199: return "Residential_Stage2Mid_CenterRight";
                case 200: return "Residential_Stage2Mid_BottomLeft";
                case 201: return "Residential_Stage2Mid_BottomCenter";
                case 202: return "Residential_Stage2Mid_BottomRight";
                case 203: return "Residential_Stage3Mid_TopLeft";
                case 204: return "Residential_Stage3Mid_TopCenter";
                case 205: return "Residential_Stage3Mid_TopRight";
                case 206: return "Residential_Stage3Mid_CenterLeft";
                case 207: return "Residential_Stage3Mid_Center";
                case 208: return "Residential_Stage3Mid_CenterRight";
                case 209: return "Residential_Stage3Mid_BottomLeft";
                case 210: return "Residential_Stage3Mid_BottomCenter";
                case 211: return "Residential_Stage3Mid_BottomRight";
                case 212: return "Residential_Stage4Mid_TopLeft";
                case 213: return "Residential_Stage4Mid_TopCenter";
                case 214: return "Residential_Stage4Mid_TopRight";
                case 215: return "Residential_Stage4Mid_CenterLeft";
                case 216: return "Residential_Stage4Mid_Center";
                case 217: return "Residential_Stage4Mid_CenterRight";
                case 218: return "Residential_Stage4Mid_BottomLeft";
                case 219: return "Residential_Stage4Mid_BottomCenter";
                case 220: return "Residential_Stage4Mid_BottomRight";
                case 221: return "Residential_Stage1Upper_TopLeft";
                case 222: return "Residential_Stage1Upper_TopCenter";
                case 223: return "Residential_Stage1Upper_TopRight";
                case 224: return "Residential_Stage1Upper_CenterLeft";
                case 225: return "Residential_Stage1Upper_Center";
                case 226: return "Residential_Stage1Upper_CenterRight";
                case 227: return "Residential_Stage1Upper_BottomLeft";
                case 228: return "Residential_Stage1Upper_BottomCenter";
                case 229: return "Residential_Stage1Upper_BottomRight";
                case 230: return "Residential_Stage2Upper_TopLeft";
                case 231: return "Residential_Stage2Upper_TopCenter";
                case 232: return "Residential_Stage2Upper_TopRight";
                case 233: return "Residential_Stage2Upper_CenterLeft";
                case 234: return "Residential_Stage2Upper_Center";
                case 235: return "Residential_Stage2Upper_CenterRight";
                case 236: return "Residential_Stage2Upper_BottomLeft";
                case 237: return "Residential_Stage2Upper_BottomCenter";
                case 238: return "Residential_Stage2Upper_BottomRight";
                case 239: return "Residential_Stage3Upper_TopLeft";
                case 240: return "Residential_Stage3Upper_TopCenter";
                case 241: return "Residential_Stage3Upper_TopRight";
                case 242: return "Residential_Stage3Upper_CenterLeft";
                case 243: return "Residential_Stage3Upper_Center";
                case 244: return "Residential_Stage3Upper_CenterRight";
                case 245: return "Residential_Stage3Upper_BottomLeft";
                case 246: return "Residential_Stage3Upper_BottomCenter";
                case 247: return "Residential_Stage3Upper_BottomRight";
                case 248: return "Residential_Stage4Upper_TopLeft";
                case 249: return "Residential_Stage4Upper_TopCenter";
                case 250: return "Residential_Stage4Upper_TopRight";
                case 251: return "Residential_Stage4Upper_CenterLeft";
                case 252: return "Residential_Stage4Upper_Center";
                case 253: return "Residential_Stage4Upper_CenterRight";
                case 254: return "Residential_Stage4Upper_BottomLeft";
                case 255: return "Residential_Stage4Upper_BottomCenter";
                case 256: return "Residential_Stage4Upper_BottomRight";
                case 257: return "Residential_Stage1High_TopLeft";
                case 258: return "Residential_Stage1High_TopCenter";
                case 259: return "Residential_Stage1High_TopRight";
                case 260: return "Residential_Stage1High_CenterLeft";
                case 261: return "Residential_Stage1High_Center";
                case 262: return "Residential_Stage1High_CenterRight";
                case 263: return "Residential_Stage1High_BottomLeft";
                case 264: return "Residential_Stage1High_BottomCenter";
                case 265: return "Residential_Stage1High_BottomRight";
                case 266: return "Residential_Stage2High_TopLeft";
                case 267: return "Residential_Stage2High_TopCenter";
                case 268: return "Residential_Stage2High_TopRight";
                case 269: return "Residential_Stage2High_CenterLeft";
                case 270: return "Residential_Stage2High_Center";
                case 271: return "Residential_Stage2High_CenterRight";
                case 272: return "Residential_Stage2High_BottomLeft";
                case 273: return "Residential_Stage2High_BottomCenter";
                case 274: return "Residential_Stage2High_BottomRight";
                case 275: return "Residential_Stage3High_TopLeft";
                case 276: return "Residential_Stage3High_TopCenter";
                case 277: return "Residential_Stage3High_TopRight";
                case 278: return "Residential_Stage3High_CenterLeft";
                case 279: return "Residential_Stage3High_Center";
                case 280: return "Residential_Stage3High_CenterRight";
                case 281: return "Residential_Stage3High_BottomLeft";
                case 282: return "Residential_Stage3High_BottomCenter";
                case 283: return "Residential_Stage3High_BottomRight";
                case 284: return "Residential_Stage4High_TopLeft";
                case 285: return "Residential_Stage4High_TopCenter";
                case 286: return "Residential_Stage4High_TopRight";
                case 287: return "Residential_Stage4High_CenterLeft";
                case 288: return "Residential_Stage4High_Center";
                case 289: return "Residential_Stage4High_CenterRight";
                case 290: return "Residential_Stage4High_BottomLeft";
                case 291: return "Residential_Stage4High_BottomCenter";
                case 292: return "Residential_Stage4High_BottomRight";
                case 293: return "Residential_Hospital_TopLeft";
                case 294: return "Residential_Hospital_TopCenter";
                case 295: return "Residential_Hospital_TopRight";
                case 296: return "Residential_Hospital_CenterLeft";
                case 297: return "Residential_Hospital_Center";
                case 298: return "Residential_Hospital_CenterRight";
                case 299: return "Residential_Hospital_BottomLeft";
                case 300: return "Residential_Hospital_BottomCenter";
                case 301: return "Residential_Hospital_BottomRight";
                case 302: return "Residential_School_TopLeft";
                case 303: return "Residential_School_TopCenter";
                case 304: return "Residential_School_TopRight";
                case 305: return "Residential_School_CenterLeft";
                case 306: return "Residential_School_Center";
                case 307: return "Residential_School_CenterRight";
                case 308: return "Residential_School_BottomLeft";
                case 309: return "Residential_School_BottomCenter";
                case 310: return "Residential_School_BottomRight";
                case 311: return "Commercial_Undeveloped_TopLeft";
                case 312: return "Commercial_Undeveloped_TopCenter";
                case 313: return "Commercial_Undeveloped_TopRight";
                case 314: return "Commercial_Undeveloped_CenterLeft";
                case 315: return "Commercial_Undeveloped_Center";
                case 316: return "Commercial_Undeveloped_CenterRight";
                case 317: return "Commercial_Undeveloped_BottomLeft";
                case 318: return "Commercial_Undeveloped_BottomCenter";
                case 319: return "Commercial_Undeveloped_BottomRight";
                case 320: return "Commercial_Stage1Low_TopLeft";
                case 321: return "Commercial_Stage1Low_TopCenter";
                case 322: return "Commercial_Stage1Low_TopRight";
                case 323: return "Commercial_Stage1Low_CenterLeft";
                case 324: return "Commercial_Stage1Low_Center";
                case 325: return "Commercial_Stage1Low_CenterRight";
                case 326: return "Commercial_Stage1Low_BottomLeft";
                case 327: return "Commercial_Stage1Low_BottomCenter";
                case 328: return "Commercial_Stage1Low_BottomRight";
                case 329: return "Commercial_Stage2Low_TopLeft";
                case 330: return "Commercial_Stage2Low_TopCenter";
                case 331: return "Commercial_Stage2Low_TopRight";
                case 332: return "Commercial_Stage2Low_CenterLeft";
                case 333: return "Commercial_Stage2Low_Center";
                case 334: return "Commercial_Stage2Low_CenterRight";
                case 335: return "Commercial_Stage2Low_BottomLeft";
                case 336: return "Commercial_Stage2Low_BottomCenter";
                case 337: return "Commercial_Stage2Low_BottomRight";
                case 338: return "Commercial_Stage3Low_TopLeft";
                case 339: return "Commercial_Stage3Low_TopCenter";
                case 340: return "Commercial_Stage3Low_TopRight";
                case 341: return "Commercial_Stage3Low_CenterLeft";
                case 342: return "Commercial_Stage3Low_Center";
                case 343: return "Commercial_Stage3Low_CenterRight";
                case 344: return "Commercial_Stage3Low_BottomLeft";
                case 345: return "Commercial_Stage3Low_BottomCenter";
                case 346: return "Commercial_Stage3Low_BottomRight";
                case 347: return "Commercial_Stage4Low_TopLeft";
                case 348: return "Commercial_Stage4Low_TopCenter";
                case 349: return "Commercial_Stage4Low_TopRight";
                case 350: return "Commercial_Stage4Low_CenterLeft";
                case 351: return "Commercial_Stage4Low_Center";
                case 352: return "Commercial_Stage4Low_CenterRight";
                case 353: return "Commercial_Stage4Low_BottomLeft";
                case 354: return "Commercial_Stage4Low_BottomCenter";
                case 355: return "Commercial_Stage4Low_BottomRight";
                case 356: return "Commercial_Stage5Low_TopLeft";
                case 357: return "Commercial_Stage5Low_TopCenter";
                case 358: return "Commercial_Stage5Low_TopRight";
                case 359: return "Commercial_Stage5Low_CenterLeft";
                case 360: return "Commercial_Stage5Low_Center";
                case 361: return "Commercial_Stage5Low_CenterRight";
                case 362: return "Commercial_Stage5Low_BottomLeft";
                case 363: return "Commercial_Stage5Low_BottomCenter";
                case 364: return "Commercial_Stage5Low_BottomRight";
                case 365: return "Commercial_Stage1Mid_TopLeft";
                case 366: return "Commercial_Stage1Mid_TopCenter";
                case 367: return "Commercial_Stage1Mid_TopRight";
                case 368: return "Commercial_Stage1Mid_CenterLeft";
                case 369: return "Commercial_Stage1Mid_Center";
                case 370: return "Commercial_Stage1Mid_CenterRight";
                case 371: return "Commercial_Stage1Mid_BottomLeft";
                case 372: return "Commercial_Stage1Mid_BottomCenter";
                case 373: return "Commercial_Stage1Mid_BottomRight";
                case 374: return "Commercial_Stage2Mid_TopLeft";
                case 375: return "Commercial_Stage2Mid_TopCenter";
                case 376: return "Commercial_Stage2Mid_TopRight";
                case 377: return "Commercial_Stage2Mid_CenterLeft";
                case 378: return "Commercial_Stage2Mid_Center";
                case 379: return "Commercial_Stage2Mid_CenterRight";
                case 380: return "Commercial_Stage2Mid_BottomLeft";
                case 381: return "Commercial_Stage2Mid_BottomCenter";
                case 382: return "Commercial_Stage2Mid_BottomRight";
                case 383: return "Commercial_Stage3Mid_TopLeft";
                case 384: return "Commercial_Stage3Mid_TopCenter";
                case 385: return "Commercial_Stage3Mid_TopRight";
                case 386: return "Commercial_Stage3Mid_CenterLeft";
                case 387: return "Commerclal_Stage3Mid_Center";
                case 388: return "Commercial_Stage3Mid_CenterRight";
                case 389: return "Commercial_Stage3Mid_BottomLeft";
                case 390: return "Commercial_Stage3Mid_BottomCenter";
                case 391: return "Commercial_Stage3Mid_BottomRight";
                case 392: return "Commercial_Stage4Mid_TopLeft";
                case 393: return "Commercial_Stage4Mid_TopCenter";
                case 394: return "Commercial_Stage4Mid_TopRight";
                case 395: return "Commercial_Stage4Mid_CenterLeft";
                case 396: return "Commercial_Stage4Mid_Center";
                case 397: return "Commercial_Stage4Mid_CenterRight";
                case 398: return "Commercial_Stage4Mid_BottomLeft";
                case 399: return "Commercial_Stage4Mid_BottomCenter";
                case 400: return "Commercial_Stage4Mid_BottomRight";
                case 401: return "Commercial_Stage5Mid_TopLeft";
                case 402: return "Commercial_Stage5Mid_TopCenter";
                case 403: return "Commercial_Stage5Mid_TopRight";
                case 404: return "Commercial_Stage5Mid_CenterLeft";
                case 405: return "Commercial_Stage5Mid_Center";
                case 406: return "Commercial_Stage5Mid_CenterRight";
                case 407: return "Commerclal_Stage5Mid_BottomLeft";
                case 408: return "Commercial_Stage5Mid_BottomCenter";
                case 409: return "Commercial_Stage5Mid_BottomRight";
                case 410: return "Commercial_Stage1Upper_TopLeft";
                case 411: return "Commercial_Stage1Upper_TopCenter";
                case 412: return "Commercial_Stage1Upper_TopRight";
                case 413: return "Commercial_Stage1Upper_CenterLeft";
                case 414: return "Commercial_Stage1Upper_Center";
                case 415: return "Commerclal_Stage1Upper_CenterRight";
                case 416: return "Commercial_Stage1Upper_BottomLeft";
                case 417: return "Commercial_Stage1Upper_BottomCenter";
                case 418: return "Commercial_Stage1Upper_BottomRight";
                case 419: return "Commercial_Stage2Upper_TopLeft";
                case 420: return "Commercial_Stage2Upper_TopCenter";
                case 421: return "Commercial_Stage2Upper_TopRight";
                case 422: return "Commercial_Stage2Upper_CenterLeft";
                case 423: return "Commercial_Stage2Upper_Center";
                case 424: return "Commercial_Stage2Upper_CenterRight";
                case 425: return "Commercial_Stage2Upper_BottomLeft";
                case 426: return "Commercial_Stage2Upper_BottomCenter";
                case 427: return "Commercial_Stage2Upper_BottomRight";
                case 428: return "Commercial_Stage3Upper_TopLeft";
                case 429: return "Commercial_Stage3Upper_TopCenter";
                case 430: return "Commercial_Stage3Upper_TopRight";
                case 431: return "Commercial_Stage3Upper_CenterLeft";
                case 432: return "Commercial_Stage3Upper_Center";
                case 433: return "Commercial_Stage3Upper_CenterRight";
                case 434: return "Commercial_Stage3Upper_BottomLeft";
                case 435: return "Commercial_Stage3Upper_BottomCenter";
                case 436: return "Commercial_Stage3Upper_BottomRight";
                case 437: return "Commercial_Stage4Upper_TopLeft";
                case 438: return "Commercial_Stage4Upper_TopCenter";
                case 439: return "Commercial_Stage4Upper_TopRight";
                case 440: return "Commercial_Stage4Upper_CenterLeft";
                case 441: return "Commercial_Stage4Upper_Center";
                case 442: return "Commercial_Stage4Upper_CenterRight";
                case 443: return "Commercial_Stage4Upper_BottomLeft";
                case 444: return "Commercial_Stage4Upper_BottomCenter";
                case 445: return "Commercial_Stage4Upper_BottomRight";
                case 446: return "Commercial_Stage5Upper_TopLeft";
                case 447: return "Commercial_Stage5Upper_TopCenter";
                case 448: return "Commercial_Stage5Upper_TopRight";
                case 449: return "Commercial_Stage5Upper_CenterLeft";
                case 450: return "Commercial_Stage5Upper_Center";
                case 451: return "Commercial_Stage5Upper_CenterRight";
                case 452: return "Commercial_Stage5Upper_BottomLeft";
                case 453: return "Commercial_StageSUpper_BottomCenter";
                case 454: return "Commercial_Stage5Upper_BottomRight";
                case 455: return "Commercial_Stage1High_TopLeft";
                case 456: return "Commercial_Stage1High_TopCenter";
                case 457: return "Commercial_Stage1High_TopRight";
                case 458: return "Commercial_Stage1High_CenterLeft";
                case 459: return "Commercial_Stage1High_Center";
                case 460: return "Commercial_Stage1High_CenterRight";
                case 461: return "Commercial_Stage1High_BottomLeft";
                case 462: return "Commercial_Stage1High_BottomCenter";
                case 463: return "Commercial_Stage1High_BottomRight";
                case 464: return "Commercial_Stage2High_TopLeft";
                case 465: return "Commercial_Stage2High_TopCenter";
                case 466: return "Commercial_Stage2High_TopRight";
                case 467: return "Commercial_Stage2High_CenterLeft";
                case 468: return "Commercial_Stage2High_Center";
                case 469: return "Commercial_Stage2High_CenterRight";
                case 470: return "Commercial_Stage2High_BottomLeft";
                case 471: return "Commercial_Stage2High_BottomCenter";
                case 472: return "Commercial_Stage2High_BottomRight";
                case 473: return "Commercial_Stage3High_TopLeft";
                case 474: return "Commercial_Stage3High_TopCenter";
                case 475: return "Commercial_Stage3High_TopRight";
                case 476: return "Commercial_Stage3High_CenterLeft";
                case 477: return "Commercial_Stage3High_Center";
                case 478: return "Commercial_Stage3High_CenterRight";
                case 479: return "Commercial_Stage3High_BottomLeft";
                case 480: return "Commercial_Stage3High_BottomCenter";
                case 481: return "Commercial_Stage3High_BottomRight";
                case 482: return "Commercial_Stage4High_TopLeft";
                case 483: return "Commercial_Stage4High_TopCenter";
                case 484: return "Commercial_Stage4High_TopRight";
                case 485: return "Commercial_Stage4High_CenterLeft";
                case 486: return "Commercial_Stage4High_Center";
                case 487: return "Commercial_Stage4High_CenterRight";
                case 488: return "Commercial_Stage4High_BottomLeft";
                case 489: return "Commercial_Stage4High_BottomCenter";
                case 490: return "Commercial_Stage4High_BottomRight";
                case 491: return "Commercial_Stage5High_TopLeft";
                case 492: return "Commercial_Stage5High_TopCenter";
                case 493: return "Commercial_Stage5High_TopRight";
                case 494: return "Commercial_Stage5High_CenterLeft";
                case 495: return "Commercial_Stage5High_Center";
                case 496: return "Commercial_Stage5High_CenterRight";
                case 497: return "Commercial_Stage5High_BottomLeft";
                case 498: return "Commercial_Stage5High_BottomCenter";
                case 499: return "Commercial_Stage5High_BottomRight";
                case 500: return "Industrial_Undeveloped_TopLeft";
                case 501: return "Industrial_Undeveloped_TopCenter";
                case 502: return "Industrial_Undeveloped_TopRight";
                case 503: return "Industrial_Undeveloped_CenterLeft";
                case 504: return "Industrial_Undeveloped_Center";
                case 505: return "Industrial_Undeveloped_CenterRight";
                case 506: return "Industrial_Undeveloped_BottomLeft";
                case 507: return "Industrial_Undeveloped_BottomCenter";
                case 508: return "Industrial_Undeveloped_BottomRight";
                case 509: return "Industrial_Stage1Low_TopLeft";
                case 510: return "Industrial_Stage1Low_TopCenter";
                case 511: return "Industrial_Stage1Low_TopRight";
                case 512: return "Industrial_Stage1Low_CenterLeft";
                case 513: return "Industrial_Stage1Low_Center";
                case 514: return "Industrial_Stage1Low_CenterRight";
                case 515: return "Industrial_Stage1Low_BottomLeft";
                case 516: return "Industrial_Stage1Low_BottomCenter";
                case 517: return "Industrial_Stage1Low_BottomRight";
                case 518: return "Industrial_Stage2Low_TopLeft";
                case 519: return "Industrial_Stage2Low_TopCenter";
                case 520: return "Industrial_Stage2Low_TopRight";
                case 521: return "Industrial_Stage2Low_CenterLeft";
                case 522: return "Industrial_Stage2Low_Center";
                case 523: return "Industrial_Stage2Low_CenterRight";
                case 524: return "Industrial_Stage2Low_BottomLeft";
                case 525: return "Industrial_Stage2Low_BottomCenter";
                case 526: return "Industrial_Stage2Low_BottomRight";
                case 527: return "Industrial_Stage3Low_TopLeft";
                case 528: return "Industrial_Stage3Low_TopCenter";
                case 529: return "Industrial_Stage3Low_TopRight";
                case 530: return "Industrial_Stage3Low_CenterLeft";
                case 531: return "Industrial_Stage3Low_Center";
                case 532: return "Industrial_Stage3Low_CenterRight";
                case 533: return "Industrial_Stage3Low_BottomLeft";
                case 534: return "Industrial_Stage3Low_BottomCenter";
                case 535: return "Industrial_Stage3Low_BottomRight";
                case 536: return "Industrial_Stage4Low_TopLeft";
                case 537: return "Industrial_Stage4Low_TopCenter";
                case 538: return "Industrial_Stage4Low_TopRight";
                case 539: return "Industrial_Stage4Low_CenterLeft";
                case 540: return "Industrial_Stage4Low_Center";
                case 541: return "Industrial_Stage4Low_CenterRight";
                case 542: return "Industrial_Stage4Low_BottomLeft";
                case 543: return "Industrial_Stage4Low_BottomCenter";
                case 544: return "Industrial_Stage4Low_BottomRight";
                case 545: return "Industrial_Stage1High_TopLeft";
                case 546: return "Industrial_Stage1High_TopCenter";
                case 547: return "Industrial_Stage1High_TopRight";
                case 548: return "Industrial_Stage1High_CenterLeft";
                case 549: return "Industrial_Stage1High_Center";
                case 550: return "Industrial_Stage1High_CenterRight";
                case 551: return "Industrial_Stage1High_BottomLeft";
                case 552: return "Industrial_Stage1High_BottomCenter";
                case 553: return "Industrial_Stage1High_BottomRight";
                case 554: return "Industrial_Stage2High_TopLeft";
                case 555: return "Industrial_Stage2High_TopCenter";
                case 556: return "Industrial_Stage2High_TopRight";
                case 557: return "Industrial_Stage2High_CenterLeft";
                case 558: return "Industrial_Stage2High_Center";
                case 559: return "Industrial_Stage2High_CenterRight";
                case 560: return "Industrial_Stage2High_BottomLeft";
                case 561: return "Industrial_Stage2High_BottomCenter";
                case 562: return "Industrial_Stage2High_BottomRight";
                case 563: return "Industrial_Stage3High_TopLeft";
                case 564: return "Industrial_Stage3High_TopCenter";
                case 565: return "Industrial_Stage3High_TopRight";
                case 566: return "Industrial_Stage3High_CenterLeft";
                case 567: return "Industrial_Stage3High_Center";
                case 568: return "Industrial_Stage3High_CenterRight";
                case 569: return "Industrial_Stage3High_BottomLeft";
                case 570: return "Industrial_Stage3High_BottomCenter";
                case 571: return "Industrial_Stage3High_BottomRight";
                case 572: return "Industrial_Stage4High_TopLeft";
                case 573: return "Industrial_Stage4High_TopCenter";
                case 574: return "Industrial_Stage4High_TopRight";
                case 575: return "Industrial_Stage4High_CenterLeft";
                case 576: return "Industrial_Stage4High_Center";
                case 577: return "Industrial_Stage4High_CenterRight";
                case 578: return "Industrial_Stage4High_BottomLeft";
                case 579: return "Industrial_Stage4High_BottomCenter";
                case 580: return "Industrial_Stage4High_BottomRight";
                case 581: return "PoliceStation_TopLeft";
                case 582: return "PoliceStation_TopCenter";
                case 583: return "PoliceStation_TopRight";
                case 584: return "PoliceStation_CenterLeft";
                case 585: return "PoliceStation_Center";
                case 586: return "PoliceStation_CenterRight";
                case 587: return "PoliceStation_BottomLeft";
                case 588: return "PoliceStation_BottomCenter";
                case 589: return "PoliceStation_BottomRight";
                case 590: return "FireDepartment_TopLeft";
                case 591: return "FireDepartment_TopCenter";
                case 592: return "FireDepartment_TopRight";
                case 593: return "FireDepartment_CenterLeft";
                case 594: return "FireDepartment_Center";
                case 595: return "FireDepartment_CenterRight";
                case 596: return "FireDepartment_BottomLeft";
                case 597: return "FireDepartment_BottomCenter";
                case 598: return "FireDepartment_BottomRight";
                case 599: return "StadiumOpen_TopLeft";
                case 600: return "StadiumOpen_TopCenterL";
                case 601: return "StadiumOpen_TopCenterR";
                case 602: return "StadiumOpen_TopRight";
                case 603: return "StadiumOpen_UpperLeft";
                case 604: return "StadiumOpen_UpperCenterL";
                case 605: return "StadiumOpen_UpperCenterR";
                case 606: return "StadimnOpen_UpperRight";
                case 607: return "StadiumOpen_LowerLeft";
                case 608: return "StadiumOpen_LowerCenterL";
                case 609: return "StadiumOpen_LowerCenterR";
                case 610: return "StadiumOpen_LowerRight";
                case 611: return "StadiumOpen_BottomLeft";
                case 612: return "StadiumOpen_BottomCenterL";
                case 613: return "StadiumOpen_BottomCenterR";
                case 614: return "StadiumOpen_BottomRight";
                case 615: return "Seaport_TopLeft";
                case 616: return "Seaport_TopCenterL";
                case 617: return "Seaport_TopCenterR";
                case 618: return "Seaport_TopRight";
                case 619: return "Seaport_UpperLeft";
                case 620: return "Seaport_UpperCenterL";
                case 621: return "Seaport_UpperCenterR";
                case 622: return "Seaport_UpperRight";
                case 623: return "Seaport_LowerLeft";
                case 624: return "Seaport_LowerCenterL";
                case 625: return "Seaport_LowerCenterR";
                case 626: return "Seaport_LowerRight";
                case 627: return "Seaport_BottomLeft";
                case 628: return "Seaport_BottomCenterL";
                case 629: return "Seaport_BottomCenterR";
                case 630: return "Seaport_BottomRight";
                case 631: return "NuclearPlant_TopLeft";
                case 632: return "NuclearPlant_TopCenterL";
                case 633: return "NuclearPlant_TopCenterR";
                case 634: return "NuclearPlant_TopRight";
                case 635: return "NuclearPlant_UpperLeft";
                case 636: return "NuclearPlant_UpperCenterL";
                case 637: return "NuclearPlant_UpperCenterR";
                case 638: return "NuclearPlant_UpperRight";
                case 639: return "NuclearPlant_LowerLeft";
                case 640: return "NuclearPlant_LowerCenterL";
                case 641: return "NuclearPlant_LowerCenterR";
                case 642: return "NuclearPlant_LowerRight";
                case 643: return "NuclearPlant_BottomLeft";
                case 644: return "NuclearPlant_BottomCenterL";
                case 645: return "NuclearPlant_BottomCenterR";
                case 646: return "NuclearPlant_BottomRight";
                case 647: return "CoalPlant_TopLeft";
                case 648: return "CoalPlant_TopCenterL";
                case 649: return "CoalPlant_TopCenterR";
                case 650: return "CoalPlant_TopRight";
                case 651: return "CoalPlant_UpperLeft";
                case 652: return "CoalPlant_UpperCenterL";
                case 653: return "CoalPlant_UpperCenterR";
                case 654: return "CoalPlant_UpperRight";
                case 655: return "CoalPlant_LowerLeft";
                case 656: return "CoalPlant_LowerCenterL";
                case 657: return "CoalPlant_LowerCenterR";
                case 658: return "CoalPlant_LowerRight";
                case 659: return "CoalPlant_BottomLeft";
                case 660: return "CoalPlant_BottomCenterL";
                case 661: return "CoalPlant_BottomCenterR";
                case 662: return "CoalPlant_BottomRight";
                case 663: return "Airport_TopLeft";
                case 664: return "Airport_TopRow2";
                case 665: return "Airport_TopRow3";
                case 666: return "Airport_TopRow4";
                case 667: return "Airport_TopRow5";
                case 668: return "Airport_TopRight";
                case 669: return "Airport_2ndRowLeft";
                case 670: return "Airport_2ndRow2";
                case 671: return "Airport_2ndRow3";
                case 672: return "Airport_2ndRow4";
                case 673: return "Airport_2ndRow5";
                case 674: return "Airport_2ndRowRight";
                case 675: return "Airport_3rdRowLeft";
                case 676: return "Airport_3rdRow2";
                case 677: return "Airport_3rdRow3";
                case 678: return "Airport_3rdRow4";
                case 679: return "Airport_3rdRow5";
                case 680: return "Airport_3rdRowRight";
                case 681: return "Airport_4thRowLeft";
                case 682: return "Airport_4thRow2";
                case 683: return "Airport_4thRow3";
                case 684: return "Airport_4thRow4";
                case 685: return "Airport_4thRow5";
                case 686: return "Airport_4thRowRight";
                case 687: return "Airport_5thRowLeft";
                case 688: return "Airport_5thRow2";
                case 689: return "Airport_5thRow3";
                case 690: return "Airport_5thRow4";
                case 691: return "Airport_5thRow5";
                case 692: return "Airport_5thRowRight";
                case 693: return "Airport_BottomLeft";
                case 694: return "Airport_BottomRow2";
                case 695: return "Airport_BottomRow3";
                case 696: return "Airport_BottomRow4";
                case 697: return "Airport_BottomRow5";
                case 698: return "Airport_BottomRight";
                case 699: return "MayorsHouse_Stage1_TopLeft";
                case 700: return "MayorsHouse_Stage1_TopCenter";
                case 701: return "MayorsHouse_Stage1_TopRight";
                case 702: return "MayorsHouse_Stage1_CenterLeft";
                case 703: return "HayarsHouse_Stage1_Center";
                case 704: return "MayorsHouse_Stage1_CenterRight";
                case 705: return "MayorsHouse_Stage1_BottomLeft";
                case 706: return "MayorsHouse_Stage1_BottomCenter";
                case 707: return "MayorsHouse_Stage1_BottomRight";
                case 708: return "MayorsHouse_Stage2_TopLeft";
                case 709: return "MayorsHouse_Stage2_TopCenter";
                case 710: return "MayorsHouse_Stage2_TopRight";
                case 711: return "MayorsHouse_Stage2_CenterLeft";
                case 712: return "MayorsHouse_Stage2_Center";
                case 713: return "MayorsHouse_Stage2_CenterRight";
                case 714: return "MayorsHouse_Stage2_BottomLeft";
                case 715: return "MayorsHouse_Stage2_BottomCenter";
                case 716: return "MayorsHouse_Stage2_BottomRight";
                case 717: return "MayorsHouse_Stage3_TopLeft";
                case 718: return "MayorsHouse_Stage3_TopCenter";
                case 719: return "MayorsHouse_Stage3_TopRight";
                case 720: return "MayorsHouse_Stage3_CenterLeft";
                case 721: return "MayorsHouse_Stage3_Center";
                case 722: return "MayorsHouse_Stage3_CenterRight";
                case 723: return "MayorsHouse_Stage3_BottomLeft";
                case 724: return "MayorsHouse_Stage3_BottomCenter";
                case 725: return "MayorsHouse_Stage3_BottomRight";
                case 726: return "MayorsHouse_Stage4_TopLeft";
                case 727: return "MayorsHouse_Stage4_TopCenter";
                case 728: return "MayorsHouse_Stage4_TopRight";
                case 729: return "MayorsHouse_Stage4_CenterLeft";
                case 730: return "MayorsHouse_Stage4_Center";
                case 731: return "MayorsHouse_Stage4_CenterRight";
                case 732: return "MayorsHouse_Stage4_BottomLeft";
                case 733: return "MayorsHouse_Stage4_BottomCenter";
                case 734: return "MayorsHouse_Stage4_BottomRight";
                case 735: return "Bank_TopLeft";
                case 736: return "Bank_TopCenter";
                case 737: return "Bank_TopRight";
                case 738: return "Bank_CenterLeft";
                case 739: return "Bank_Center";
                case 740: return "Bank_CenterRight";
                case 741: return "Bank_BottomLeft";
                case 742: return "Bank_BottomCenter";
                case 743: return "Bank_BottomRight";
                case 744: return "AmusementPark_TopLeft";
                case 745: return "AmusementPark_TopCenter";
                case 746: return "AmusementPark_TopRight";
                case 747: return "AmusementPark_CenterLeft";
                case 748: return "AmusementPark_Center";
                case 749: return "AmusementPark_CenterRight";
                case 750: return "AmusementPark_BottomLeft";
                case 751: return "AmusementPark_BottomCenter";
                case 752: return "AmusementPark_BottomRight";
                case 753: return "Zoo_TopLeft";
                case 754: return "Zoo_TopCenter";
                case 755: return "Zoo_TopRight";
                case 756: return "Zoo_CenterLeft";
                case 757: return "Zoo_Center";
                case 758: return "Zoo_CenterRight";
                case 759: return "Zoo_BottomLeft";
                case 760: return "Zoo_BottomCenter";
                case 761: return "Zoo_BottomRight";
                case 762: return "Casino_TopLeft";
                case 763: return "Casino_TopCenter";
                case 764: return "Casino_TopRight";
                case 765: return "Casino_CenterLeft";
                case 766: return "Casino_Center";
                case 767: return "Casino_CenterRight";
                case 768: return "Casino_BottomLeft";
                case 769: return "Casino_BottomCenter";
                case 770: return "Casino_BottomRight";
                case 771: return "PoliceHQ_TopLeft";
                case 772: return "PoliceHQ_TopCenter";
                case 773: return "PoliceHQ_TopRight";
                case 774: return "PoliceHQ_CenterLeft";
                case 775: return "PoliceHQ_Center";
                case 776: return "PoliceHQ_CenterRight";
                case 777: return "PoliceHQ_BottomLeft";
                case 778: return "PoliceHQ_BottomCenter";
                case 779: return "PoliceHQ_BottomRight";
                case 780: return "FireHQ_TopLeft";
                case 781: return "FireHQ_TopCenter";
                case 782: return "FireHQ_TopRight";
                case 783: return "FireHQ_CenterLeft";
                case 784: return "FireHQ_Center";
                case 785: return "FireHQ_CenterRight";
                case 786: return "FireHQ_BottomLeft";
                case 787: return "FireHQ_BottomCenter";
                case 788: return "FireHQ_BottomRight";
                case 789: return "Fountain_TopLeft";
                case 790: return "Fountain_TopCenter";
                case 791: return "Fountain_TopRight";
                case 792: return "Fountain_CenterLeft";
                case 793: return "Fountain_Center";
                case 794: return "Fountain_CenterRight";
                case 795: return "Fountain_BottomLeft";
                case 796: return "Fountain_BottomCenter";
                case 797: return "Fountain_BottomRight";
                case 798: return "MarioStatue_TopLeft";
                case 799: return "MarioStatue_TopCenter";
                case 800: return "MarioStatue_TopRight";
                case 801: return "MarioStatue_CenterLeft";
                case 802: return "MarioStatue_Center";
                case 803: return "MarioStatue_CenterRight";
                case 804: return "MarioStatue_BottomLeft";
                case 805: return "MarioStatue_BottomCenter";
                case 806: return "MarioStatue_BottomRight";
                case 807: return "Expo_TopLeft";
                case 808: return "Expo_TopCenter";
                case 809: return "Expo_TopRight";
                case 810: return "Expo_CenterLeft";
                case 811: return "Expo_Center";
                case 812: return "Expo_CenterRight";
                case 813: return "Expo_BottomLeft";
                case 814: return "Expo_BottomCenter";
                case 815: return "Expo_BottomRight";
                case 816: return "Windmill_TopLeft";
                case 817: return "Windmill_TopCenter";
                case 818: return "Windmill_TopRight";
                case 819: return "Windmill_CenterLeft";
                case 820: return "Windmill_Center";
                case 821: return "Windmill_CenterRight";
                case 822: return "Windmill_BottomLeft";
                case 823: return "Windmill_BottomCenter";
                case 824: return "Windmill_BottomRight";
                case 825: return "Library_TopLeft";
                case 826: return "Library_TopCenter";
                case 827: return "Library_TopRight";
                case 828: return "Library_CenterLeft";
                case 829: return "Library_Center";
                case 830: return "Library_CenterRight";
                case 831: return "Library_BottomLeft";
                case 832: return "Library_BottomCenter";
                case 833: return "Library_BottomRight";
                case 834: return "LargePark_TopLeft";
                case 835: return "LargePark_TopCenter";
                case 836: return "LargePark_TopRight";
                case 837: return "LargePark_CenterLeft";
                case 838: return "LargePark_Center";
                case 839: return "LargePark_CenterRight";
                case 840: return "LargePark_BottomLeft";
                case 841: return "LargePark_BottomCenter";
                case 842: return "LargePark_BottomRight";
                case 843: return "TrainStation_TopLeft";
                case 844: return "TrainStation_TopCenter";
                case 845: return "TrainStation_TopRight";
                case 846: return "TrainStation_CenterLeft";
                case 847: return "TrainStation_Center";
                case 848: return "TrainStation_CenterRight";
                case 849: return "TrainStation_BottomLeft";
                case 850: return "TrainStation_BottomCenter";
                case 851: return "TrainStation_BottomRight";
                case 852: return "Road_DrawbridgeClear_A";
                case 853: return "Road_DrawbridgeClear_B";
                case 854: return "Road_DrawbridgeEW_TopLeft";
                case 855: return "Road_DrawbridgeEW_BottomLeft";
                case 856: return "Road_DrawbridgeEW_TopRight";
                case 857: return "Road_DrawbridgeEW_BottomRight";
                case 858: return "Road_DrawbridgeNS_BottomLeft";
                case 859: return "Road_DrawbridgeNS_BottomRight";
                case 860: return "Road_DrawbridgeNS_TopLeft";
                case 861: return "Road_DrawbridgeNS_TopRight";
                case 862: return "(Unused)";
                case 863: return "(Unused)";
                case 864: return "(Unused)";
                case 865: return "(Unused)";
                case 866: return "(Unused)";
                case 867: return "(Unused)";
                case 868: return "Fallout";
                case 869: return "FloodedLand";
                case 870: return "StadiumDomed_TopLeft";
                case 871: return "StadiumDomed_TopCenterL";
                case 872: return "StadiumDomed_TopCenterR";
                case 873: return "StadiumDomed_TopRight";
                case 874: return "StadiumDomed_UpperLeft";
                case 875: return "StadiumDomed_UpperCenterL";
                case 876: return "StadiumDomed_UpperCenterR";
                case 877: return "StadiumDomed_UpperRight";
                case 878: return "StadiumDomed_LowerLeft";
                case 879: return "StadiumDomed_LowerCenterL";
                case 880: return "StadiumDomed_LowerCenterR";
                case 881: return "StadiumDomed_LowerRight";
                case 882: return "StadiumDomed_BottomLeft";
                case 883: return "StadiumDomed_BottomCenterL";
                case 884: return "StadiumDomed_BottomCenterR";
                case 885: return "StadiumDomed_BottomRight";
                case 886: return "Residential_TopEW_WestHalf_TopLeft";
                case 887: return "Residential_TopEW_WestHalf_TopCenter";
                case 888: return "Residential_TopEW_WestHalf_TopRight";
                case 889: return "Residential_TopEW_WestHalf_CenterLeft";
                case 890: return "Residential_TopEW_WestHalf_Center";
                case 891: return "Residential_TopEW_WestHalf_CenterRight";
                case 892: return "Residential_TopEW_WestHalf_BottomLeft";
                case 893: return "Residential_TopEW_WestHalf_BottomCenter";
                case 894: return "Residential_TopEW_WestHalf_BottomRight";
                case 895: return "Residential_TopEW_EastHalf_TopLeft";
                case 896: return "Residential_TopEW_EastHalf_TopCenter";
                case 897: return "Residential_TopEW_EastHalf_TopRight";
                case 898: return "Residential_TopEW_EastHalf_CenterLeft";
                case 899: return "Residential_TopEW_EastHalf_Center";
                case 900: return "Residential_TopEW_EastHalf_CenterRight";
                case 901: return "Residential_TopEW_EastHalf_BottomLeft";
                case 902: return "Residential_TopEW_EastHalf_BottomCenter";
                case 903: return "Residential_TopEW_EastHalf_BottomRight";
                case 904: return "Residential_TopNS_NorthHalf_TopLeft";
                case 905: return "Residential_TopNS_NorthHalf_TopCenter";
                case 906: return "Residential_TopNS_NorthHalf_TopRight";
                case 907: return "Residential_TopNS_NorthHalf_CenterLeft";
                case 908: return "Residential_TopNS_NorthHalf_Center";
                case 909: return "Residential_TopNS_NorthHalf_CenterRight";
                case 910: return "Residential_TopNS_NorthHalf_BottomLeft";
                case 911: return "Residential_TopNS_NorthHalf_BottomCenter";
                case 912: return "Residential_TopNS_NorthHalf_BottomRight";
                case 913: return "Residential_TopNS_SouthHalf_TopLeft";
                case 914: return "Residential_TopNS_SouthHalf_TopCenter";
                case 915: return "Residential_TopNS_SouthHalf_TopRight";
                case 916: return "Residential_TopNS_SouthHalf_CenterLeft";
                case 917: return "Residential_TopNS_SouthHalf_Center";
                case 918: return "Residential_TopNS_SouthHalf_CenterRight";
                case 919: return "Residential_TopNS_SouthHalf_BottomLeft";
                case 920: return "Residential_TopNS_SouthHalf_BottomCenter";
                case 921: return "Residential_TopNS_SouthHalf_BottomRight";
                case 922: return "Commercial_TopEW_WestHalf_TopLeft";
                case 923: return "Commercial_TopEW_WestHalf_TopCenter";
                case 924: return "Commercial_TopEW_WestHalf_TopRight";
                case 925: return "Commercial_IOpEW_WestHalf_CenterLeft";
                case 926: return "Commercial_TopEW_WestHalf_Center";
                case 927: return "Commercial_TopEW_WestHalf_CenterRight";
                case 928: return "Commercial_TopEW_WestHalf_BottomLeft";
                case 929: return "Commercial_TopEW_WestHalf_BottomCenter";
                case 930: return "Commercial_TopEW_WestHalf_BottomRight";
                case 931: return "Commercial_TopEW_EastHalf_TopLeft";
                case 932: return "Commercial_TopEW_EastHalf_TopCenter";
                case 933: return "Commercial_TopEW_EastHalf_TopRight";
                case 934: return "Commercial_TopEW_EastHalf_CenterLeft";
                case 935: return "Commercial_TopEW_EastHalf_Center";
                case 936: return "Commercial_TopEW_EastHalf_CenterRight";
                case 937: return "Commercial_TopEW_EastHalf_BottomLeft";
                case 938: return "Commercial_TopEW_EastHalf_BottomCenter";
                case 939: return "Commercial_TopEW_EastHalf_BottomRight";
                case 940: return "Commercial_TopNS_NorthHalf_TopLeft";
                case 941: return "Commercial_TopNS_NorthHalf_TopCenter";
                case 942: return "Commercial_TopNS_NorthHalf_TopRight";
                case 943: return "Commercial_TopNS_NorthHalf_CenterLeft";
                case 944: return "Commercial_TopNS_NorthHalf_Center";
                case 945: return "Commercial_TopNS_NorthHalf_CenterRight";
                case 946: return "Commercial_TopNS_NorthHalf_BottomLeft";
                case 947: return "Commercial_TopNS_NorthHalf_BottomCenter";
                case 948: return "Commercial_TopNS_NorthHalf_BottomRight";
                case 949: return "Commercial_TopNS_SouthHalf_TopLeft";
                case 950: return "Commercial_TopNS_SouthHalf_TopCenter";
                case 951: return "Commercial_TopNS_SouthHalf_TopRight";
                case 952: return "Commercial_TopNS_SouthHalf_CenterLeft";
                case 953: return "Commercial_TopNS_SouthHalf_Center";
                case 954: return "Commercial_TopNS_SouthHalf_CenterRight";
                case 955: return "Commercial_TopNS_SouthHalf_BottomLeft";
                case 956: return "Commercial_TopNS_SouthHalf_BottomCenter";
                case 957: return "Commercial_TopNS_SouthHalf_BottomRight";
                case 958: return "(Unused)";
                case 959: return "(Unused)";
                case 960: return "(Unused)";
                case 961: return "(Unused)";
                case 962: return "(Unused)";
                case 963: return "(Unused)";
                case 964: return "(Unused)";
                case 965: return "(Unused)";
                case 966: return "(Unused)";
                case 967: return "(Unused)";
                case 968: return "(Unused)";
                case 969: return "(Unused)";
                case 970: return "(Unused)";
                case 971: return "(Unused)";
                case 972: return "(Unused)";
                case 973: return "(Unused)";
                case 974: return "(Unused)";
                case 975: return "(Unused)";
                case 976: return "(Unused)";
                case 977: return "(Unused)";
                case 978: return "(Unused)";
                case 979: return "(Unused)";
                case 980: return "(Unused)";
                case 981: return "(Unused)";
                case 982: return "(Unused)";
                case 983: return "(Unused)";
                case 984: return "(Unused)";
                case 985: return "(Unused)";
                case 986: return "(Unused)";
                case 987: return "(Unused)";
                case 988: return "(Unused)";
                case 989: return "(Unused)";
                case 990: return "(Unused)";
                case 991: return "(Unused)";
                case 992: return "(Unused)";
                case 993: return "(Unused)";
                case 994: return "(Unused)";
                case 995: return "(Unused)";
                case 996: return "(Unused)";
                case 997: return "(Unused)";
                case 998: return "(Unused)";
                case 999: return "(Unused)";
                case 1000: return "(Unused)";
                case 1001: return "(Unused)";
                case 1002: return "(Unused)";
                case 1003: return "(Unused)";
                case 1004: return "(Unused)";
                case 1005: return "(Unused)";
                case 1006: return "(Unused)";
                case 1007: return "(Unused)";
                case 1008: return "(Unused)";
                case 1009: return "(Unused)";
                case 1010: return "(Unused)";
                case 1011: return "(Unused)";
                case 1012: return "(Unused)";
                case 1013: return "(Unused)";
                case 1014: return "(Unused)";
                case 1015: return "(Unused)";
                case 1016: return "(Unused)";
                case 1017: return "(Unused)";
                case 1018: return "(Unused)";
                case 1019: return "(Unused)";
                case 1020: return "(Unused)";
                case 1021: return "(Unused)";
                case 1022: return "(Unused)";
                case 1023: return "(Unused)";
                default: throw new ArgumentException(); //Should never happen.
            }
        }
    }
}
