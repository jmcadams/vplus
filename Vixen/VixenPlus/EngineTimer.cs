using System;

namespace Vixen
{
	internal class EngineTimer : ITickSource, IDisposable
	{
		private TickCallDelegate m_tickCall;

		internal EngineTimer(TickCallDelegate tickCall)
		{
			m_tickCall = tickCall.Invoke;
		}

		public void Dispose()
		{
			m_tickCall = null;
			GC.SuppressFinalize(this);
		}

		public int Milliseconds
		{
			get { return m_tickCall(); }
		}

		~EngineTimer()
		{
			Dispose();
		}

		internal delegate int TickCallDelegate();
	}
}