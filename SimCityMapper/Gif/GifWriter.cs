using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace Mechaskrom.Gif
{
    //Color table = color palette in GIF standard language.

    /// <summary>
    /// Writes multiple frames as an animated GIF.
    /// </summary>
    class GifWriter
    {
        private const int WidthMax = UInt16.MaxValue;
        private const int HeightMax = UInt16.MaxValue;

        protected const int BitsPerPixel = 8; //Color table/palette entries and frame pixels.
        protected const int MaxBitLength = 12; //Used in LZW compression of a frame.

        private bool mIsInit;
        protected readonly MemoryStream mStream;
        private readonly Color[] mGlobalColorTable; //Used on frames without a local color table/palette.
        private readonly UInt16 mLoopCount; //Times to loop animation. 0 = infinite loop.

        /// <summary>
        /// Constructs a GIF writer.
        /// </summary>
        /// <param name="globalColorTable">Used on frames without a local color table. null = use local color tables instead.</param>
        /// <param name="loopCount">Times to loop the animation. 0 = infinite loop.</param>
        public GifWriter(Color[] globalColorTable, UInt16 loopCount)
        {
            if (globalColorTable != null && globalColorTable.Length < (1 << BitsPerPixel))
            {
                throw new ArgumentException("Global color table must have at least " + (1 << BitsPerPixel) + " entries!");
            }

            mIsInit = false;
            mStream = new MemoryStream();
            mGlobalColorTable = globalColorTable;
            mLoopCount = loopCount;
        }

        /// <summary>
        /// Adds a frame to the animation.
        /// </summary>
        /// <param name="frame">Frame to add.</param>
        /// <param name="localColorTable">Local color table. null = use global color table instead.</param>
        /// <param name="delayCentiseconds">Display duration in centiseconds. </param>
        public void addFrame(GifFrame frame, Color[] localColorTable, UInt16 delayCentiseconds)
        {
            if (frame.Width > WidthMax)
            {
                throw new ArgumentException("Frame can't be wider than " + WidthMax + " pixels!");
            }

            if (frame.Height > HeightMax)
            {
                throw new ArgumentException("Frame can't be taller than " + HeightMax + " pixels!");
            }

            if (mGlobalColorTable == null && localColorTable == null)
            {
                throw new ArgumentException("Must provide a local color table because this GIF writer was created without a global color table!");
            }

            if (localColorTable != null && localColorTable.Length < (1 << BitsPerPixel))
            {
                throw new ArgumentException("Local color table must have at least " + (1 << BitsPerPixel) + " entries!");
            }

            if (!mIsInit) //GIF stream isn't yet initialized?
            {
                writeHeader();
                writeLogicalScreenDescriptor((UInt16)frame.Width, (UInt16)frame.Height);
                writeGlobalColorTable();
                writeApplicationExtension();
                mIsInit = true;
            }

            writeGraphicsControlExtension(delayCentiseconds);
            writeImageDescriptor((UInt16)frame.Width, (UInt16)frame.Height, localColorTable);
            writeLocalColorTable(localColorTable);
            writeImageData(frame.Pixels);
        }

        /// <summary>
        /// Write result to a file in the specified path.
        /// </summary>
        /// <param name="savePath">The path and name of the file to create.</param>
        public void save(string savePath)
        {
            using (FileStream fs = File.Create(savePath))
            {
                save(fs);
            }
        }

        /// <summary>
        /// Write result to the specified stream.
        /// </summary>
        /// <param name="stream">The stream to write result to.</param>
        public void save(Stream stream)
        {
            if (!mIsInit)
            {
                throw new ArgumentException("The GIF writer was saved without any frames added to it!");
            }

            //Write content of internal stream to destination stream.
            mStream.WriteTo(stream);

            //Write EOF marker to destination stream.
            stream.WriteByte(0x3B); //Trailer (EOF marker).
        }

        private void writeHeader()
        {
            //GIF header.

            //Signature + version.
            writeString("GIF89a");
        }

        private void writeLogicalScreenDescriptor(UInt16 canvasWidth, UInt16 canvasHeight)
        {
            //Information about the canvas (what frames are drawn on) and the global color table.

            //Canvas width.
            writeUInt16(canvasWidth);

            //Canvas height.
            writeUInt16(canvasHeight);

            //Packed field.
            int globalColorTableSize = BitsPerPixel - 1; //(3 bits) Global color table size. 2^(N+1).
            int sortFlag = 0; //(1 bit) Global color table is sorted? Not required so leave it at 0.
            int colorResolution = BitsPerPixel - 1; //(3 bits) Number of bits per pixel minus 1.
            int globalColorTableFlag = mGlobalColorTable != null ? 1 : 0; //(1 bit) Global color table is present?
            writeUInt8((byte)(
                (globalColorTableSize << 0) |
                (sortFlag << 3) |
                (colorResolution << 4) |
                (globalColorTableFlag << 7)));

            //Background color index. Index in global color table to use in background.
            writeUInt8(0);

            //Pixel aspect ratio. (N + 15) / 64 if N != 0.
            writeUInt8(0);
        }

        private void writeGlobalColorTable()
        {
            //Optional global color table. Used if a frame has not a local one.
            if (mGlobalColorTable != null)
            {
                writeColorTable(mGlobalColorTable, 1 << BitsPerPixel);
            }
        }

        private void writeApplicationExtension()
        {
            //Optional extension to specify loop count of an animated GIF.

            //Extension introducer.
            writeUInt8(0x21);

            //Extension label.
            writeUInt8(0xFF);

            //Block length.
            writeUInt8(11);

            //Application identifier + application authentication code.
            writeString("NETSCAPE2.0");

            //Sub-block length.
            writeUInt8(3);

            //Sub-block identifier.
            writeUInt8(1);

            //Loop count. 0 = infinite loop.
            writeUInt16(mLoopCount);

            //Sub-block terminator.
            writeUInt8(0);
        }

        private void writeGraphicsControlExtension(UInt16 delayCentiseconds)
        {
            //Optional extension to specify transparency and some animation settings for the following frame.

            //Extension introducer.
            writeUInt8(0x21);

            //Graphic control label.
            writeUInt8(0xF9);

            //Block length.
            writeUInt8(4);

            //Packed field.
            int transparentColorFlag = 0; //(1 bit) Do not draw pixels with the specified transparent color index?
            int userInputFlag = 0; //(1 bit) Wait for some sort of "input" from the user before moving on to the next image?
            int disposalMethod = 1; //3 bits) What happens to the current image when you move onto the next in an animation. *1.
            int reserved = 0; //(3 bits)
            writeUInt8((byte)(
                (transparentColorFlag << 0) |
                (userInputFlag << 1) |
                (disposalMethod << 2) |
                (reserved << 5)));

            //Delay time. In hundredths of seconds (centiseconds).
            writeUInt16(delayCentiseconds);

            //Transparent color index. Set the transparent color flag in the packed field to activate it.
            writeUInt8(0);

            //Block terminator.
            writeUInt8(0);

            //*1 = Disposal methods for the current image:
            //0=Don't care.
            //1=Leave it. Next image is drawn over it.
            //2=Clear it to the background color (as indicated by the logical screen descriptor).
            //3=Restore the canvas to the previous state (before current image was drawn). Not widely supported?
            //4-7=Undefined.
        }

        private void writeImageDescriptor(UInt16 imageWidth, UInt16 imageHeight, Color[] localColorTable)
        {
            //Information about the following frame and its local color table.

            //Image separator.
            writeUInt8(0x2C);

            //Canvas X position. Where the image should begin on the canvas.
            writeUInt16(0);

            //Canvas Y position. Where the image should begin on the canvas.
            writeUInt16(0);

            //Image width.
            writeUInt16(imageWidth);

            //Image height.
            writeUInt16(imageHeight);

            //Packed field.
            int localColorTableSize = BitsPerPixel - 1; //(3 bits) Local color table size. 2^(N+1).
            int reserved = 0; //(2 bits) Reserved for future use. 
            int sortFlag = 0; //(1 bit) Local color table is sorted? Not required so leave it at 0.
            int interlaceFlag = 0; //(1 bit) Image is interlaced?
            int localColorTableFlag = localColorTable != null ? 1 : 0; //(1 bit) Local color table is present?
            writeUInt8((byte)(
                (localColorTableSize << 0) |
                (reserved << 3) |
                (sortFlag << 5) |
                (interlaceFlag << 6) |
                (localColorTableFlag << 7)));

        }

        private void writeLocalColorTable(Color[] localColorTable)
        {
            //Optional local color table for the following frame. Global color table is used if none.
            if (localColorTable != null)
            {
                writeColorTable(localColorTable, 1 << BitsPerPixel);
            }
        }

        private void writeImageData(byte[] data) //Data is 8-bit palette indices.
        {
            //Frame data written in an LZW GIF variant.

            //LZW minimum code size. Typically number of bits per pixel.
            writeUInt8(BitsPerPixel);

            //GIF-LZW compressed data sub-blocks.
            writeGifLzwData(data);

            //Terminator.
            writeUInt8(0);
        }

        protected virtual void writeGifLzwData(byte[] data)
        {
            GifLzw.encode(data, BitsPerPixel, MaxBitLength, mStream);
        }

        private void writeUInt8(byte val)
        {
            mStream.WriteByte(val);
        }

        private void writeUInt16(UInt16 val)
        {
            mStream.WriteByte((byte)(val >> 0));
            mStream.WriteByte((byte)(val >> 8));
        }

        private void writeString(string str)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(str);
            mStream.Write(bytes, 0, bytes.Length);
        }

        private void writeColorTable(Color[] colorTable, int entryCount)
        {
            for (int i = 0; i < entryCount; i++)
            {
                Color color = colorTable[i];
                writeUInt8(color.R);
                writeUInt8(color.G);
                writeUInt8(color.B);
            }
        }
    }

    /// <summary>
    /// Writes multiple frames with caching as an animated GIF.<br/>
    /// 
    /// Compressed frames are cached which will make adding many repeated frames faster.<br/>
    /// Only the reference to a frame's content is cached so any editing of<br/>
    /// its content after it's been added will not show up in the final animation.
    /// </summary>
    class GifWriterCached : GifWriter
    {
        private readonly Dictionary<byte[], byte[]> mFrameCache; //Uncompressed data to compressed data lookup.

        /// <summary>
        /// Constructs a GIF writer with frame caching.
        /// </summary>
        ///<param name="globalColorTable">Used on frames without a local color table. null = use local color tables instead.</param>
        /// <param name="loopCount">Times to loop the animation. 0 = infinite loop.</param>
        public GifWriterCached(Color[] globalColorTable, UInt16 loopCount)
            : base(globalColorTable, loopCount)
        {
            mFrameCache = new Dictionary<byte[], byte[]>();
        }

        protected override void writeGifLzwData(byte[] data)
        {
            byte[] gifLzwData;
            if (!mFrameCache.TryGetValue(data, out gifLzwData)) //Data has not been compressed yet?
            {
                //Compress and cache it.
                MemoryStream ms = new MemoryStream();
                GifLzw.encode(data, BitsPerPixel, MaxBitLength, ms);
                gifLzwData = ms.ToArray();
                mFrameCache.Add(data, gifLzwData);
            }
            mStream.Write(gifLzwData, 0, gifLzwData.Length);
        }
    }
}
