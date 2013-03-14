namespace Standard
{
    using System;

    internal class IntervalState : TimedState
    {
        public byte IntensityLevel;
        public int Remaining;

        public IntervalState(byte[,] eventValues, byte intensityLevel, IChannelEnumerable channels) : base(eventValues, channels)
        {
            this.Remaining = 0;
            this.IntensityLevel = intensityLevel;
        }

        public IntervalState(byte[,] eventValues, byte intensityLevel, IChannelEnumerable channels, int count) : base(eventValues, channels)
        {
            this.Remaining = count;
            this.IntensityLevel = intensityLevel;
        }
    }
}

