using System;
using System.Diagnostics;

using FMOD;

namespace VixenPlus {
    internal class EngineContext
    {
        public byte[] ChannelMask;
        public EventSequence CurrentSequence;
        public byte[,] Data;
        public int LastIndex = -1;
        public byte[] LastPeriod;
        public int MaxEvent = Int32.MaxValue;
        public RouterContext RouterContext;
        public int SequenceTickLength;
        public SoundChannel SoundChannel;
        public int StartOffset;
        public int TickCount;
        public readonly Stopwatch Timekeeper = new Stopwatch();
    }
}