using System;
using System.IO;
using System.Text;
using System.Xml;

using Common;

using CommonControls;

namespace VixenPlus {
    internal class Timer : IComparable<Timer>
    {
        private string _objectFileName;
        private readonly TimeSpan _objectLength;
        private readonly ObjectType _objectType;
        private readonly DateTime _recurrenceEnd;
        private readonly DateTime _recurrenceStart;


        public Timer(XmlNode timerNode)
        {
            _objectType = ObjectType.Sequence;
            IsExecuting = false;
            LastExecution = DateTime.MinValue;
            NotValidUntil = DateTime.MinValue;
            var startDateTimeNode = timerNode["StartDateTime"];
            if (startDateTimeNode != null)
            {
                StartDateTime = DateTime.Parse(startDateTimeNode.InnerText);
            }
            var timerLenNode = timerNode["TimerLength"];
            if (timerLenNode != null)
            {
                TimerLength = TimeSpan.Parse(timerLenNode.InnerText);
            }
            XmlNode node = timerNode["Item"];
            if (node != null)
            {
                if (node.Attributes != null)
                {
                    _objectLength = TimeSpan.Parse(node.Attributes["length"].Value);
                    _objectType = (ObjectType) Enum.Parse(typeof (ObjectType), node.Attributes["type"].Value);
                }
                ProgramFileName = node.InnerText;
            }
            if (_objectLength != TimerLength)
            {
                var repeatNode = timerNode["RepeatInterval"];
                if (repeatNode != null)
                {
                    RepeatInterval = Convert.ToInt32(repeatNode.InnerText);
                }
            }
            XmlNode node2 = timerNode["Recurrence"];
            if (node2 == null) {
                return;
            }
            if (node2.Attributes != null)
            {
                Recurrence = (RecurrenceType) Enum.Parse(typeof (RecurrenceType), node2.Attributes["type"].Value);
            }
            var xmlElement = node2["StartDate"];
            if (xmlElement != null)
            {
                _recurrenceStart = DateTime.Parse(xmlElement.InnerText);
            }
            var element = node2["EndDate"];
            if (element != null)
            {
                _recurrenceEnd = DateTime.Parse(element.InnerText);
            }
            switch (Recurrence)
            {
                case RecurrenceType.Weekly:
                    var weeklyDataNode = node2["Data"];
                    if (weeklyDataNode != null)
                    {
                        RecurrenceData = Convert.ToInt32(weeklyDataNode.InnerText);
                    }
                    break;

                case RecurrenceType.Monthly:
                    var monthlyDataNode = node2["Data"];
                    if (monthlyDataNode != null)
                    {
                        RecurrenceData = monthlyDataNode.InnerText;
                    }
                    break;

                case RecurrenceType.Yearly:
                    var yearlyDataNode = node2["Data"];
                    if (yearlyDataNode != null)
                    {
                        RecurrenceData = DateTime.Parse(yearlyDataNode.InnerText);
                    }
                    break;
            }
        }


        public DateTime EndDate
        {
            get
            {
                var time2 = StartDateTime + TimerLength;
                return time2.Date;
            }
        }

        public DateTime EndDateTime
        {
            get { return (StartDateTime + TimerLength); }
        }

        public TimeSpan EndTime
        {
            get { return (StartTime + TimerLength); }
        }

        public bool IsExecuting { get; set; }

        public DateTime LastExecution { get; set; }

        public DateTime NotValidUntil { get; set; }

        public TimeSpan ObjectLength
        {
            get { return _objectLength; }
        }

        public ObjectType ObjectType
        {
            get { return _objectType; }
        }

        public string ProgramFileName
        {
            get { return _objectFileName; }
            private set
            {
                _objectFileName = value;
                ProgramName = Path.GetFileName(value);
                SetObjectPath();
            }
        }

        public string ProgramName { get; private set; }

        public RecurrenceType Recurrence { get; private set; }

        public object RecurrenceData { get; private set; }

        public DateTime RecurrenceEnd
        {
            get {
                return Recurrence == RecurrenceType.None ? EndDateTime : _recurrenceEnd;
            }
        }

        public DateTime RecurrenceEndDateTime
        {
            get
            {
                if (Recurrence == RecurrenceType.None)
                {
                    return EndDateTime;
                }
                return (_recurrenceEnd + EndTime);
            }
        }

        public TimeSpan RecurrenceSpan
        {
            get
            {
                if (Recurrence == RecurrenceType.None)
                {
                    return TimerLength;
                }
                return (_recurrenceEnd - _recurrenceStart);
            }
        }

        public DateTime RecurrenceStart
        {
            get {
                return Recurrence == RecurrenceType.None ? StartDateTime : _recurrenceStart;
            }
        }

        public DateTime RecurrenceStartDateTime
        {
            get
            {
                if (Recurrence == RecurrenceType.None)
                {
                    return StartDateTime;
                }
                return (_recurrenceStart + StartTime);
            }
        }

        public int RepeatInterval { get; private set; }

        public DateTime StartDate
        {
            get { return StartDateTime.Date; }
        }

        public DateTime StartDateTime { get; private set; }

        public TimeSpan StartTime
        {
            get { return StartDateTime.TimeOfDay; }
        }

        public TimeSpan TimerLength { get; private set; }


        public int CompareTo(Timer other)
        {
            return StartTime.CompareTo(other.StartTime);
        }

        private void SetObjectPath() {
            if (string.IsNullOrEmpty(_objectFileName)) {
                return;
            }

            var path = (_objectType == ObjectType.Program) ? Paths.ProgramPath : Paths.SequencePath;
            // ReSharper disable AssignNullToNotNullAttribute
            _objectFileName = Path.Combine(path, Path.GetFileName(_objectFileName));
            // ReSharper restore AssignNullToNotNullAttribute
        }


        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(ProgramName);
            if (TimerLength != _objectLength)
            {
                builder.Append("|Repeats");
                if (RepeatInterval != 0)
                {
                    builder.AppendFormat(" every {0} minutes", RepeatInterval);
                }
            }
            if (Recurrence != RecurrenceType.None)
            {
                builder.AppendFormat("|Recurs on a {0} basis", Recurrence.ToString().ToLower());
            }
            return builder.ToString();
        }
    }
}