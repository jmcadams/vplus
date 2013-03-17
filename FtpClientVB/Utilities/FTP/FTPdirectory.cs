namespace Utilities.FTP
{
    using Microsoft.VisualBasic.CompilerServices;
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;

    [OptionText]
    public class FTPdirectory : List<FTPfileInfo>
    {
        private const char slash = '/';

        public FTPdirectory()
        {
        }

        public FTPdirectory(string dir, string path)
        {
            foreach (string str in dir.Replace("\n", "").Split(new char[] { '\r' }))
            {
                if (Operators.CompareString(str, "", true) != 0)
                {
                    this.Add(new FTPfileInfo(str, path));
                }
            }
        }

        public bool FileExists(string filename)
        {
            foreach (FTPfileInfo info in this)
            {
                if (Operators.CompareString(info.Filename, filename, true) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        public FTPdirectory GetDirectories()
        {
            return this.GetFileOrDir(FTPfileInfo.DirectoryEntryTypes.Directory, "");
        }

        private FTPdirectory GetFileOrDir(FTPfileInfo.DirectoryEntryTypes type, string ext = "")
        {
            FTPdirectory pdirectory2 = new FTPdirectory();
            foreach (FTPfileInfo info in this)
            {
                if (info.FileType == type)
                {
                    if (Operators.CompareString(ext, "", true) == 0)
                    {
                        pdirectory2.Add(info);
                    }
                    else if (Operators.CompareString(ext, info.Extension, true) == 0)
                    {
                        pdirectory2.Add(info);
                    }
                }
            }
            return pdirectory2;
        }

        public FTPdirectory GetFiles(string ext = "")
        {
            return this.GetFileOrDir(FTPfileInfo.DirectoryEntryTypes.File, ext);
        }

        public static string GetParentDirectory(string dir)
        {
            string str2 = dir.TrimEnd(new char[] { '/' });
            int num = str2.LastIndexOf('/');
            if (num <= 0)
            {
                throw new ApplicationException("No parent for root");
            }
            return str2.Substring(0, num - 1);
        }
    }
}

