using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace Vixen
{
	internal class Timer : IComparable<Timer>
	{
		private List<ReferenceRectF> m_displayBounds;
		private bool m_isExecuting;
		private string m_objectDisplayName;
		private string m_objectFileName;
		private TimeSpan m_objectLength;
		private ObjectType m_objectType;
		private RecurrenceType m_recurrence;
		private object m_recurrenceData;
		private DateTime m_recurrenceEnd;
		private DateTime m_recurrenceStart;
		private int m_repeatInterval;
		private DateTime m_startDateTime;
		private TimeSpan m_timerLength;

		public Timer()
		{
			m_objectType = ObjectType.Sequence;
			m_displayBounds = new List<ReferenceRectF>();
			m_isExecuting = false;
			LastExecution = DateTime.MinValue;
			NotValidUntil = DateTime.MinValue;
		}

		public Timer(XmlNode timerNode)
		{
			m_objectType = ObjectType.Sequence;
			m_displayBounds = new List<ReferenceRectF>();
			m_isExecuting = false;
			LastExecution = DateTime.MinValue;
			NotValidUntil = DateTime.MinValue;
			m_startDateTime = DateTime.Parse(timerNode["StartDateTime"].InnerText);
			m_timerLength = TimeSpan.Parse(timerNode["TimerLength"].InnerText);
			XmlNode node = timerNode["Item"];
			m_objectLength = TimeSpan.Parse(node.Attributes["length"].Value);
			m_objectType = (ObjectType) Enum.Parse(typeof (ObjectType), node.Attributes["type"].Value);
			ProgramFileName = node.InnerText;
			if (m_objectLength != m_timerLength)
			{
				m_repeatInterval = Convert.ToInt32(timerNode["RepeatInterval"].InnerText);
			}
			XmlNode node2 = timerNode["Recurrence"];
			if (node2 != null)
			{
				m_recurrence = (RecurrenceType) Enum.Parse(typeof (RecurrenceType), node2.Attributes["type"].Value);
				m_recurrenceStart = DateTime.Parse(node2["StartDate"].InnerText);
				m_recurrenceEnd = DateTime.Parse(node2["EndDate"].InnerText);
				switch (m_recurrence)
				{
					case RecurrenceType.Weekly:
						m_recurrenceData = Convert.ToInt32(node2["Data"].InnerText);
						break;

					case RecurrenceType.Monthly:
						m_recurrenceData = node2["Data"].InnerText;
						break;

					case RecurrenceType.Yearly:
						m_recurrenceData = DateTime.Parse(node2["Data"].InnerText);
						break;
				}
			}
		}

		public List<ReferenceRectF> DisplayBounds
		{
			get { return m_displayBounds; }
			set { m_displayBounds = value; }
		}

		public DateTime EndDate
		{
			get
			{
				DateTime time2 = m_startDateTime + m_timerLength;
				return time2.Date;
			}
		}

		public DateTime EndDateTime
		{
			get { return (m_startDateTime + m_timerLength); }
		}

		public TimeSpan EndTime
		{
			get { return (StartTime + m_timerLength); }
		}

		public bool IsExecuting
		{
			get { return m_isExecuting; }
			set { m_isExecuting = value; }
		}

		public DateTime LastExecution { get; set; }

		public DateTime NotValidUntil { get; set; }

		public TimeSpan ObjectLength
		{
			get { return m_objectLength; }
			set { m_objectLength = value; }
		}

		public ObjectType ObjectType
		{
			get { return m_objectType; }
			set
			{
				m_objectType = value;
				SetObjectPath();
			}
		}

		public string ProgramFileName
		{
			get { return m_objectFileName; }
			set
			{
				m_objectFileName = value;
				m_objectDisplayName = Path.GetFileName(value);
				SetObjectPath();
			}
		}

		public string ProgramName
		{
			get { return m_objectDisplayName; }
		}

		public RecurrenceType Recurrence
		{
			get { return m_recurrence; }
			set { m_recurrence = value; }
		}

		public object RecurrenceData
		{
			get { return m_recurrenceData; }
			set { m_recurrenceData = value; }
		}

		public DateTime RecurrenceEnd
		{
			get
			{
				if (m_recurrence == RecurrenceType.None)
				{
					return EndDateTime;
				}
				return m_recurrenceEnd;
			}
			set { m_recurrenceEnd = value; }
		}

		public DateTime RecurrenceEndDateTime
		{
			get
			{
				if (m_recurrence == RecurrenceType.None)
				{
					return EndDateTime;
				}
				return (m_recurrenceEnd + EndTime);
			}
		}

		public TimeSpan RecurrenceSpan
		{
			get
			{
				if (m_recurrence == RecurrenceType.None)
				{
					return m_timerLength;
				}
				return (m_recurrenceEnd - m_recurrenceStart);
			}
		}

		public DateTime RecurrenceStart
		{
			get
			{
				if (m_recurrence == RecurrenceType.None)
				{
					return m_startDateTime;
				}
				return m_recurrenceStart;
			}
			set { m_recurrenceStart = value; }
		}

		public DateTime RecurrenceStartDateTime
		{
			get
			{
				if (m_recurrence == RecurrenceType.None)
				{
					return StartDateTime;
				}
				return (m_recurrenceStart + StartTime);
			}
		}

		public int RepeatInterval
		{
			get { return m_repeatInterval; }
			set { m_repeatInterval = value; }
		}

		public DateTime StartDate
		{
			get { return m_startDateTime.Date; }
			set { m_startDateTime = new DateTime(value.Year, value.Month, value.Day, m_startDateTime.Hour, m_startDateTime.Minute, 0); }
		}

		public DateTime StartDateTime
		{
			get { return m_startDateTime; }
			set { m_startDateTime = value; }
		}

		public TimeSpan StartTime
		{
			get { return m_startDateTime.TimeOfDay; }
			set
			{
				m_startDateTime = new DateTime(m_startDateTime.Year, m_startDateTime.Month, m_startDateTime.Day, value.Hours,
				                               value.Minutes, 0);
			}
		}

		public TimeSpan TimerLength
		{
			get { return m_timerLength; }
			set { m_timerLength = value; }
		}

		public int CompareTo(Timer other)
		{
			return StartTime.CompareTo(other.StartTime);
		}

		public Timer Clone()
		{
			var timer = new Timer();
			timer.Copy(this);
			return timer;
		}

		public void Copy(Timer timer)
		{
			m_displayBounds = timer.m_displayBounds;
			m_objectLength = timer.m_objectLength;
			ObjectType = timer.ObjectType;
			ProgramFileName = timer.ProgramFileName;
			m_objectDisplayName = timer.m_objectDisplayName;
			m_recurrence = timer.m_recurrence;
			m_recurrenceEnd = timer.m_recurrenceEnd;
			m_recurrenceStart = timer.m_recurrenceStart;
			m_repeatInterval = timer.m_repeatInterval;
			m_startDateTime = timer.m_startDateTime;
			m_timerLength = timer.m_timerLength;
			m_recurrenceData = timer.m_recurrenceData;
			m_isExecuting = timer.m_isExecuting;
		}

		public void SaveToXml(XmlNode contextNode)
		{
			XmlNode node = Xml.SetNewValue(contextNode, "Timer", string.Empty);
			Xml.SetValue(node, "StartDateTime", m_startDateTime.ToString());
			Xml.SetValue(node, "TimerLength", m_timerLength.ToString());
			XmlNode node2 = Xml.SetValue(node, "Item", Path.GetFileName(m_objectFileName));
			Xml.SetAttribute(node2, "length", m_objectLength.ToString());
			Xml.SetAttribute(node2, "type", m_objectType.ToString());
			if (m_objectLength != m_timerLength)
			{
				Xml.SetValue(node, "RepeatInterval", m_repeatInterval.ToString());
			}
			if (m_recurrence != RecurrenceType.None)
			{
				XmlNode node3 = Xml.SetValue(node, "Recurrence", string.Empty);
				Xml.SetAttribute(node3, "type", m_recurrence.ToString());
				Xml.SetValue(node3, "StartDate", m_recurrenceStart.ToShortDateString());
				Xml.SetValue(node3, "EndDate", m_recurrenceEnd.ToShortDateString());
				switch (m_recurrence)
				{
					case RecurrenceType.Weekly:
						Xml.SetValue(node3, "Data", ((int) m_recurrenceData).ToString());
						break;

					case RecurrenceType.Monthly:
						Xml.SetValue(node3, "Data", (string) m_recurrenceData);
						break;

					case RecurrenceType.Yearly:
						Xml.SetValue(node3, "Data", ((DateTime) m_recurrenceData).ToString());
						break;
				}
			}
		}

		private void SetObjectPath()
		{
			if ((m_objectFileName != null) && (m_objectFileName != string.Empty))
			{
				string str = (m_objectType == ObjectType.Program) ? Paths.ProgramPath : Paths.SequencePath;
				m_objectFileName = Path.Combine(str, Path.GetFileName(m_objectFileName));
			}
		}

		public override string ToString()
		{
			var builder = new StringBuilder();
			builder.Append(m_objectDisplayName);
			if (m_timerLength != m_objectLength)
			{
				builder.Append("|Repeats");
				if (m_repeatInterval != 0)
				{
					builder.AppendFormat(" every {0} minutes", m_repeatInterval);
				}
			}
			if (m_recurrence != RecurrenceType.None)
			{
				builder.AppendFormat("|Recurs on a {0} basis", m_recurrence.ToString().ToLower());
			}
			return builder.ToString();
		}
	}
}