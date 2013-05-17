namespace VixenPlus
{
    internal class TimerExecutor
    {
        private readonly ExecutingTimerControlDialog _controlDialog;

        public TimerExecutor()
        {
            _controlDialog = new ExecutingTimerControlDialog();
        }

        public int ExecutingTimerCount
        {
            get { return _controlDialog.TimerCount; }
        }

        public int GetExecutingTimerExecutionContextHandle(int executingTimerIndex)
        {
            var contextOf = _controlDialog.GetContextOf(executingTimerIndex);
            return contextOf == null ? 0 : contextOf.ExecutionContextHandle;
        }

        public void SpawnExecutorFor(Timer timer)
        {
            var context = new TimerContext(timer);
            _controlDialog.AddTimer(context);
        }
    }
}