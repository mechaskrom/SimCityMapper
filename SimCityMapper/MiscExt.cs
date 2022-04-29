using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;

namespace SimCityMapper
{
    static class MiscExt
    {
        public static int GreatestCommonFactor(this int va11, int val2)
        {
            while (val2 != 0)
            {
                int temp = val2;
                val2 = va11 % val2;
                va11 = temp;
            }
            return va11;
        }

        public static int LeastCommonMultiple(this int va11, int val2)
        {
            return (va11 / GreatestCommonFactor(va11, val2)) * val2;
        }

        public static int clip(this int val, int min, int max)
        {
            if (val > max) return max;
            if (val < min) return min;
            return val;
        }

        public static Point getOffset(this Point p, int dx, int dy)
        {
            p.Offset(dx, dy);
            return p;
        }

        public static Point getOffset(this Point p, Point point)
        {
            return p.getOffset(point.X, point.Y);
        }

        public static void AppendLine(this StringBuilder sb, string format, object arg0)
        {
            sb.AppendFormat(format, arg0);
            sb.AppendLine();
        }

        public static void AppendLine(this StringBuilder sb, string format, object arg0, object arg1)
        {
            sb.AppendFormat(format, arg0, arg1);
            sb.AppendLine();
        }

        public static void AppendLine(this StringBuilder sb, string format, object arg0, object arg1, object arg2)
        {
            sb.AppendFormat(format, arg0, arg1, arg2);
            sb.AppendLine();
        }

        public static void AppendLine(this StringBuilder sb, string format, params object[] args)
        {
            sb.AppendFormat(format, args);
            sb.AppendLine();
        }

        public static string GetFullPathWithEndingSeparator(this string s)
        {
            //Ensure that path string ends with a directory separator.
            return Path.GetFullPath(s).TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
        }

        public static string ToStringItems<T>(this IEnumerable<T> e)
        {
            //Convert collection items to a string. Mostly to debug print array content.
            StringBuilder sb = new StringBuilder();
            foreach (T item in e)
            {
                sb.Append(item.ToString());
                sb.Append(',');
            }
            return '[' + sb.ToString().TrimEnd(',') + ']';
        }

        public static void AddNotNull<T>(this List<T> l, T item) where T : class
        {
            if (item != null)
            {
                l.Add(item);
            }
        }

        public static Rectangle getRect(this byte[] bytes, int stride)
        {
            //Return byte[] expressed as a rectangle. Useful when checking for out of bound accesses.
            return new Rectangle(0, 0, stride, bytes.Length / stride);
        }

        public static void clearBytes(this byte[] dstBytes, byte value)
        {
            for (int i = 0; i < dstBytes.Length; i++)
            {
                dstBytes[i] = value;
            }
        }

        public static void write16bit(this byte[] dstBytes, int index, int value)
        {
            dstBytes[index + 0] = (byte)(value >> 0);
            dstBytes[index + 1] = (byte)(value >> 8);
        }

        public static void write32bit(this byte[] dstBytes, int index, int value)
        {
            dstBytes[index + 0] = (byte)(value >> 0);
            dstBytes[index + 1] = (byte)(value >> 8);
            dstBytes[index + 2] = (byte)(value >> 16);
            dstBytes[index + 3] = (byte)(value >> 24);
        }

        public static int read16bit(this byte[] dstBytes, int index)
        {
            return (dstBytes[index + 0] << 0) | (dstBytes[index + 1] << 8);
        }

        public static int read32bit(this byte[] dstBytes, int index)
        {
            return (dstBytes[index + 0] << 0) | (dstBytes[index + 1] << 8) | (dstBytes[index + 2] << 16) | (dstBytes[index + 3] << 24);
        }

