namespace Vixen
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Xml;

    internal class Timer : IComparable<Timer>
    {
        private List<ReferenceRectF> m_displayBounds;
        private bool m_isExecuting;
        private DateTime m_lastExecution;
        private DateTime m_notValidUntil;
        private string m_objectDisplayName;
        private string m_objectFileName;
        private TimeSpan m_objectLength;
        private Vixen.ObjectType m_objectType;
        private RecurrenceType m_recurrence;
        private object m_recurrenceData;
        private DateTime m_recurrenceEnd;
        private DateTime m_recurrenceStart;
        private int m_repeatInterval;
        private DateTime m_startDateTime;
        private TimeSpan m_timerLength;

        public Timer()
        {
            this.m_objectType = Vixen.ObjectType.Sequence;
            this.m_displayBounds = new List<ReferenceRectF>();
            this.m_isExecuting = false;
            this.m_lastExecution = DateTime.MinValue;
            this.m_notValidUntil = DateTime.MinValue;
        }

        public Timer(XmlNode timerNode)
        {
            this.m_objectType = Vixen.ObjectType.Sequence;
            this.m_displayBounds = new List<ReferenceRectF>();
            this.m_isExecuting = false;
            this.m_lastExecution = DateTime.MinValue;
            this.m_notValidUntil = DateTime.MinValue;
            this.m_startDateTime = DateTime.Parse(timerNode["StartDateTime"].InnerText);
            this.m_timerLength = TimeSpan.Parse(timerNode["TimerLength"].InnerText);
            XmlNode node = timerNode["Item"];
            this.m_objectLength = TimeSpan.Parse(node.Attributes["length"].Value);
            this.m_objectType = (Vixen.ObjectType) Enum.Parse(typeof(Vixen.ObjectType), node.Attributes["type"].Value);
            this.ProgramFileName = node.InnerText;
            if (this.m_objectLength != this.m_timerLength)
            {
                this.m_repeatInterval = Convert.ToInt32(timerNode["RepeatInterval"].InnerText);
            }
            XmlNode node2 = timerNode["Recurrence"];
            if (node2 != null)
            {
                this.m_recurrence = (RecurrenceType) Enum.Parse(typeof(RecurrenceType), node2.Attributes["type"].Value);
                this.m_recurrenceStart = DateTime.Parse(node2["StartDate"].InnerText);
                this.m_recurrenceEnd = DateTime.Parse(node2["EndDate"].InnerText);
                switch (this.m_recurrence)
                {
                    case RecurrenceType.Weekly:
                        this.m_recurrenceData = Convert.ToInt32(node2["Data"].InnerText);
                        break;

                    case RecurrenceType.Monthly:
                        this.m_recurrenceData = node2["Data"].InnerText;
                        break;

                    case RecurrenceType.Yearly:
                        this.m_recurrenceData = DateTime.Parse(node2["Data"].InnerText);
                        break;
                }
            }
        }

        public Timer Clone()
        {
            Timer timer = new Timer();
            timer.Copy(this);
            return timer;
        }

        public int CompareTo(Timer other)
        {
            return this.StartTime.CompareTo(other.StartTime);
        }

        public void Copy(Timer timer)
        {
            this.m_displayBounds = timer.m_displayBounds;
            this.m_objectLength = timer.m_objectLength;
            this.ObjectType = timer.ObjectType;
            this.ProgramFileName = timer.ProgramFileName;
            this.m_objectDisplayName = timer.m_objectDisplayName;
            this.m_recurrence = timer.m_recurrence;
            this.m_recurrenceEnd = timer.m_recurrenceEnd;
            this.m_recurrenceStart = timer.m_recurrenceStart;
            this.m_repeatInterval = timer.m_repeatInterval;
            this.m_startDateTime = timer.m_startDateTime;
            this.m_timerLength = timer.m_timerLength;
            this.m_recurrenceData = timer.m_recurrenceData;
            this.m_isExecuting = timer.m_isExecuting;
        }

        public void SaveToXml(XmlNode contextNode)
        {
            XmlNode node = Xml.SetNewValue(contextNode, "Timer", string.Empty);
            Xml.SetValue(node, "StartDateTime", this.m_startDateTime.ToString());
            Xml.SetValue(node, "TimerLength", this.m_timerLength.ToString());
            XmlNode node2 = Xml.SetValue(node, "Item", Path.GetFileName(this.m_objectFileName));
            Xml.SetAttribute(node2, "length", this.m_objectLength.ToString());
            Xml.SetAttribute(node2, "type", this.m_objectType.ToString());
            if (this.m_objectLength != this.m_timerLength)
            {
                Xml.SetValue(node, "RepeatInterval", this.m_repeatInterval.ToString());
            }
            if (this.m_recurrence != RecurrenceType.None)
            {
                XmlNode node3 = Xml.SetValue(node, "Recurrence", string.Empty);
                Xml.SetAttribute(node3, "type", this.m_recurrence.ToString());
                Xml.SetValue(node3, "StartDate", this.m_recurrenceStart.ToShortDateString());
                Xml.SetValue(node3, "EndDate", this.m_recurrenceEnd.ToShortDateString());
                switch (this.m_recurrence)
                {
                    case RecurrenceType.Weekly:
                        Xml.SetValue(node3, "Data", ((int) this.m_recurrenceData).ToString());
                        break;

                    case RecurrenceType.Monthly:
                        Xml.SetValue(node3, "Data", (string) this.m_recurrenceData);
                        break;

                    case RecurrenceType.Yearly:
                        Xml.SetValue(node3, "Data", ((DateTime) this.m_recurrenceData).ToString());
                        break;
                }
            }
        }

        private void SetObjectPath()
        {
            if ((this.m_objectFileName != null) && (this.m_objectFileName != string.Empty))
            {
                string str = (this.m_objectType == Vixen.ObjectType.Program) ? Paths.ProgramPath : Paths.SequencePath;
                this.m_objectFileName = Path.Combine(str, Path.GetFileName(this.m_objectFileName));
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(this.m_objectDisplayName);
            if (this.m_timerLength != this.m_objectLength)
            {
                builder.Append("|Repeats");
                if (this.m_repeatInterval != 0)
                {
                    builder.AppendFormat(" every {0} minutes", this.m_repeatInterval);
                }
            }
            if (this.m_recurrence != RecurrenceType.None)
            {
                builder.AppendFormat("|Recurs on a {0} basis", this.m_recurrence.ToString().ToLower());
            }
            return builder.ToString();
        }

        public List<ReferenceRectF> DisplayBounds
        {
            get
            {
                return this.m_displayBounds;
            }
            set
            {
                this.m_displayBounds = value;
            }
        }

        public DateTime EndDate
        {
            get
            {
                DateTime time2 = this.m_startDateTime + this.m_timerLength;
                return time2.Date;
            }
        }

        public DateTime EndDateTime
        {
            get
            {
                return (this.m_startDateTime + this.m_timerLength);
            }
        }

        public TimeSpan EndTime
        {
            get
            {
                return (this.StartTime + this.m_timerLength);
            }
        }

        public bool IsExecuting
        {
            get
            {
                return this.m_isExecuting;
            }
            set
            {
                this.m_isExecuting = value;
            }
        }

        public DateTime LastExecution
        {
            get
            {
                return this.m_lastExecution;
            }
            set
            {
                this.m_lastExecution = value;
            }
        }

        public DateTime NotValidUntil
        {
            get
            {
                return this.m_notValidUntil;
            }
            set
            {
                this.m_notValidUntil = value;
            }
        }

        public TimeSpan ObjectLength
        {
            get
            {
                return this.m_objectLength;
            }
            set
            {
                this.m_objectLength = value;
            }
        }

        public Vixen.ObjectType ObjectType
        {
            get
            {
                return this.m_objectType;
            }
            set
            {
                this.m_objectType = value;
                this.SetObjectPath();
            }
        }

        public string ProgramFileName
        {
            get
            {
                return this.m_objectFileName;
            }
            set
            {
                this.m_objectFileName = value;
                this.m_objectDisplayName = Path.GetFileName(value);
                this.SetObjectPath();
            }
        }

        public string ProgramName
        {
            get
            {
                return this.m_objectDisplayName;
            }
        }

        public RecurrenceType Recurrence
        {
            get
            {
                return this.m_recurrence;
            }
            set
            {
                this.m_recurrence = value;
            }
        }

        public object RecurrenceData
        {
            get
            {
                return this.m_recurrenceData;
            }
            set
            {
                this.m_recurrenceData = value;
            }
        }

        public DateTime RecurrenceEnd
        {
            get
            {
                if (this.m_recurrence == RecurrenceType.None)
                {
                    return this.EndDateTime;
                }
                return this.m_recurrenceEnd;
            }
            set
            {
                this.m_recurrenceEnd = value;
            }
        }

        public DateTime RecurrenceEndDateTime
        {
            get
            {
                if (this.m_recurrence == RecurrenceType.None)
                {
                    return this.EndDateTime;
                }
                return (this.m_recurrenceEnd + this.EndTime);
            }
        }

        public TimeSpan RecurrenceSpan
        {
            get
            {
                if (this.m_recurrence == RecurrenceType.None)
                {
                    return this.m_timerLength;
                }
                return (TimeSpan) (this.m_recurrenceEnd - this.m_recurrenceStart);
            }
        }

        public DateTime RecurrenceStart
        {
            get
            {
                if (this.m_recurrence == RecurrenceType.None)
                {
                    return this.m_startDateTime;
                }
                return this.m_recurrenceStart;
            }
            set
            {
                this.m_recurrenceStart = value;
            }
        }

        public DateTime RecurrenceStartDateTime
        {
            get
            {
                if (this.m_recurrence == RecurrenceType.None)
                {
                    return this.StartDateTime;
                }
                return (this.m_recurrenceStart + this.StartTime);
            }
        }

        public int RepeatInterval
        {
            get
            {
                return this.m_repeatInterval;
            }
            set
            {
                this.m_repeatInterval = value;
            }
        }

        public DateTime StartDate
        {
            get
            {
                return this.m_startDateTime.Date;
            }
            set
            {
                this.m_startDateTime = new DateTime(value.Year, value.Month, value.Day, this.m_startDateTime.Hour, this.m_startDateTime.Minute, 0);
            }
        }

        public DateTime StartDateTime
        {
            get
            {
                return this.m_startDateTime;
            }
            set
            {
                this.m_startDateTime = value;
            }
        }

        public TimeSpan StartTime
        {
            get
            {
                return this.m_startDateTime.TimeOfDay;
            }
            set
            {
                this.m_startDateTime = new DateTime(this.m_startDateTime.Year, this.m_startDateTime.Month, this.m_startDateTime.Day, value.Hours, value.Minutes, 0);
            }
        }

        public TimeSpan TimerLength
        {
            get
            {
                return this.m_timerLength;
            }
            set
            {
                this.m_timerLength = value;
            }
        }
    }
}

