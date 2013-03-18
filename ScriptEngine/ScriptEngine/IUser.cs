namespace ScriptEngine
{
    using System;
    using Vixen;

    public interface IUser : IDisposable
    {
        void ForceStop();
        void ScriptEnded();
        void ScriptStarting();

        HardwareUpdateDelegate HardwareUpdate { set; }

        bool Running { get; }

        EventSequence Sequence { set; }
    }
}

