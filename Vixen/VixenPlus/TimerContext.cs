using System;

namespace VixenPlus
{
	internal class TimerContext : IDisposable
	{
		public delegate void OnExecutionChange(TimerContext context);

		public delegate void OnExecutionEnd(TimerContext context);

		private static IExecution m_executionInterface;

		private readonly DateTime m_endDateTime;
		private readonly int m_executionContextHandle;
		private readonly Timer m_timer;
		private Host m_host;

		public TimerContext(Timer timer, Host host)
		{
			m_timer = timer;
			m_host = host;
			m_endDateTime =
				new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, m_timer.StartDateTime.Hour,
				             m_timer.StartDateTime.Minute, 0).Add(m_timer.TimerLength);
			if (m_executionInterface == null)
			{
				m_executionInterface = (IExecution) Interfaces.Available["IExecution"];
			}
			m_executionContextHandle = m_executionInterface.RequestContext(true, false, null);
			m_executionInterface.SetSynchronousProgramChangeHandler(m_executionContextHandle, ProgramChanged);
		}

		public DateTime EndDateTime
		{
			get { return m_endDateTime; }
		}

		public int ExecutionContextHandle
		{
			get { return m_executionContextHandle; }
		}

		public IExecution ExecutionInterface
		{
			get { return m_executionInterface; }
		}

		public bool Stopping { get; set; }

		public Timer Timer
		{
			get { return m_timer; }
		}

		public void Dispose()
		{
			m_executionInterface.ReleaseContext(m_executionContextHandle);
			GC.SuppressFinalize(this);
		}

		public event OnExecutionChange ExecutionChange;

		public event OnExecutionEnd ExecutionEnd;

		~TimerContext()
		{
			Dispose();
		}

		private void ProgramChanged(ProgramChange changeType)
		{
			switch (changeType)
			{
				case ProgramChange.SequenceChange:
					if (ExecutionChange != null)
					{
						ExecutionChange(this);
					}
					break;

				case ProgramChange.End:
					if (ExecutionEnd != null)
					{
						ExecutionEnd(this);
					}
					break;
			}
		}

		public override string ToString()
		{
			string str = m_executionInterface.LoadedProgram(m_executionContextHandle);
			string str2 = m_executionInterface.LoadedSequence(m_executionContextHandle);
			if (str.Length == 0)
			{
				return str2;
			}
			return string.Format("{0}: {1}", str, str2);
		}
	}
}