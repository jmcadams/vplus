using System;
using System.IO;

namespace Vixen
{
	internal class EventSequenceStub : IDisposable
	{
		private EventSequence m_eventSequence;
		private string m_filename;
		private int m_length;
		private string m_lengthString;

		public EventSequenceStub(EventSequence sequence)
		{
			m_filename = string.Empty;
			m_length = 0;
			m_lengthString = string.Empty;
			AudioName = string.Empty;
			AudioFileName = string.Empty;
			m_eventSequence = null;
			m_filename = sequence.FileName;
			Length = sequence.Time;
			if (sequence.Audio != null)
			{
				AudioName = sequence.Audio.Name;
				AudioFileName = sequence.Audio.FileName;
			}
			m_eventSequence = sequence;
			Mask = m_eventSequence.Mask;
		}

		public EventSequenceStub(string fileName, bool referenceSequence)
		{
			m_filename = string.Empty;
			m_length = 0;
			m_lengthString = string.Empty;
			AudioName = string.Empty;
			AudioFileName = string.Empty;
			m_eventSequence = null;
			var sequence = new EventSequence(fileName);
			m_filename = sequence.FileName;
			Length = sequence.Time;
			if (sequence.Audio != null)
			{
				AudioName = sequence.Audio.Name;
				AudioFileName = sequence.Audio.FileName;
			}
			Mask = sequence.Mask;
			if (referenceSequence)
			{
				m_eventSequence = sequence;
			}
			else
			{
				sequence.Dispose();
				sequence = null;
			}
		}

		public string AudioFileName { get; set; }

		public string AudioName { get; set; }

		public string FileName
		{
			get { return m_filename; }
		}

		public int Length
		{
			get { return m_length; }
			set
			{
				m_length = value;
				m_lengthString = string.Format("{0}:{1:d2}", m_length/0xea60, (m_length%0xea60)/0x3e8);
			}
		}

		public string LengthString
		{
			get { return m_lengthString; }
		}

		public byte[][] Mask { get; set; }

		public string Name
		{
			get { return Path.GetFileNameWithoutExtension(m_filename); }
			set { m_filename = Path.ChangeExtension(value, ".vpr"); }
		}

		public EventSequence Sequence
		{
			get { return m_eventSequence; }
		}

		public void Dispose()
		{
			Dispose(true);
		}

		public void Dispose(bool disposing)
		{
			if (m_eventSequence != null)
			{
				m_eventSequence.Dispose();
				m_eventSequence = null;
			}
			GC.SuppressFinalize(this);
		}

		~EventSequenceStub()
		{
			Dispose(false);
		}

		public EventSequence RetrieveSequence()
		{
			if (m_eventSequence == null)
			{
				m_eventSequence = new EventSequence(m_filename);
			}
			return m_eventSequence;
		}

		public override string ToString()
		{
			return string.Format("{0} ({1})", Name, m_lengthString);
		}
	}
}