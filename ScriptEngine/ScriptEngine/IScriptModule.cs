namespace ScriptEngine
{
    using System;
    using Vixen;

    public interface IScriptModule
    {
        string GenerateSequenceCode(EventSequence sequence);

        string[] Imports { get; }

        string[] References { get; }
    }
}

