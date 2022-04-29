using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace SimCityMapper
{
    //Simple palette indexed 8-bit image. Essentially a 1D byte array with a 2D size.
    class Frame : Mechaskrom.Gif.GifFrame
    {
        public Frame(int width, int height)
            : base(width, height)
        {
        }

        public Frame(int width, int height, byte[] pixels)
            : base(width, height, pixels)
        {
        }

        public Frame(Frame copyFrom) //Makes a deep copy.
            : this(copyFrom.Width, copyFrom.Height)
        {
            Buffer.BlockCopy(copyFrom.Pixels, 0, Pixels, 0, copyFrom.Pixels.Length);
        }

        public byte this[int index]
        {
            get { return mPixels[index]; }
            set { mPixels[index] = value; }
        }

        public byte this[Point pos]
        {
            get { return mPixels[getOffset(pos)]; }
            set { mPixels[getOffset(pos)] = value; }
        }

        public byte this[int x, int y]
        {
            get { return mPixels[getOffset(x, y)]; }
            set { mPixels[getOffset(x, y)] = value; }
        }

        public int getOffset(Point pos)
        {
            return getOffset(pos.X, pos.Y);
        }

        public int getOffset(int x, int y)
        {
            return x + (y * Stride);
        }

        public void clear(byte value)
        {
            mPixels.clearBytes(value);
        }

        public void write(Frame srcFrame, Point dstPos) //Write pixels from source frame to this.
        {
            write(srcFrame, srcFrame.getRect(), dstPos);
        }

        public void write(Frame srcFrame, Rectangle srcRect, Point dstPos)
        {
            write(srcFrame, srcRect, this, dstPos);
        }

        private static void write(Frame srcFrame, Rectangle srcRect, Frame dstFrame, Point dstPos)
        {
            //Write will just copy pixels over to another frame.
            MiscExt.copyBytes(srcFrame.Pixels, srcFrame.Stride, srcRect, dstFrame.Pixels, dstFrame.Stride, dstPos);
        }

        public void draw(Frame srcFrame, Point dstPos) //Draw pixels from source frame to this.
        {
            draw(srcFrame, dstPos, null);
        }

        public void draw(Frame srcFrame, Point dstPos, byte[] remap)
        {
            draw(srcFrame, srcFrame.getRect(), dstPos, remap);
        }

        public void draw(Frame srcFrame, Rectangle srcRect, Point dstPos)
        {
            draw(srcFrame, srcRect, dstPos, null);
        }

        public void draw(Frame srcFrame, Rectangle srcRect, Point dstPos, byte[] remap)
        {
            draw(srcFrame, srcRect, this, getRect(), dstPos, remap);
        }

        private static void draw(Frame srcFrame, Rectangle srcRect, Frame dstFrame, Rectangle dstClip, Point dstPos, byte[] remap)
        {
            //Draw is similar to write (see above), but will skip transparent pixels (index 0),
            //drawn region can be clipped and pixels can be remapped.
            MiscExt.drawPixels(srcFrame.Pixels, srcFrame.Stride, srcRect, dstFrame.Pixels, dstFrame.Stride, dstClip, dstPos, remap);
        }
    }
}
