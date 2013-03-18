namespace ScriptEngine
{
    using System;
    using System.IO;

    internal class UserSource
    {
        public int EndLine;
        public string FileName;
        public string FilePath;
        public int StartLine;

        public UserSource(string filePath)
        {
            this.FilePath = filePath;
            this.FileName = Path.GetFileName(filePath);
        }
    }
}

