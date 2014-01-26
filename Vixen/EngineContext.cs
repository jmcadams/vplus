using System.Diagnostics;

using FMOD;

internal class EngineContext
{
    public byte[] ChannelMask = null;
    public EventSequence CurrentSequence = null;
    public byte[,] Data;
    public int FadeStartTickCount;
    public int LastIndex = -1;
    public byte[] LastPeriod = null;
    public int MaxEvent = 2147483647;
    public RouterContext RouterContext;
    public int SequenceIndex = 0;
    public int SequenceTickLength;
    public SoundChannel SoundChannel;
    public int StartOffset = 0;
    public int TickCount = 0;
    public Stopwatch Timekeeper = new Stopwatch();
}