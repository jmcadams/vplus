namespace LedTriksUtil
{
    using System;

    public interface IFrameOutput
    {
        void Start();
        void Stop();
        void UpdateFrame(Frame frame);
    }
}

