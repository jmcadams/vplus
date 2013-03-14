namespace Standard
{
    using System;

    public interface ITimeModifier : IModifier
    {
        uint Hour { get; }

        uint Hours { get; }

        uint Millisecond { get; }

        uint Milliseconds { get; }

        uint Minute { get; }

        uint Minutes { get; }

        uint Second { get; }

        uint Seconds { get; }
    }
}

