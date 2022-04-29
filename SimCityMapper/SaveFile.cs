using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SimCityMapper
{
    abstract class SaveFile
    {
        //Save file can have multiple cities e.g. SRAM files have two.
        public abstract City[] getCities();

        public static SaveFile tryOpen(string filePath)
        {
            StringBuilder sb = new StringBuilder();
            using (FileStream fs = File.OpenRead(filePath))
            {
                StateFile state;
                if (tryOpen(StateFileSnes9x.open, fs, sb, "-Snes9x: ", out state)) return SaveFileState.open(state);
                if (tryOpen(StateFileZsnes.open, fs, sb, "-ZSNES: ", out state)) return SaveFileState.open(state);

                //Not an emulator state file. Try as an SRAM file as a last resort.
                try
                {
                    fs.Seek(0, SeekOrigin.Begin);
                    return SaveFileSram.open(fs);
                }
                catch (Exception ex)
                {
                    sb.AppendLine("-SRAM: " + ex.Message);
                }
            }
            throw new ArgumentException("Couldn't open save file. Tried formats and reason: \n" + sb.ToString());
        }

        private static bool tryOpen(Func<FileStream, StateFile> opener, FileStream fs, StringBuilder sb, string format, out StateFile state)
        {
            try
            {
                fs.Seek(0, SeekOrigin.Begin);
                state = opener(fs);
                return true;
            }
            catch (Exception ex)
            {
                sb.AppendLine(format + ex.Message);
            }
            state = null;
            return false;
        }
    }
}
