using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace VixenPlus
{
    internal class Timer : IComparable<Timer>
    {
        private List<ReferenceRectF> _displayBounds;
        private bool _isExecuting;
        private string _objectDisplayName;
        private string _objectFileName;
        private TimeSpan _objectLength;
        private ObjectType _objectType;
        private RecurrenceType _recurrence;
        private object _recurrenceData;
        private DateTime _recurrenceEnd;
        private DateTime _recurrenceStart;
        private int _repeatInterval;
        private DateTime _startDateTime;
        private TimeSpan _timerLength;

        public Timer()
        {
            _objectType = ObjectType.Sequence;
            _displayBounds = new List<ReferenceRectF>();
            _isExecuting = false;
            LastExecution = DateTime.MinValue;
            NotValidUntil = DateTime.MinValue;
        }

        public Timer(XmlNode timerNode)
        {
            _objectType = ObjectType.Sequence;
            _displayBounds = new List<ReferenceRectF>();
            _isExecuting = false;
            LastExecution = DateTime.MinValue;
            NotValidUntil = DateTime.MinValue;
            XmlElement startDateTimeNode = timerNode["StartDateTime"];
            if (startDateTimeNode != null)
            {
                _startDateTime = DateTime.Parse(startDateTimeNode.InnerText);
            }
            XmlElement timerLenNode = timerNode["TimerLength"];
            if (timerLenNode != null)
            {
                _timerLength = TimeSpan.Parse(timerLenNode.InnerText);
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
            if (_objectLength != _timerLength)
            {
                XmlElement repeatNode = timerNode["RepeatInterval"];
                if (repeatNode != null)
                {
                    _repeatInterval = Convert.ToInt32(repeatNode.InnerText);
                }
            }
            XmlNode node2 = timerNode["Recurrence"];
            if (node2 != null)
            {
                if (node2.Attributes != null)
                {
                    _recurrence = (RecurrenceType) Enum.Parse(typeof (RecurrenceType), node2.Attributes["type"].Value);
                }
                XmlElement xmlElement = node2["StartDate"];
                if (xmlElement != null)
                {
                    _recurrenceStart = DateTime.Parse(xmlElement.InnerText);
                }
                XmlElement element = node2["EndDate"];
                if (element != null)
                {
                    _recurrenceEnd = DateTime.Parse(element.InnerText);
                }
                switch (_recurrence)
                {
                    case RecurrenceType.Weekly:
                        XmlElement weeklyDataNode = node2["Data"];
                        if (weeklyDataNode != null)
                        {
                            _recurrenceData = Convert.ToInt32(weeklyDataNode.InnerText);
                        }
                        break;

                    case RecurrenceType.Monthly:
                        XmlElement monthlyDataNode = node2["Data"];
                        if (monthlyDataNode != null)
                        {
                            _recurrenceData = monthlyDataNode.InnerText;
                        }
                        break;

                    case RecurrenceType.Yearly:
                        XmlElement yearlyDataNode = node2["Data"];
                        if (yearlyDataNode != null)
                        {
                            _recurrenceData = DateTime.Parse(yearlyDataNode.InnerText);
                        }
                        break;
                }
            }
        }

        public List<ReferenceRectF> DisplayBounds
        {
            get { return _displayBounds; }
            set { _displayBounds = value; }
        }

        public DateTime EndDate
        {
            get
            {
                DateTime time2 = _startDateTime + _timerLength;
                return time2.Date;
            }
        }

        public DateTime EndDateTime
        {
            get { return (_startDateTime + _timerLength); }
        }

        public TimeSpan EndTime
        {
            get { return (StartTime + _timerLength); }
        }

        public bool IsExecuting
        {
            get { return _isExecuting; }
            set { _isExecuting = value; }
        }

        public DateTime LastExecution { get; set; }

        public DateTime NotValidUntil { get; set; }

        public TimeSpan ObjectLength
        {
            get { return _objectLength; }
            set { _objectLength = value; }
        }

        public ObjectType ObjectType
        {
            get { return _objectType; }
            set
            {
                _objectType = value;
                SetObjectPath();
            }
        }

        public string ProgramFileName
        {
            get { return _objectFileName; }
            set
            {
                _objectFileName = value;
                _objectDisplayName = Path.GetFileName(value);
                SetObjectPath();
            }
        }

        public string ProgramName
        {
            get { return _objectDisplayName; }
        }

        public RecurrenceType Recurrence
        {
            get { return _recurrence; }
            set { _recurrence = value; }
        }

        public object RecurrenceData
        {
            get { return _recurrenceData; }
            set { _recurrenceData = value; }
        }

        public DateTime RecurrenceEnd
        {
            get {
                return _recurrence == RecurrenceType.None ? EndDateTime : _recurrenceEnd;
            }
            set { _recurrenceEnd = value; }
        }

        public DateTime RecurrenceEndDateTime
        {
            get
            {
                if (_recurrence == RecurrenceType.None)
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
                if (_recurrence == RecurrenceType.None)
                {
                    return _timerLength;
                }
                return (_recurrenceEnd - _recurrenceStart);
            }
        }

        public DateTime RecurrenceStart
        {
            get {
                return _recurrence == RecurrenceType.None ? _startDateTime : _recurrenceStart;
            }
            set { _recurrenceStart = value; }
        }

        public DateTime RecurrenceStartDateTime
        {
            get
            {
                if (_recurrence == RecurrenceType.None)
                {
                    return StartDateTime;
                }
                return (_recurrenceStart + StartTime);
            }
        }

        public int RepeatInterval
        {
            get { return _repeatInterval; }
            set { _repeatInterval = value; }
        }

        public DateTime StartDate
        {
            get { return _startDateTime.Date; }
            set { _startDateTime = new DateTime(value.Year, value.Month, value.Day, _startDateTime.Hour, _startDateTime.Minute, 0); }
        }

        public DateTime StartDateTime
        {
            get { return _startDateTime; }
            set { _startDateTime = value; }
        }

        public TimeSpan StartTime
        {
            get { return _startDateTime.TimeOfDay; }
            set
            {
                _startDateTime = new DateTime(_startDateTime.Year, _startDateTime.Month, _startDateTime.Day, value.Hours,
                                              value.Minutes, 0);
            }
        }

        public TimeSpan TimerLength
        {
            get { return _timerLength; }
            set { _timerLength = value; }
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
            _displayBounds = timer._displayBounds;
            _objectLength = timer._objectLength;
            ObjectType = timer.ObjectType;
            ProgramFileName = timer.ProgramFileName;
            _objectDisplayName = timer._objectDisplayName;
            _recurrence = timer._recurrence;
            _recurrenceEnd = timer._recurrenceEnd;
            _recurrenceStart = timer._recurrenceStart;
            _repeatInterval = timer._repeatInterval;
            _startDateTime = timer._startDateTime;
            _timerLength = timer._timerLength;
            _recurrenceData = timer._recurrenceData;
            _isExecuting = timer._isExecuting;
        }

        public void SaveToXml(XmlNode contextNode)
        {
            XmlNode node = Xml.SetNewValue(contextNode, "Timer", string.Empty);
            Xml.SetValue(node, "StartDateTime", _startDateTime.ToString(CultureInfo.InvariantCulture));
            Xml.SetValue(node, "TimerLength", _timerLength.ToString());
            XmlNode node2 = Xml.SetValue(node, "Item", Path.GetFileName(_objectFileName));
            Xml.SetAttribute(node2, "length", _objectLength.ToString());
            Xml.SetAttribute(node2, "type", _objectType.ToString());
            if (_objectLength != _timerLength)
            {
                Xml.SetValue(node, "RepeatInterval", _repeatInterval.ToString(CultureInfo.InvariantCulture));
            }
            if (_recurrence != RecurrenceType.None)
            {
                XmlNode node3 = Xml.SetValue(node, "Recurrence", string.Empty);
                Xml.SetAttribute(node3, "type", _recurrence.ToString());
                Xml.SetValue(node3, "StartDate", _recurrenceStart.ToShortDateString());
                Xml.SetValue(node3, "EndDate", _recurrenceEnd.ToShortDateString());
                switch (_recurrence)
                {
                    case RecurrenceType.Weekly:
                        Xml.SetValue(node3, "Data", ((int) _recurrenceData).ToString(CultureInfo.InvariantCulture));
                        break;

                    case RecurrenceType.Monthly:
                        Xml.SetValue(node3, "Data", (string) _recurrenceData);
                        break;

                    case RecurrenceType.Yearly:
                        Xml.SetValue(node3, "Data", ((DateTime) _recurrenceData).ToString(CultureInfo.InvariantCulture));
                        break;
                }
            }
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
            builder.Append(_objectDisplayName);
            if (_timerLength != _objectLength)
            {
                builder.Append("|Repeats");
                if (_repeatInterval != 0)
                {
                    builder.AppendFormat(" every {0} minutes", _repeatInterval);
                }
            }
            if (_recurrence != RecurrenceType.None)
            {
                builder.AppendFormat("|Recurs on a {0} basis", _recurrence.ToString().ToLower());
            }
            return builder.ToString();
        }
    }
}