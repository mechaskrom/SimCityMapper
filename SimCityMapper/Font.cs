using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace SimCityMapper
{
    //Simple bitmap font class for drawing texts to frames.
    class Font
    {
        private const int CharTilesWidthInTiles = 16;
        private const int CharTilesHeightInTiles = 16;

        private readonly Frame mCharTiles;
        private readonly Size mCharTileSize;

        public Font(string charTilesPath)
            : this(openCharTilesImage(charTilesPath))
        {
        }

        public Font(Bitmap charTilesBmp)
            : this(openCharTilesImage(charTilesBmp))
        {
        }

        private Font(Frame charTiles)
        {
            mCharTiles = charTiles;
            mCharTileSize = new Size(charTiles.Width / CharTilesWidthInTiles, charTiles.Height / CharTilesHeightInTiles);
        }

        public Size CharTileSize
        {
            get { return mCharTileSize; }
        }

        private static Frame openCharTilesImage(string bmpPath)
        {
            using (Bitmap bmp = new Bitmap(bmpPath))
            {
                return openCharTilesImage(bmp);
            }
        }

        private static Frame openCharTilesImage(Bitmap bmp)
        {
            if ((bmp.Width % CharTilesWidthInTiles) != 0)
            {
                throw new ArgumentException("Font bitmap's width must be a multiple of " + CharTilesWidthInTiles + "!");
            }

            if ((bmp.Height % CharTilesHeightInTiles) != 0)
            {
                throw new ArgumentException("Font bitmap's height must be a multiple of " + CharTilesHeightInTiles + "!");
            }

            if (bmp.PixelFormat != PixelFormat.Format24bppRgb)
            {
                throw new ArgumentException("Font bitmap's pixel format must be 24-bit RGB!");
            }

            //Convert bitmap pixels to palette indices. Only 3 RGB colors currently supported:
            //-0xFF00FF Magenta = 0 = transparent.
            //-0xFFFFFF White = 1 = light char color.
            //-0x000000 Black = 2 = dark char color (drop shadow).
            Frame frame = new Frame(bmp.Width, bmp.Height);
            byte[] bmpPixels = bmp.getPixels();
            for (int i = 0, j = 0; i < bmpPixels.Length; i += 3, j++)
            {
                int b = bmpPixels[i + 0];
                int g = bmpPixels[i + 1];
                int r = bmpPixels[i + 2];
                int c = (r << 16) | (g << 8) | (b << 0);
                if (c == 0xFF00FF) //Magenta?
                {
                    frame[j] = 0; //Transparent black.
                }
                else if (c == 0xFFFFFF) //White?
                {
                    frame[j] = 1;
                }
                else if (c == 0x000000) //Black?
                {
                    frame[j] = 2;
                }
                else
                {
                    throw new ArgumentException("Font bitmap has unsupported colors!");
                }
            }

            return frame;
        }

        public void draw(string text, Point dstPos, Frame dstFrame, byte[] remap)
        {
            draw(mCharTiles, text, dstPos, dstFrame, remap);
        }

        private static void draw(Frame srcFrame, string text, Point dstPos, Frame dstFrame, byte[] remap)
        {
            //Draw text with char tiles from source to destination frame at position and optionally color remapped.
            Rectangle srcRc = new Rectangle(0, 0, srcFrame.Width / CharTilesWidthInTiles, srcFrame.Height / CharTilesHeightInTiles);
            int orgX = dstPos.X;
            foreach (char c in text)
            {
                if (c == '\n') //New line?
                {
                    dstPos.X = orgX;
                    dstPos.Y += srcRc.Height;
                }
                else if (c <= 255)
                {
                    srcRc.X = (c % CharTilesWidthInTiles) * srcRc.Width;
                    srcRc.Y = (c / CharTilesWidthInTiles) * srcRc.Height;
                    dstFrame.draw(srcFrame, srcRc, dstPos, remap);
                    dstPos.X += srcRc.Width;
                }
                else
                {
                    throw new ArgumentException("Text has unicode character with code above 255!");
                }
            }
        }
    }
}
