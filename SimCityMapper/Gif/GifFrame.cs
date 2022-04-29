using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Mechaskrom.Gif
{
    /// <summary>
    /// Image where every pixel is an index to a palette. Essentially a 1D byte array with a 2D size.
    /// </summary>
    class GifFrame
    {
        protected readonly Size mSize; //Image size in pixels.
        protected readonly byte[] mPixels; //Indices to a palette.

        /// <summary>
        /// Constructs a frame with all pixels set to index 0.
        /// </summary>
        /// <param name="width">Width in pixels.</param>
        /// <param name="height">Height in pixels.</param>
        public GifFrame(int width, int height)
            : this(width, height, new byte[width * height])
        {
        }

        /// <summary>
        /// Constructs a frame from an 8-bit indexed bitmap.
        /// </summary>
        /// <param name="bmp">Bitmap to create frame from.</param>
        public GifFrame(Bitmap bitmap)
            : this(bitmap.Width, bitmap.Height, getPixels(bitmap))
        {
        }

        /// <summary>
        /// Constructs a frame from an array with indices to a palette.
        /// </summary>
        /// <param name="width">Width in pixels.</param>
        /// <param name="height">Height in pixels.</param>
        /// <param name="pixels">Indices to a palette. Length must be equal to width*height.</param>
        public GifFrame(int width, int height, byte[] pixels)
        {
            if ((width * height) != pixels.Length)
            {
                throw new ArgumentException("Width*height doesn't match array length!");
            }
            mSize = new Size(width, height);
            mPixels = pixels;
        }

        /// <summary>
        /// Size of frame in pixels.
        /// </summary>
        public Size Size
        {
            get { return mSize; }
        }

        /// <summary>
        /// Width of frame in pixels.
        /// </summary>
        public int Width
        {
            get { return mSize.Width; }
        }

        /// <summary>
        /// Height of frame in pixels.
        /// </summary>
        public int Height
        {
            get { return mSize.Height; }
        }

        /// <summary>
        /// Content of frame.
        /// </summary>
        public byte[] Pixels
        {
            get { return mPixels; }
        }

        /// <summary>
        /// Length of frame's content. Usually same as stride*height.
        /// </summary>
        public int Length
        {
            get { return mPixels.Length; }
        }

        /// <summary>
        /// Bytes/pixels per row. Usually same as width.
        /// </summary>
        public int Stride
        {
            get { return Width; }
        }

        /// <summary>
        /// Write frame to a file in the specified path.
        /// </summary>
        /// <param name="savePath">The path and name of the file to create.</param>
        /// <param name="palette">Color palette/table. null = use default palette.</param>
        public void save(string savePath, Color[] palette)
        {
            using (Bitmap bmp = toBmp(palette))
            {
                bmp.Save(savePath);
            }
        }

        /// <summary>
        /// Convert frame to a bitmap.
        /// </summary>
        /// <param name="palette">Color palette/table. null = use default palette.</param>
        /// <returns></returns>
        public Bitmap toBmp(Color[] palette)
        {
            return toBmp(mPixels, Stride, getRect(), palette);
        }

        protected static Bitmap toBmp(byte[] srcPixels, int srcStride, Rectangle srcRect, Color[] palette)
        {
            //Saves source pixels as an 8-bit indexed bitmap.
            if (!new Rectangle(0, 0, srcStride, srcPixels.Length / srcStride).Contains(srcRect))
            {
                throw new ArgumentException("Source rectangle is outside source array!");
            }

            Bitmap bmp = new Bitmap(srcRect.Width, srcRect.Height, PixelFormat.Format8bppIndexed);
            if (palette != null) //Use custom palette instead of default?
            {
                setPalette(bmp, palette);
            }
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, bmp.PixelFormat);
            int dstStride = bmpData.Stride;
            int srcInd = srcRect.X + (srcRect.Y * srcStride);
            long dstInd = bmpData.Scan0.ToInt64();
            if (srcRect.X == 0 && srcStride == srcRect.Width && srcStride == dstStride) //Can copy all pixels at once?
            {
                Marshal.Copy(srcPixels, srcInd, (IntPtr)dstInd, srcRect.Width * srcRect.Height);
            }
            else //Copy line-by-line.
            {
                for (int row = 0; row < srcRect.Height; row++, srcInd += srcStride, dstInd += dstStride)
                {
                    Marshal.Copy(srcPixels, srcInd, (IntPtr)dstInd, srcRect.Width);
                }
            }
            bmp.UnlockBits(bmpData);
            return bmp;
        }

        protected Rectangle getRect() //Frame expressed as a rectangle.
        {
            return new Rectangle(0, 0, Width, Height);
        }

        protected static byte[] getPixels(Bitmap bmp)
        {
            //Returns bitmap's pixels as a byte array.
            if (bmp.PixelFormat != PixelFormat.Format8bppIndexed)
            {
                throw new ArgumentException("Bitmap's pixel format must be 8-bit indexed!");
            }

            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, bmp.PixelFormat);
            int bmpStride = bmpData.Stride;
            int arrStride = bmpData.Width;
            byte[] pixels = new byte[arrStride * bmpData.Height];
            long bmpInd = bmpData.Scan0.ToInt64();
            int arrInd = 0;
            if (bmpStride == arrStride) //Can read all pixels at once if same stride.
            {
                Marshal.Copy((IntPtr)bmpInd, pixels, arrInd, pixels.Length);
            }
            else //Read per line if different stride.
            {
                for (int y = 0; y < bmpData.Height; y++, bmpInd += bmpStride, arrInd += arrStride)
                {
                    Marshal.Copy((IntPtr)bmpInd, pixels, arrInd, arrStride);
                }
            }
            bmp.UnlockBits(bmpData);
            return pixels;
        }

        protected static void setPalette(Bitmap bmp, Color[] palette)
        {
            if (palette.Length > 256)
            {
                throw new ArgumentException("Palette can't have more than 256 colors!");
            }

            if (bmp.PixelFormat != PixelFormat.Format8bppIndexed)
            {
                throw new ArgumentException("Bitmap's pixel format must be 8-bit indexed!");
            }

            ColorPalette bmpPalette = bmp.Palette; //Returns a copy.
            for (int i = 0; i < palette.Length; i++)
            {
                bmpPalette.Entries[i] = palette[i];
            }
            //Provided palette may have less than 256 colors so set any remaining entries to black.
            for (int i = palette.Length; i < 256; i++)
            {
                bmpPalette.Entries[i] = Color.Black;
            }
            bmp.Palette = bmpPalette; //Set changed copy as the new palette.
        }
    }
}
