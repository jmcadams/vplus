namespace Standard
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Timers;
    using Vixen;

    public class Context
    {
        private short[] m_channelAffects;
        private byte[] m_channelBuffer;
        public ChannelCollection m_channels = new ChannelCollection();
        public bool m_clearAfterCommand = false;
        public int m_eventPeriodLength = 100;
        private HardwareUpdateDelegate m_hardwareUpdate;
        public Dictionary<ModifierType, int> m_modifiers = new Dictionary<ModifierType, int>();
        private int m_refCount = 0;
        private RunState m_runState = RunState.Stopped;
        private Dictionary<System.Timers.Timer, BlockingTimer> m_timeFrameTimers = new Dictionary<System.Timers.Timer, BlockingTimer>();

        public Context()
        {
            this.State = RunState.Running;
        }

        public void AffectChannel(int channelIndex, byte value)
        {
            this.m_channelAffects[channelIndex] = value;
        }

        public void BeginUpdate()
        {
            this.m_refCount++;
        }

        private void ClearChannels(IChannelEnumerable channels)
        {
            this.BeginUpdate();
            for (int i = 0; i < channels.Count; i++)
            {
                this.AffectChannel(channels[i].OutputChannel, 0);
            }
            this.EndUpdate();
        }

        internal BlockingTimer CreateTimer(int interval, IntervalState state)
        {
            if (this.State != RunState.Running)
            {
                return null;
            }
            lock (this.m_timeFrameTimers)
            {
                BlockingTimer timer = new BlockingTimer(interval, new ElapsedEventHandler(this.IntervalTimerElapsed), state);
                this.m_timeFrameTimers[timer.Timer] = timer;
                if (state is RandomIntervalState)
                {
                    this.OutputRandom((RandomIntervalState) state);
                }
                else
                {
                    this.OutputValue(state);
                }
                timer.Start();
                return timer;
            }
        }

        internal BlockingTimer CreateTimer(int interval, TimeFrameState state)
        {
            if (this.State != RunState.Running)
            {
                return null;
            }
            lock (this.m_timeFrameTimers)
            {
                BlockingTimer timer = new BlockingTimer(interval, new ElapsedEventHandler(this.TimeFrameTimerElapsed), state);
                this.m_timeFrameTimers[timer.Timer] = timer;
                this.NextEvent((TimeFrameState) timer.State);
                timer.Start();
                return timer;
            }
        }

        public void Dispose()
        {
            this.Stop();
            GC.SuppressFinalize(this);
        }

        private void EndTimer(BlockingTimer blockingTimer)
        {
            if (this.State == RunState.Running)
            {
                lock (this.m_timeFrameTimers)
                {
                    if (this.m_clearAfterCommand)
                    {
                        this.ClearChannels(blockingTimer.State.Channels);
                    }
                    this.m_timeFrameTimers.Remove(blockingTimer.Timer);
                    blockingTimer.Stop();
                    blockingTimer.Timer.Dispose();
                }
            }
        }

        public void EndUpdate()
        {
            if (this.State == RunState.Running)
            {
                bool flag = false;
                for (int i = 0; i < this.m_channelAffects.Length; i++)
                {
                    int num = this.m_channelAffects[i];
                    if (num != -1)
                    {
                        flag = true;
                        this.m_channelBuffer[i] = (byte) num;
                        this.m_channelAffects[i] = -1;
                    }
                }
                if ((this.m_refCount > 0) && ((((--this.m_refCount == 0) && flag) && (this.m_hardwareUpdate != null)) && ((Thread.CurrentThread.ThreadState & ThreadState.AbortRequested) == ThreadState.Running)))
                {
                    lock (this.m_hardwareUpdate)
                    {
                        this.m_hardwareUpdate(this.m_channelBuffer);
                    }
                }
            }
        }

        ~Context()
        {
            this.Dispose();
        }

        private void IntervalTimerElapsed(object sender, ElapsedEventArgs e)
        {
            System.Timers.Timer key = (System.Timers.Timer) sender;
            BlockingTimer timer2 = null;
            if (this.m_timeFrameTimers.TryGetValue(key, out timer2))
            {
                IntervalState state = (IntervalState) timer2.State;
                if (state is RandomIntervalState)
                {
                    this.OutputRandom((RandomIntervalState) state);
                }
                else
                {
                    this.OutputValue(state);
                }
                if ((state.Remaining > 0) && (--state.Remaining == 0))
                {
                    this.EndTimer(timer2);
                }
            }
        }

        private bool NextEvent(TimeFrameState state)
        {
            if (this.State != RunState.Running)
            {
                return false;
            }
            if (state.CurrentIndex == state.EventColumnCount)
            {
                return false;
            }
            this.BeginUpdate();
            for (int i = 0; i < state.Channels.Count; i++)
            {
                this.AffectChannel(state.Channels[i].OutputChannel, state.EventValues[i, state.CurrentIndex]);
            }
            this.EndUpdate();
            state.CurrentIndex++;
            return true;
        }

        internal void OutputRandom(RandomIntervalState state)
        {
            this.OutputRandom(state.EventValues, state.SaturationLevel, state.IntensityLevel);
        }

        public void OutputRandom(byte[,] eventValues, float saturationLevel, byte intensityLevel)
        {
            Actions.GenerateRandomValues(eventValues, saturationLevel, intensityLevel);
            int length = eventValues.GetLength(0);
            this.BeginUpdate();
            for (int i = 0; i < length; i++)
            {
                this.AffectChannel(this.m_channels[i].OutputChannel, eventValues[i, 0]);
            }
            this.EndUpdate();
        }

        internal void OutputValue(IntervalState state)
        {
            int length = state.EventValues.GetLength(0);
            int num2 = state.EventValues.GetLength(1);
            this.BeginUpdate();
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < num2; j++)
                {
                    state.EventValues[i, j] = state.IntensityLevel;
                }
                this.AffectChannel(state.Channels[i].OutputChannel, state.EventValues[i, 0]);
            }
            this.EndUpdate();
        }

        public void Stop()
        {
            if (this.State == RunState.Running)
            {
                this.State = RunState.Stopping;
            }
        }

        private void TimeFrameTimerElapsed(object sender, ElapsedEventArgs e)
        {
            System.Timers.Timer key = (System.Timers.Timer) sender;
            BlockingTimer timer2 = null;
            if (this.m_timeFrameTimers.TryGetValue(key, out timer2) && !this.NextEvent((TimeFrameState) timer2.State))
            {
                this.EndTimer(timer2);
            }
        }

        internal void WaitOn(BlockingTimer timer)
        {
            timer.Block().WaitOne();
        }

        internal void WaitOnAny()
        {
            while ((this.State == RunState.Running) && (this.m_timeFrameTimers.Count > 0))
            {
                System.Timers.Timer[] array = new System.Timers.Timer[this.m_timeFrameTimers.Count];
                this.m_timeFrameTimers.Keys.CopyTo(array, 0);
                this.WaitOn(this.m_timeFrameTimers[array[0]]);
            }
        }

        public HardwareUpdateDelegate HardwareUpdate
        {
            set
            {
                this.m_hardwareUpdate = value;
            }
        }

        public EventSequence Sequence
        {
            set
            {
                this.m_channels.Clear();
                foreach (Channel channel in value.Channels)
                {
                    this.m_channels.Add(new VixenChannel(channel));
                }
                this.m_channelBuffer = new byte[this.m_channels.Count];
                this.m_channelAffects = new short[this.m_channels.Count];
            }
        }

        public RunState State
        {
            get
            {
                return this.m_runState;
            }
            set
            {
                this.m_runState = value;
                if (value == RunState.Stopping)
                {
                    lock (this.m_timeFrameTimers)
                    {
                        foreach (BlockingTimer timer in this.m_timeFrameTimers.Values)
                        {
                            timer.Stop();
                        }
                        this.m_timeFrameTimers.Clear();
                    }
                    this.m_runState = RunState.Stopped;
                }
            }
        }

        public enum RunState
        {
            Running,
            Stopping,
            Stopped
        }
    }
}

