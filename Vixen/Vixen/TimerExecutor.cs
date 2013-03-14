namespace Vixen
{
    using System;

    internal class TimerExecutor
    {
        private ExecutingTimerControlDialog m_controlDialog;
        private Host m_host;

        public TimerExecutor(Host host)
        {
            this.m_host = host;
            this.m_controlDialog = new ExecutingTimerControlDialog();
        }

        public int GetExecutingTimerExecutionContextHandle(int executingTimerIndex)
        {
            TimerContext contextOf = this.m_controlDialog.GetContextOf(executingTimerIndex);
            if (contextOf == null)
            {
                return 0;
            }
            return contextOf.ExecutionContextHandle;
        }

        public void SpawnExecutorFor(Timer timer)
        {
            TimerContext context = new TimerContext(timer, this.m_host);
            this.m_controlDialog.AddTimer(context);
        }

        public int ExecutingTimerCount
        {
            get
            {
                return this.m_controlDialog.TimerCount;
            }
        }
    }
}

