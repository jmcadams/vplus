namespace Standard
{
    using System;

    internal class TimeFrameState : TimedState
    {
        public int CurrentIndex;
        public int EventColumnCount;

        public TimeFrameState(byte[,] eventValues, IChannelEnumerable channels) : base(eventValues, channels)
        {
            this.CurrentIndex = 0;
            this.EventColumnCount = eventValues.GetLength(1);
        }
    }
}

