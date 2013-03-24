namespace VixenPlus
{
	public class OutputPlugin
	{
		private readonly bool m_enabled;
		private readonly int m_from;
		private readonly int m_id;
		private readonly string m_name;
		private readonly int m_to;

		public OutputPlugin(string name, int id, bool enabled, int from, int to)
		{
			m_name = name;
			m_id = id;
			m_enabled = enabled;
			m_from = from;
			m_to = to;
		}

		public int ChannelFrom
		{
			get { return m_from; }
		}

		public int ChannelTo
		{
			get { return m_to; }
		}

		public bool Enabled
		{
			get { return m_enabled; }
		}

		public int Id
		{
			get { return m_id; }
		}

		public string Name
		{
			get { return m_name; }
		}
	}
}