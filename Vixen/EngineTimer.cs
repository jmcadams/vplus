using System;

internal class EngineTimer : ITickSource, IDisposable
{
    // ReSharper disable once UnusedAutoPropertyAccessor.Local
    private TickCallDelegate TickCall { get; set; }


    internal EngineTimer(TickCallDelegate tickCall)
    {
        TickCall = tickCall.Invoke;
    }

    public void Dispose()
    {
        TickCall = null;
        GC.SuppressFinalize(this);
    }

/*
    public int Milliseconds
    {
        get { return _tickCall(); }
    }
*/

    ~EngineTimer()
    {
        Dispose();
    }

    internal delegate int TickCallDelegate();
}