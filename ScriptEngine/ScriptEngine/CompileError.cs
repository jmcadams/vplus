namespace ScriptEngine
{
    using System;

    public class CompileError
    {
        public string ErrorMessage;
        public string FileName;
        public int Line;

        internal CompileError(string fileName, int line, string errorMessage)
        {
            this.FileName = fileName;
            this.Line = line;
            this.ErrorMessage = errorMessage;
        }
    }
}

