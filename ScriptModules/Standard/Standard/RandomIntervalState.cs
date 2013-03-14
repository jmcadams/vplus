namespace Standard
{
    using System;

    internal class RandomIntervalState : IntervalState
    {
        public float SaturationLevel;

        public RandomIntervalState(byte[,] eventValues, IChannelEnumerable channels, float saturationLevel, byte intensityLevel, int count) : base(eventValues, intensityLevel, channels, count)
        {
            this.SaturationLevel = saturationLevel;
        }
    }
}

