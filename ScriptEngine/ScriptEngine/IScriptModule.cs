namespace ScriptEngine
{
    using System;
    using VixenPlus;

    public interface IScriptModule
    {
        string GenerateSequenceCode(EventSequence sequence);

        string[] Imports { get; }

        string[] References { get; }
    }
}

