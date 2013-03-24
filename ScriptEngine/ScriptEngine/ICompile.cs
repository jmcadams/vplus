namespace ScriptEngine
{
    using System;
    using System.Reflection;
    using VixenPlus;

    public interface ICompile
    {
        void ClearFlag(int flag);
        bool Compile(EventSequence sequence);
        void SetFlag(int flag);

        Assembly CompiledAssembly { get; }

        CompileError[] Errors { get; }

        string TypeName { get; }
    }
}

