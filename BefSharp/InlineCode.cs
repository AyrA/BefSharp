using System.IO;
using System.Diagnostics;
using System.Text;
using System;

namespace BefSharp
{
    public static class InlineCode
    {
        public struct CompilerSettings
        {
            public bool allowSave;
            public bool runInstantly;
            public bool exitEnd;
        }

        public static byte[] delim
        {
            get
            {
                byte[] b = new byte[20];
                for (int i = 1; i <= b.Length; i++)
                {
                    b[i-1] = (byte)((i*27)%256);
                }
                return b;
            }
        }

        private static string EXE
        {
            get
            {
                return Process.GetCurrentProcess().MainModule.FileName;
            }
        }

        public static bool hasInlineCode()
        {
            return getInlineStart() > -1;
        }

        public static int getInlineStart()
        {
            byte[] ASM = File.ReadAllBytes(EXE);
            byte[] SUB = delim;

            for (int i = 0; i < ASM.Length - SUB.Length; i++)
            {
                //Find first char
                if (ASM[i] == SUB[0])
                {
                    //scan subrange
                    for (int j = 0; j < SUB.Length; j++)
                    {
                        if (ASM[i + j] != SUB[j])
                        {
                            //cancel scan, since not found
                            break;
                        }
                        else if (j == SUB.Length - 1)
                        {
                            return i + j + 1;
                        }
                    }
                }
            }
            return -1;
        }

        public static string getInlineCode()
        {
            if (hasInlineCode())
            {
                byte[] ASM = File.ReadAllBytes(EXE);
                //The first 3 chars are the compiler settings.
                return Encoding.UTF8.GetString(ASM, getInlineStart(), ASM.Length - getInlineStart()).Substring(3);
            }
            return null;
        }

        public static CompilerSettings getSettings()
        {
            CompilerSettings CS = new CompilerSettings();
            CS.allowSave = CS.exitEnd = CS.runInstantly = false;

            if (hasInlineCode())
            {
                byte[] ASM = File.ReadAllBytes(EXE);
                //The first 3 chars are the compiler settings.
                string Settings=Encoding.UTF8.GetString(ASM, getInlineStart(), ASM.Length - getInlineStart()).Substring(0,3);
                CS.allowSave = (Settings[0] == 1);
                CS.exitEnd = (Settings[1] == 1);
                CS.runInstantly = (Settings[2] == 1);
            }
            return CS;
        }

        public static void storeInlineCode(string DestEXE, string code, CompilerSettings CS)
        {
            if (DestEXE.ToLower() == EXE.ToLower())
            {
                throw new Exception("You cannot embed code in the exe, that is running. Select a file that does not exists.");
            }
            if (hasInlineCode())
            {
                throw new Exception("You cannot do this with a compiler, that already contains code.");
            }
            if (File.Exists(DestEXE))
            {
                try
                {
                    File.Delete(DestEXE);
                }
                catch
                {
                    throw new Exception("Cannot create " + DestEXE);
                }
            }
            FileStream FS=File.Create(DestEXE);
            byte[] ASM = File.ReadAllBytes(EXE);

            //1. Recreate the code.
            FS.Write(ASM, 0, ASM.Length);
            //2. Write delimiter
            FS.Write(delim, 0, delim.Length);
            //3. Write Settings
            FS.Write(new byte[] { (byte)(CS.allowSave ? 1 : 0), (byte)(CS.exitEnd ? 1 : 0), (byte)(CS.runInstantly ? 1 : 0) }, 0, 3);
            //4. Write Code
            ASM = Encoding.UTF8.GetBytes(code);
            FS.Write(ASM, 0, ASM.Length);
            //Thats all, folks
            FS.Close();
            FS.Dispose();
        }
    }
}
