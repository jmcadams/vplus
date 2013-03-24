namespace Vixen
{
	internal class TimerExecutor
	{
		private readonly ExecutingTimerControlDialog m_controlDialog;
		private readonly Host m_host;

		public TimerExecutor(Host host)
		{
			m_host = host;
			m_controlDialog = new ExecutingTimerControlDialog();
		}

		public int ExecutingTimerCount
		{
			get { return m_controlDialog.TimerCount; }
		}

		public int GetExecutingTimerExecutionContextHandle(int executingTimerIndex)
		{
			TimerContext contextOf = m_controlDialog.GetContextOf(executingTimerIndex);
			if (contextOf == null)
			{
				return 0;
			}
			return contextOf.ExecutionContextHandle;
		}

		public void SpawnExecutorFor(Timer timer)
		{
			var context = new TimerContext(timer, m_host);
			m_controlDialog.AddTimer(context);
		}
	}
}