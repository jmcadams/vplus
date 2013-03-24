using System.Diagnostics;
using FMOD;

namespace Vixen
{
	internal class EngineContext
	{
		public byte[] m_channelMask = null;
		public EventSequence m_currentSequence = null;
		public byte[,] m_data;
		public int m_fadeStartTickCount;
		public int m_lastIndex = -1;
		public byte[] m_lastPeriod = null;
		public int m_maxEvent = 0x7fffffff;
		public RouterContext m_routerContext;
		public int m_sequenceIndex = 0;
		public int m_sequenceTickLength;
		public SoundChannel m_soundChannel;
		public int m_startOffset = 0;
		public int m_tickCount = 0;
		public Stopwatch m_timekeeper = new Stopwatch();
	}
}