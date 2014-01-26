using System.Diagnostics;

using FMOD;

internal class EngineContext
{
    public byte[] ChannelMask;
    public EventSequence CurrentSequence;
    public byte[,] Data;
    public int FadeStartTickCount;
    public int LastIndex = -1;
    public byte[] LastPeriod;
    public int MaxEvent = 2147483647;
    public RouterContext RouterContext;
    public int SequenceIndex;
    public int SequenceTickLength;
    public SoundChannel SoundChannel;
    public int StartOffset;
    public int TickCount;
    public readonly Stopwatch Timekeeper = new Stopwatch();
}