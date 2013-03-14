namespace Vixen
{
    using System;
    using System.Runtime.CompilerServices;

    internal class EngineTimer : ITickSource, IDisposable
    {
        private TickCallDelegate m_tickCall;

        internal EngineTimer(TickCallDelegate tickCall)
        {
            this.m_tickCall = new TickCallDelegate(tickCall.Invoke);
        }

        public void Dispose()
        {
            this.m_tickCall = null;
            GC.SuppressFinalize(this);
        }

        ~EngineTimer()
        {
            this.Dispose();
        }

        public int Milliseconds
        {
            get
            {
                return this.m_tickCall();
            }
        }

        internal delegate int TickCallDelegate();
    }
}

