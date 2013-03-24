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
			TimerContext contextOf = _controlDialog.GetContextOf(executingTimerIndex);
			if (contextOf == null)
			{
				return 0;
			}
			return contextOf.ExecutionContextHandle;
		}

		public void SpawnExecutorFor(Timer timer)
		{
			var context = new TimerContext(timer);
			_controlDialog.AddTimer(context);
		}
	}
}