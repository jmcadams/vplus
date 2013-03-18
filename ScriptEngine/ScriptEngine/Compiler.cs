namespace ScriptEngine
{
    using System;

    public class Compiler
    {
        public static ICompile CreateCompiler()
        {
            return new ScriptCompiler();
        }
    }
}

