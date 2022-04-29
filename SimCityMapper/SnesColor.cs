using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace SimCityMapper
{
    struct SnesColor //SNES 15-bit RGB format. 0BBBBBGG GGGRRRRR.
    {
        private byte mR;
        private byte mG;
        private byte mB;

        public SnesColor(int r, int g, int b)
        {
            mR = (byte)(r & 0x1F);
            mG = (byte)(g & 0x1F);
            mB = (byte)(b & 0x1F);
        }

        public SnesColor(int rgb15Bit) //SNES 15-bit RGB format.
            : this(rgb15Bit >> 0, rgb15Bit >> 5, rgb15Bit >> 10)
        {
        }

        public byte R
        {
            get { return mR; }
            set { mR = (byte)(value & 0x1F); }
        }

        public byte G
        {
            get { return mG; }
            set { mG = (byte)(value & 0x1F); }
        }

        public byte B
        {
            get { return mB; }
            set { mB = (byte)(value & 0x1F); }
        }

        public Color toColor() //15-bit to 24-bit.
        {
            //Expand color range (set 3 LSB to value of 3 MSB).
            return Color.FromArgb(
                (mR << 3) | (mR >> 2),
                (mG << 3) | (mG >> 2),
                (mB << 3) | (mB >> 2));
        }

        public override string ToString()
        {
            return string.Format("({0,2},{1,2},{2,2})", mR, mG, mB);
        }
    }

    class SnesPalette
    {
        private readonly SnesColor[] mPalette;

        public SnesPalette(byte[] data, int dataInd)
            : this(data, dataInd, 256)
        {
        }

        public SnesPalette(byte[] data, int dataInd, int colorCount)
        {
            mPalette = new SnesColor[colorCount];
            for (int i = 0; i < mPalette.Length; i++, dataInd += 2)
            {
                int rgb15Bit = data.read16bit(dataInd);
                mPalette[i] = new SnesColor(rgb15Bit);
            }
        }

        public SnesPalette(UInt16[] data, int dataInd)
            : this(data, dataInd, 256)
        {
        }

        public SnesPalette(UInt16[] data, int dataInd, int colorCount)
        {
            mPalette = new SnesColor[colorCount];
            for (int i = 0; i < mPalette.Length; i++, dataInd += 1)
            {
                int rgb15Bit = data[dataInd];
                mPalette[i] = new SnesColor(rgb15Bit);
            }
        }

        public SnesPalette(SnesPalette source)
            : this(source, 0, source.mPalette.Length)
        {
        }

        public SnesPalette(SnesPalette source, int sourceInd, int colorCount)
        {
            mPalette = new SnesColor[colorCount];
            Array.Copy(source.mPalette, sourceInd, mPalette, 0, mPalette.Length);
        }

        public SnesColor this[int index]
        {
            get { return mPalette[index]; }
            set { mPalette[index] = value; }
        }

        public SnesColor[] Entries
        {
            get { return mPalette; }
        }

        public SnesColorRow getSnesColorRow(int row)
        {
            return new SnesColorRow(this, row * SnesColorRow.Length);
        }

        public void writeColorRow(SnesColorRow colorRow, int row)
        {
            Array.Copy(colorRow.Entries, 0, mPalette, row * SnesColorRow.Length, SnesColorRow.Length);
        }

        public Color[] toColors()
        {
            Color[] palette24Bit = new Color[mPalette.Length];
            for (int i = 0; i < mPalette.Length; i++)
            {
                palette24Bit[i] = mPalette[i].toColor();
            }
            return palette24Bit;
        }

        public string ToString(int index, int length, int perRow)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = index; i < (index + length); i++)
            {
                sb.Append(mPalette[i].ToString());
                if ((i - index) % perRow == (perRow - 1))
                {
                    sb.AppendLine();
                }
            }
            return sb.ToString();
        }
    }

    //Always 16 colors.
    class SnesColorRow : SnesPalette
    {
        public const int Length = 16;

        public SnesColorRow(byte[] data, int dataInd)
            : base(data, dataInd, Length)
        {
        }

        public SnesColorRow(UInt16[] data, int dataInd)
            : base(data, dataInd, Length)
        {
        }

        public SnesColorRow(SnesPalette source, int sourceInd)
            : base(source, sourceInd, Length)
        {
        }

        public ColorRow toColorRow()
        {
            return new ColorRow(this);
        }
    }

    //Always 16 colors.
    class ColorRow
    {
        public const int Length = SnesColorRow.Length;

        private readonly Color[] mRow;

        public ColorRow()
            : this(new Color[Length])
        {
        }

        public ColorRow(SnesColorRow snesColorRow)
            : this(snesColorRow.toColors())
        {
        }

        public ColorRow(Color[] colorRow)
        {
            if (colorRow.Length != Length)
            {
                throw new ArgumentException("Row should have exactly " + Length + " colors!");
            }

            mRow = colorRow;
        }

        public Color this[int index]
        {
            get { return mRow[index]; }
            set { mRow[index] = value; }
        }

        public Color[] Entries
        {
            get { return mRow; }
        }
    }
}
