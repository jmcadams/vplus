using VixenPlus.Dialogs;

namespace VixenPlus {
    internal class TimerExecutor
    {
        private readonly ExecutingTimerControlDialog _controlDialog;

        public TimerExecutor()
        {
            _controlDialog = new ExecutingTimerControlDialog();
        }

        public void SpawnExecutorFor(Timer timer)
        {
            var context = new TimerContext(timer);
            _controlDialog.AddTimer(context);
        }
    }
}