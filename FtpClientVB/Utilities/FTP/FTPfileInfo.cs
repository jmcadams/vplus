namespace Utilities.FTP
{
    using Microsoft.VisualBasic;
    using Microsoft.VisualBasic.CompilerServices;
    using System;
    using System.Text.RegularExpressions;

    [OptionText]
    public class FTPfileInfo
    {
        private DateTime _fileDateTime;
        private string _filename;
        private DirectoryEntryTypes _fileType;
        private static string[] _ParseFormats = new string[] { @"(?<dir>[\-d])(?<permission>([\-r][\-w][\-xs]){3})\s+\d+\s+\w+\s+\w+\s+(?<size>\d+)\s+(?<timestamp>\w+\s+\d+\s+\d{4})\s+(?<name>.+)", @"(?<dir>[\-d])(?<permission>([\-r][\-w][\-xs]){3})\s+\d+\s+\d+\s+(?<size>\d+)\s+(?<timestamp>\w+\s+\d+\s+\d{4})\s+(?<name>.+)", @"(?<dir>[\-d])(?<permission>([\-r][\-w][\-xs]){3})\s+\d+\s+\d+\s+(?<size>\d+)\s+(?<timestamp>\w+\s+\d+\s+\d{1,2}:\d{2})\s+(?<name>.+)", @"(?<dir>[\-d])(?<permission>([\-r][\-w][\-xs]){3})\s+\d+\s+\w+\s+\w+\s+(?<size>\d+)\s+(?<timestamp>\w+\s+\d+\s+\d{1,2}:\d{2})\s+(?<name>.+)", @"(?<dir>[\-d])(?<permission>([\-r][\-w][\-xs]){3})(\s+)(?<size>(\d+))(\s+)(?<ctbit>(\w+\s\w+))(\s+)(?<size2>(\d+))\s+(?<timestamp>\w+\s+\d+\s+\d{2}:\d{2})\s+(?<name>.+)", @"(?<timestamp>\d{2}\-\d{2}\-\d{2}\s+\d{2}:\d{2}[Aa|Pp][mM])\s+(?<dir>\<\w+\>){0,1}(?<size>\d+){0,1}\s+(?<name>.+)" };
        private string _path;
        private string _permission;
        private long _size;

        public FTPfileInfo(string line, string path)
        {
            Match matchingRegex = this.GetMatchingRegex(line);
            if (matchingRegex == null)
            {
                throw new ApplicationException("Unable to parse line: " + line);
            }
            this._filename = matchingRegex.Groups["name"].Value;
            this._path = path;
            this._size = Conversions.ToLong(matchingRegex.Groups["size"].Value);
            this._permission = matchingRegex.Groups["permission"].Value;
            string left = matchingRegex.Groups["dir"].Value;
            if ((Operators.CompareString(left, "", true) != 0) & (Operators.CompareString(left, "-", true) != 0))
            {
                this._fileType = DirectoryEntryTypes.Directory;
            }
            else
            {
                this._fileType = DirectoryEntryTypes.File;
            }
            try
            {
                this._fileDateTime = DateTime.Parse(matchingRegex.Groups["timestamp"].Value);
            }
            catch (Exception exception1)
            {
                ProjectData.SetProjectError(exception1);
                Exception exception = exception1;
                this._fileDateTime = new DateTime();
                ProjectData.ClearProjectError();
            }
        }

        private Match GetMatchingRegex(string line)
        {
            int num2 = _ParseFormats.Length - 1;
            for (int i = 0; i <= num2; i++)
            {
                Match match2 = new Regex(_ParseFormats[i]).Match(line);
                if (match2.Success)
                {
                    return match2;
                }
            }
            return null;
        }

        public string Extension
        {
            get
            {
                int num = this.Filename.LastIndexOf(".");
                if ((num >= 0) & (num < (this.Filename.Length - 1)))
                {
                    return this.Filename.Substring(num + 1);
                }
                return "";
            }
        }

        public DateTime FileDateTime
        {
            get
            {
                return this._fileDateTime;
            }
            internal set
            {
                this._fileDateTime = value;
            }
        }

        public string Filename
        {
            get
            {
                return this._filename;
            }
        }

        public DirectoryEntryTypes FileType
        {
            get
            {
                return this._fileType;
            }
        }

        public string FullName
        {
            get
            {
                return (this.Path + this.Filename);
            }
        }

        public string NameOnly
        {
            get
            {
                int length = this.Filename.LastIndexOf(".");
                if (length > 0)
                {
                    return this.Filename.Substring(0, length);
                }
                return this.Filename;
            }
        }

        public string Path
        {
            get
            {
                return Conversions.ToString(Operators.ConcatenateObject(this._path, Interaction.IIf(this._path.EndsWith("/"), "", "/")));
            }
        }

        public string Permission
        {
            get
            {
                return this._permission;
            }
        }

        public long Size
        {
            get
            {
                return this._size;
            }
        }

        [OptionText]
        public enum DirectoryEntryTypes
        {
            File,
            Directory
        }
    }
}

