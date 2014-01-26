using System;

internal class EngineTimer : ITickSource, IDisposable
{
    private TickCallDelegate _tickCall;

    internal EngineTimer(TickCallDelegate tickCall)
    {
        _tickCall = tickCall.Invoke;
    }

    public void Dispose()
    {
        _tickCall = null;
        GC.SuppressFinalize(this);
    }

    public int Milliseconds
    {
        get { return _tickCall(); }
    }

    ~EngineTimer()
    {
        Dispose();
    }

    internal delegate int TickCallDelegate();
}