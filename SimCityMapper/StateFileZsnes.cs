using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SimCityMapper
{
    class StateFileZsnes : StateFile
    {
        private const int RamStart = 3091; //State file RAM offset.
        private const int VramStart = RamStart + RamLength; //RAM is followed by VRAM.
        private const int OamRamStart = 536;
        private const int CgRamStart = OamRamStart + 1024;

        private readonly string mSignature;
        private readonly byte[] mContent;

        private StateFileZsnes(string signature, byte[] content, byte[] ram, byte[] vram)
            : base(ram, vram)
        {
            mSignature = signature;
            mContent = content;
        }

        public static StateFileZsnes open(string statePath)
        {
            using (FileStream fs = File.OpenRead(statePath))
            {
                return open(fs);
            }
        }

        public static StateFileZsnes open(Stream stream)
        {
            //ZSNES state file format is a bit difficult to find information about. Best so far:
            //https://fdwr.tripod.com/docs/zst_frmt.txt
            //For an older version V0.6. I use ZSNES 1.51 and the version in its state files are V143.
            //They seem similar though and some things I found so far in SimCity state files made by ZSNES 1.51:
            //Starts with a 26 byte signature = "ZSNES Save State File V143".
            //Then 3065 bytes of miscellaneous data.
            //Then 128K RAM (26+3065=3091 offset).
            //Then 64K VRAM (3091+131072=134163 offset).
            //Total so far = 199699 bytes like in the old version state file.
            //But there is an extra 107336 unknown bytes at the end in this new version.
            //I guess from the starting comments in the document that this is SPC, WRAM, etc.
            //I.e. sound stuff and the extra 32K save RAM in the SimCity cartridge?

            //Check signature and version in header.
            string signature = stream.readString(26);
            if (signature != "ZSNES Save State File V143")
            {
                throw new InvalidDataException("ZSNES signature not present or version not supported!");
            }
            stream.Seek(-26, SeekOrigin.Current);

            byte[] content = stream.readArray(stream.Length - stream.Position); //Read rest of stream into an array.

            byte[] ram = new byte[RamLength];
            Array.Copy(content, RamStart, ram, 0, ram.Length);

            byte[] vram = new byte[VramLength];
            Array.Copy(content, VramStart, vram, 0, vram.Length);

            return new StateFileZsnes(signature, content, ram, vram);
        }

        public override byte[] findSram()
        {
            throw new NotImplementedException();
        }

        public override void save(string savePath)
        {
            //Write any changes to RAM and VRAM back to file.
            Array.Copy(Ram, 0, mContent, RamStart, Ram.Length);
            Array.Copy(Vram, 0, mContent, VramStart, Vram.Length);
            File.WriteAllBytes(savePath, mContent);
        }
    }
}
