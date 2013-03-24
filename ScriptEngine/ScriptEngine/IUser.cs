namespace ScriptEngine
{
    using System;
    using VixenPlus;

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

