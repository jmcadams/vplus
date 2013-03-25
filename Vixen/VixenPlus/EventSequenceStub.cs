using System;
using System.IO;

namespace VixenPlus
{
	internal class EventSequenceStub : IDisposable
	{
		private EventSequence _eventSequence;
		private string _filename;
		private int _length;
		private string _lengthString;

		public EventSequenceStub(EventSequence sequence)
		{
			_filename = string.Empty;
			_length = 0;
			_lengthString = string.Empty;
			AudioName = string.Empty;
			AudioFileName = string.Empty;
			_eventSequence = null;
			_filename = sequence.FileName;
			Length = sequence.Time;
			if (sequence.Audio != null)
			{
				AudioName = sequence.Audio.Name;
				AudioFileName = sequence.Audio.FileName;
			}
			_eventSequence = sequence;
			Mask = _eventSequence.Mask;
		}

		public EventSequenceStub(string fileName, bool referenceSequence)
		{
			_filename = string.Empty;
			_length = 0;
			_lengthString = string.Empty;
			AudioName = string.Empty;
			AudioFileName = string.Empty;
			_eventSequence = null;
			var sequence = new EventSequence(fileName);
			_filename = sequence.FileName;
			Length = sequence.Time;
			if (sequence.Audio != null)
			{
				AudioName = sequence.Audio.Name;
				AudioFileName = sequence.Audio.FileName;
			}
			Mask = sequence.Mask;
			if (referenceSequence)
			{
				_eventSequence = sequence;
			}
			else
			{
				sequence.Dispose();
			}
		}

		public string AudioFileName { get; set; }

		public string AudioName { get; set; }

		public string FileName
		{
			get { return _filename; }
		}

		public int Length
		{
			get { return _length; }
			set
			{
				_length = value;
				_lengthString = string.Format("{0}:{1:d2}", _length/60000, (_length%60000)/1000);
			}
		}

		public string LengthString
		{
			get { return _lengthString; }
		}

		public byte[][] Mask { get; set; }

		public string Name
		{
			get { return Path.GetFileNameWithoutExtension(_filename); }
			set { _filename = Path.ChangeExtension(value, ".vpr"); }
		}

		public EventSequence Sequence
		{
			get { return _eventSequence; }
		}

		public void Dispose()
		{
			Dispose(true);
		}

		public void Dispose(bool disposing)
		{
			if (_eventSequence != null)
			{
				_eventSequence.Dispose();
				_eventSequence = null;
			}
			GC.SuppressFinalize(this);
		}

		~EventSequenceStub()
		{
			Dispose(false);
		}

		public EventSequence RetrieveSequence()
		{
			return _eventSequence ?? (_eventSequence = new EventSequence(_filename));
		}

		public override string ToString()
		{
			return string.Format("{0} ({1})", Name, _lengthString);
		}
	}
}