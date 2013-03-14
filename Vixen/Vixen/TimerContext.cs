namespace Vixen
{
    using System;
    using System.Runtime.CompilerServices;

    internal class TimerContext : IDisposable
    {
        private DateTime m_endDateTime;
        private int m_executionContextHandle;
        private static IExecution m_executionInterface = null;
        private Host m_host;
        private bool m_stopping;
        private Vixen.Timer m_timer;

        public event OnExecutionChange ExecutionChange;

        public event OnExecutionEnd ExecutionEnd;

        public TimerContext(Vixen.Timer timer, Host host)
        {
            this.m_timer = timer;
            this.m_host = host;
            this.m_endDateTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, this.m_timer.StartDateTime.Hour, this.m_timer.StartDateTime.Minute, 0).Add(this.m_timer.TimerLength);
            if (m_executionInterface == null)
            {
                m_executionInterface = (IExecution) Interfaces.Available["IExecution"];
            }
            this.m_executionContextHandle = m_executionInterface.RequestContext(true, false, null);
            m_executionInterface.SetSynchronousProgramChangeHandler(this.m_executionContextHandle, new ProgramChangeHandler(this.ProgramChanged));
        }

        public void Dispose()
        {
            m_executionInterface.ReleaseContext(this.m_executionContextHandle);
            GC.SuppressFinalize(this);
        }

        ~TimerContext()
        {
            this.Dispose();
        }

        private void ProgramChanged(ProgramChange changeType)
        {
            switch (changeType)
            {
                case ProgramChange.SequenceChange:
                    if (this.ExecutionChange != null)
                    {
                        this.ExecutionChange(this);
                    }
                    break;

                case ProgramChange.End:
                    if (this.ExecutionEnd != null)
                    {
                        this.ExecutionEnd(this);
                    }
                    break;
            }
        }

        public override string ToString()
        {
            string str = m_executionInterface.LoadedProgram(this.m_executionContextHandle);
            string str2 = m_executionInterface.LoadedSequence(this.m_executionContextHandle);
            if (str.Length == 0)
            {
                return str2;
            }
            return string.Format("{0}: {1}", str, str2);
        }

        public DateTime EndDateTime
        {
            get
            {
                return this.m_endDateTime;
            }
        }

        public int ExecutionContextHandle
        {
            get
            {
                return this.m_executionContextHandle;
            }
        }

        public IExecution ExecutionInterface
        {
            get
            {
                return m_executionInterface;
            }
        }

        public bool Stopping
        {
            get
            {
                return this.m_stopping;
            }
            set
            {
                this.m_stopping = value;
            }
        }

        public Vixen.Timer Timer
        {
            get
            {
                return this.m_timer;
            }
        }

        public delegate void OnExecutionChange(TimerContext context);

        public delegate void OnExecutionEnd(TimerContext context);
    }
}

