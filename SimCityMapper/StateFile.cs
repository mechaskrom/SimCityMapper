using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SimCityMapper
{
    abstract class StateFile
    {
        protected const int RamLength = 1 << 17;
        protected const int VramLength = 1 << 16;

        private readonly byte[] mRam;
        private readonly byte[] mVram;

        protected StateFile(byte[] ram, byte[] vram)
        {
            mRam = ram;
            mVram = vram;
        }

        public byte[] Ram
        {
            get { return mRam; }
        }

        public byte[] Vram
        {
            get { return mVram; }
        }

        public abstract byte[] findSram(); //Returns null if not found.

        public byte[] getSram() //Throws if not found.
        {
            byte[] sram = findSram();
            if (sram == null)
            {
                throw new Exception("Couldn't find SRAM data in state file!");
            }
            return sram;
        }

        public abstract void save(string statePath);
    }
}