        public static void drawPixels(this byte[] srcPixels, int srcStride, Rectangle srcRect,
            byte[] dstPixels, int dstStride, Rectangle dstClip, Point dstPos, byte[] remap)
        {
            //Draws part of 'srcPixels' specified by 'srcRect' at 'dstPos' in 'dstPixels'.
            //Source and destination arrays are 8-bit indexed pixels.
            //Source pixels with a 0 index aren't drawn (transparent). Drawn pixels are clipped inside 'dstClip'.
            //Optionally (if not null) uses 'remap'-table to remap pixel indices.
            if (srcPixels.Length % srcStride != 0)
            {
                throw new ArgumentException("Source stride is not a multiple of source array length!");
            }
            if (dstPixels.Length % dstStride != 0)
            {
                throw new ArgumentException("Destination stride is not a multiple of destination array length!");
            }
            if (!getRect(srcPixels, srcStride).Contains(srcRect))
            {
                throw new ArgumentException("Source rectangle is outside source array!");
            }
            if (!getRect(dstPixels, dstStride).Contains(dstClip))
            {
                throw new ArgumentException("Destination clip is outside destination array!");
            }

            //Calculate actual source rectangle and draw position after clipping.
            Rectangle drawRect = new Rectangle(dstPos, srcRect.Size);
            drawRect.Intersect(dstClip); //Clip draw rectangle.
            //Adjust source rectangle and destination with any changes clipping made to draw rectangle.
            srcRect.Offset(drawRect.X - dstPos.X, drawRect.Y - dstPos.Y);
            srcRect.Size = drawRect.Size;
            dstPos = drawRect.Location;

            //Draw part of source specified by 'srcRect' at 'dstPos' in destination.
            int srcInd = srcRect.X + (srcRect.Y * srcStride);
            int dstInd = dstPos.X + (dstPos.Y * dstStride);
            for (int y = 0; y < srcRect.Height; y++, srcInd += srcStride, dstInd += dstStride)
            {
                for (int x = 0; x < srcRect.Width; x++)
                {
                    byte b = srcPixels[srcInd + x];
                    if (remap != null) //Remap index?
                    {
                        b = remap[b];
                    }
                    if (b != 0) //Not a transparent pixel?
                    {
                        dstPixels[dstInd + x] = b;
                    }
                }
            }
        }

        public static void copyBytes(this byte[] srcBytes, int srcStride, byte[] dstBytes, int dstStride, Point dstPos)
        {
            copyBytes(srcBytes, srcStride, new Point(0, 0), dstBytes, dstStride, dstPos, new Size(srcStride, srcBytes.Length / srcStride));
        }

        public static void copyBytes(this byte[] srcBytes, int srcStride, Rectangle srcRect, byte[] dstBytes, int dstStride, Point dstPos)
        {
            copyBytes(srcBytes, srcStride, srcRect.Location, dstBytes, dstStride, dstPos, srcRect.Size);
        }

        private static void copyBytes(this byte[] srcBytes, int srcStride, Point srcPos, byte[] dstBytes, int dstStride, Point dstPos, Size size)
        {
            //Size = size of region to copy.
            if (size.Width * size.Height < 1)
            {
                throw new ArgumentException("Size of region to copy is smaller than 1!");
            }
            if (srcBytes.Length % srcStride != 0)
            {
                throw new ArgumentException("Source stride is not a multiple of source array length!");
            }
            if (dstBytes.Length % dstStride != 0)
            {
                throw new ArgumentException("Destination stride is not a multiple of destination array length!");
            }
            if (!getRect(srcBytes, srcStride).Contains(new Rectangle(srcPos, size)))
            {
                throw new ArgumentException("Source rectangle is outside source array!");
            }
            if (!getRect(dstBytes, dstStride).Contains(new Rectangle(dstPos, size)))
            {
                throw new ArgumentException("Destination rectangle is outside destination array!");
            }

            int srcInd = srcPos.X + (srcPos.Y * srcStride);
            int dstInd = dstPos.X + (dstPos.Y * dstStride);
            for (int row = 0; row < size.Height; row++, srcInd += srcStride, dstInd += dstStride)
            {
                //Buffer.BlockCopy seems a bit faster on byte[] than Array.Copy.
                Buffer.BlockCopy(srcBytes, srcInd, dstBytes, dstInd, size.Width);
            }
        }

        public static byte[] getPixels(this Bitmap bmp)
        {
            //Returns bitmap's pixels as a byte array.
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, bmp.PixelFormat);
            int bitsPerPixel = Bitmap.GetPixelFormatSize(bmpData.PixelFormat);
            int bmpStride = bmpData.Stride;
            int arrStride = (bmpData.Width * bitsPerPixel + 7) / 8; //Round up.
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
    }
}
