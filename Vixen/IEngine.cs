using System;
using System.Xml;

public interface IEngine : IDisposable
{
    XmlDocument CommDoc { set; }

    HardwareUpdateDelegate HardwareUpdate { set; }

    bool IsRunning { get; }
    event OnEngineError EngineError;

    event EventHandler EngineStopped;

    void Initialize(EventSequence sequence);
    void Pause();
    bool Play();
    void Stop();
}