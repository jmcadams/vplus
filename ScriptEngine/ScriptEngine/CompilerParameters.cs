namespace ScriptEngine
{
    using System;
    using System.Collections.Generic;

    public class CompilerParameters
    {
        public List<string> AssemblyReferences = new List<string>();
        public List<string> Imports = new List<string>();
        public List<string> ModuleReferences = new List<string>();
    }
}

