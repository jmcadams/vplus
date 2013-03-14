namespace Standard
{
    using System;
    using System.Threading;
    using System.Timers;

    internal class BlockingTimer
    {
        private ManualResetEvent m_resetEvent = null;
        private TimedState m_state;
        private System.Timers.Timer m_timer;

        public BlockingTimer(int interval, ElapsedEventHandler elapsedHandler, TimedState state)
        {
            this.m_timer = new System.Timers.Timer((double) interval);
            this.m_timer.Elapsed += elapsedHandler;
            this.m_state = state;
        }

        public ManualResetEvent Block()
        {
            if (this.m_resetEvent == null)
            {
                this.m_resetEvent = new ManualResetEvent(false);
            }
            else
            {
                this.m_resetEvent.Reset();
            }
            return this.m_resetEvent;
        }

        public void Start()
        {
            this.m_timer.Start();
        }

        public void Stop()
        {
            this.m_timer.Stop();
            this.m_timer.Dispose();
            if (this.m_resetEvent != null)
            {
                if (!this.m_resetEvent.SafeWaitHandle.IsClosed)
                {
                    this.m_resetEvent.Set();
                }
                this.m_resetEvent.Close();
            }
        }

        public TimedState State
        {
            get
            {
                return this.m_state;
            }
        }

        public System.Timers.Timer Timer
        {
            get
            {
                return this.m_timer;
            }
        }
    }
}

