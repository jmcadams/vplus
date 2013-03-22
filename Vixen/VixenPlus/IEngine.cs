namespace Vixen
{
    using System;
    using System.Runtime.CompilerServices;
    using System.Xml;

    public interface IEngine : IDisposable
    {
        event OnEngineError EngineError;

        event EventHandler EngineStopped;

        void Initialize(EventSequence sequence);
        void Pause();
        bool Play();
        void Stop();

        XmlDocument CommDoc { set; }

        HardwareUpdateDelegate HardwareUpdate { set; }

        bool IsRunning { get; }
    }
}

