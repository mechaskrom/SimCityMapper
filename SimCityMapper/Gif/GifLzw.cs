//#define DEBUG_LZW

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Mechaskrom.Gif
{
    /// <summary>
    /// GIF-LZW compression.
    /// </summary>
    static class GifLzw
    {
        private class CodeWriter //Convert codes -> bits -> bytes -> sub-block -> stream.
        {
            private readonly Stream mStream;
            private readonly byte[] mByteBuffer;
            private int mByteCount;
            private int mBitBuffer;
            private int mBitCount;

#if DEBUG_LZW
            //For debugging. E.g. compare with http://www.matthewflickinger.com/lab/whatsinagif/gif_explorer.asp
            private readonly List<int> mCodesOut = new List<int>();
            private readonly List<byte> mBytesOut = new List<byte>();
#endif

            public CodeWriter(Stream stream)
            {
                mStream = stream;
                mByteBuffer = new byte[255];
                mByteCount = 0;
                mBitBuffer = 0;
                mBitCount = 0;
            }

            public void writeCode(int code, int codeBitLength)
            {
                //Write code bits to bit buffer.
                mBitBuffer |= code << mBitCount;
                mBitCount += codeBitLength;

                //Convert bit buffer to bytes.
                while (mBitCount >= 8)
                {
                    writeByte((byte)mBitBuffer);
                    mBitBuffer >>= 8;
                    mBitCount -= 8;
                }

#if DEBUG_LZW
                mCodesOut.Add(code);
#endif
            }

            private void writeByte(byte b) //Write byte to byte buffer.
            {
                if (mByteCount == 255) //Max 255 bytes per data sub-block.
                {
                    writeBlock(255);
                    mByteCount = 0;
                }
                mByteBuffer[mByteCount++] = b;
            }

            private void writeBlock(byte length) //Write byte buffer to stream as a block.
            {
                mStream.WriteByte(length); //Block length.
                mStream.Write(mByteBuffer, 0, length);

#if DEBUG_LZW
                mBytesOut.Add(length);
                mBytesOut.AddRange(mByteBuffer.Take(length));
#endif
            }

            public void flush()
            {
                //Convert any remaining bits to a byte (missing bits are set to 0).
                if (mBitCount > 0)
                {
                    System.Diagnostics.Debug.Assert(mBitCount <= 8, "Remaining bits should be equal to or less than 8!");
                    writeByte((byte)mBitBuffer);
                }

                //Write any remaining bytes to stream.
                if (mByteCount > 0)
                {
                    System.Diagnostics.Debug.Assert(mByteCount <= 255, "Remaining bytes should be equal to or less than 255!");
                    writeBlock((byte)mByteCount);
                }

#if DEBUG_LZW
                debugSaveCodesOut(mCodesOut);
                debugSaveBytesOut(mBytesOut);
#endif
            }

#if DEBUG_LZW
            private static void debugSaveCodesOut(List<int> codesOut) //For debugging.
            {
                StringBuilder sbCodes = new StringBuilder();
                for (int i = 0; i < codesOut.Count; i++)
                {
                    sbCodes.Append(("#" + codesOut[i].ToString() + " ").PadLeft(6));
                    if ((i % 10) == 9)
                    {
                        sbCodes.AppendLine();
                    }
                }
                File.WriteAllText(debugSavePath() + "GIF-LZW codes out.txt", sbCodes.ToString());
            }

            private static void debugSaveBytesOut(List<byte> bytesOut) //For debugging.
            {
                //Write bytes like a GIF image data block.
                List<byte> imageData = new List<byte>();
                imageData.Add(8);//LZW minimum code size. Typically number of bits per pixel.
                imageData.AddRange(bytesOut);//LZW compressed data sub-blocks.
                imageData.Add(0);//Terminator.

                StringBuilder sbBytes = new StringBuilder();
                for (int i = 0; i < imageData.Count; i++)
                {
                    sbBytes.Append(imageData[i].ToString("X2") + " ");
                    if ((i % 25) == 24)
                    {
                        sbBytes.AppendLine();
                    }
                }
                File.WriteAllText(debugSavePath() + "GIF-LZW bytes out.txt", sbBytes.ToString());
            }

            private static string debugSavePath() //For debugging.
            {
                //Return path to .exe and ensure that it ends with a directory separator.
                string path = AppDomain.CurrentDomain.BaseDirectory;
                return Path.GetFullPath(path).TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar;
            }
#endif
        }

        /// <summary>
        /// Encodes data to a stream using GIF-LZW compression.
        /// </summary>
        /// <param name="data">Data entries (input) to compress.</param>
        /// <param name="dataBitLength">Length per data entry (input) in bits.</param>
        /// <param name="maxBitLength">Max length of compression codes (output) in bits.</param>
        /// <param name="outStream">Stream to write compressed data to.</param>
        public static void encode(byte[] data, int dataBitLength, int maxBitLength, Stream outStream)
        {
            if (dataBitLength < 2 || dataBitLength > 8)
            {
                throw new ArgumentException("Data bit length should be 2-8!");
            }
            if (maxBitLength <= dataBitLength || maxBitLength > 12)
            {
                throw new ArgumentException("Max bit length should be larger than data bit length and max 12!");
            }

            //GIF uses a variant of LZW (Lempel-Ziv-Welch) compression where the main difference is that
            //GIF-LZW uses two additional special codes (clear and end).

            //Patterns (a certain sequence of bytes) are converted to and stored as codes in a table.
            //A new pattern is always formed by an already stored pattern plus one new character.
            //Therefore codes in the table can be thought of as 'pattern code' + 'one character'.
            //Let's call these two values 'p' and 'c'.

            //We can take advantage of this fact and use a dictionary for some pretty fast encoding.
            //The dictionary will at most contain 2^(max bit length) - 2^(data bit length) entries
            //e.g. 4096-256=3840 entries if 8 bit data and 12 bit max length, so it is also pretty memory
            //efficient. Keys in the dictionary are created by bit shifting and or-ing the 'p' and 'c'
            //values together. Some codes will be a single byte pattern (root codes) and consist of
            //just 'c' (0-255 if 8 bit data) i.e. 'p' is 0, but root codes don't need to be stored in
            //the dictionary. The dictionary is just used to look up 'p+c' codes.

            //We could use a 2D array (look-up-table) instead of a dictionary, but it can get pretty big.
            //For example if max bit length is 12 and data bit length is 8 (two pretty common values)
            //then the table would be 4096*256*2=2MB if using 16 bit shorts (enough to store 12 bit codes in).
            //I did some testing and a dictionary was similar or faster speedwise.

            int rootCodeCount = 1 << dataBitLength;
            int clearCode = rootCodeCount + 0; //Value for the first special GIF-LZW code (clear).
            int endCode = clearCode + 1; //Value for the second special GIF-LZW code (end).
            int maxCode = 1 << maxBitLength;

            int codeBitLength = dataBitLength + 1;
            int nextCode = endCode + 1;

            CodeWriter codeWriter = new CodeWriter(outStream);
            codeWriter.writeCode(clearCode, codeBitLength); //Start with a clear code.
            if (data.Length > 0)
            {
                Dictionary<int, int> codeTable = new Dictionary<int, int>(); //Stores 'p+c' codes.
                int p = data[0]; //Pattern 'p' code. Same as 'c' (i.e. first data byte) at start.
                for (int i = 1; i < data.Length; i++)
                {
                    byte c = data[i];
                    int pc = (p << dataBitLength) | c; //Combine 'p' and 'c'.
                    int pcExisting;
                    if (codeTable.TryGetValue(pc, out pcExisting)) //Pattern 'p+c' code already exists in table?
                    {
                        p = pcExisting; //Set pattern 'p' code to existing code of 'p+c'.
                    }
                    else //Pattern 'p+c' is new.
                    {
                        //Write code for pattern 'p' to output.
                        codeWriter.writeCode(p, codeBitLength);

                        //Pattern 'p+c' is new so add it to the table.
                        if (nextCode < maxCode) //Table has room for a new code?
                        {
                            if (nextCode == (1 << codeBitLength)) //Time to increase code bit length?
                            {
                                codeBitLength++;
                            }
                            codeTable.Add(pc, nextCode++);
                        }
                        else //Table is full.
                        {
                            codeTable.Clear(); //Clear code table.
                            codeWriter.writeCode(clearCode, codeBitLength); //Restart with a clear code.
                            codeBitLength = dataBitLength + 1; //Reset code bit length.
                            nextCode = endCode + 1;
                        }
                        p = c; //Start a new pattern.
                    }
                }
                //Write last pattern code to output.
                codeWriter.writeCode(p, codeBitLength);
            }
            codeWriter.writeCode(endCode, codeBitLength); //End with end code.
            codeWriter.flush(); //Flush any remaining data to stream.
        }

        #region OldDeprecatedStuff
        [Obsolete("Method is not used anymore. Kept here for a reference only on how to do it the old way.")]
        private static void codesToBytes(List<int> codes, int dataBitLength, Stream stream)
        {
            //Convert codes -> bits -> bytes -> sub-block -> stream. Data bit length should be 2-8.

            //The old encoder used to just add codes to a list and return it to this method.
            //That was maybe a bit easier to understand so I'm keeping this for now for a
            //reference on how to do it if I ever change my mind. Speedwise the old and new
            //method are very similar though.

            CodeWriter codeWriter = new CodeWriter(stream);
            int codeBitLength = dataBitLength + 1;
            int clearCode = 1 << dataBitLength;
            int currCode = 1 << dataBitLength; //Used to determine when code bit length should increase.
            int nextCodeLengthen = currCode * 2;
            for (int i = 0; i < codes.Count; i++)
            {
                int code = codes[i];

                codeWriter.writeCode(code, codeBitLength);

                //Clear code after starting one?
                if (code == clearCode && i > 0)
                {
                    //Reset code bit length and when to increase it next time.
                    codeBitLength = dataBitLength + 1;
                    currCode = 1 << dataBitLength;
                    nextCodeLengthen = currCode * 2;
                }

                //Increase current code. Stop after it reaches 4095 (max code).
                if (currCode < 4095)
                {
                    currCode++;
                    if (currCode == nextCodeLengthen) //Time to increase code bit length?
                    {
                        codeBitLength++;
                        nextCodeLengthen *= 2;
                    }
                }
            }
            codeWriter.flush(); //Flush any remaining data to stream.
        }
        #endregion
    }
}
