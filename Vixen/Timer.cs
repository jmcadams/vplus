using System;
using System.IO;
using System.Text;
using System.Xml;

internal class Timer : IComparable<Timer>
{
    private string _objectFileName;
    private readonly TimeSpan _objectLength;
    private readonly ObjectType _objectType;
    private readonly DateTime _recurrenceEnd;
    private readonly DateTime _recurrenceStart;


/*
    public Timer()
    {
        _objectType = ObjectType.Sequence;
        _isExecuting = false;
        LastExecution = DateTime.MinValue;
        NotValidUntil = DateTime.MinValue;
    }
*/

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
/*
        set { _objectLength = value; }
*/
    }

    public ObjectType ObjectType
    {
        get { return _objectType; }
/*
        private set
        {
            _objectType = value;
            SetObjectPath();
        }
*/
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
/*
        set { _recurrenceEnd = value; }
*/
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
/*
        set { _recurrenceStart = value; }
*/
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
/*
        set { StartDateTime = new DateTime(value.Year, value.Month, value.Day, StartDateTime.Hour, StartDateTime.Minute, 0); }
*/
    }

    public DateTime StartDateTime { get; private set; }

    public TimeSpan StartTime
    {
        get { return StartDateTime.TimeOfDay; }
/*
        set
        {
            StartDateTime = new DateTime(StartDateTime.Year, StartDateTime.Month, StartDateTime.Day, value.Hours,
                value.Minutes, 0);
        }
*/
    }

    public TimeSpan TimerLength { get; private set; }


    public int CompareTo(Timer other)
    {
        return StartTime.CompareTo(other.StartTime);
    }

/*
    public Timer Clone()
    {
        var timer = new Timer();
        timer.Copy(this);
        return timer;
    }
*/

/*
    public void Copy(Timer timer)
    {
        _objectLength = timer._objectLength;
        ObjectType = timer.ObjectType;
        ProgramFileName = timer.ProgramFileName;
        ProgramName = timer.ProgramName;
        Recurrence = timer.Recurrence;
        _recurrenceEnd = timer._recurrenceEnd;
        _recurrenceStart = timer._recurrenceStart;
        RepeatInterval = timer.RepeatInterval;
        StartDateTime = timer.StartDateTime;
        TimerLength = timer.TimerLength;
        RecurrenceData = timer.RecurrenceData;
        _isExecuting = timer._isExecuting;
    }
*/

/*
    public void SaveToXml(XmlNode contextNode)
    {
        var node = Xml.SetNewValue(contextNode, "Timer", string.Empty);
        Xml.SetValue(node, "StartDateTime", StartDateTime.ToString(CultureInfo.InvariantCulture));
        Xml.SetValue(node, "TimerLength", TimerLength.ToString());
        var node2 = Xml.SetValue(node, "Item", Path.GetFileName(_objectFileName));
        Xml.SetAttribute(node2, "length", _objectLength.ToString());
        Xml.SetAttribute(node2, "type", _objectType.ToString());
        if (_objectLength != TimerLength)
        {
            Xml.SetValue(node, "RepeatInterval", RepeatInterval.ToString(CultureInfo.InvariantCulture));
        }
        if (Recurrence == RecurrenceType.None) {
            return;
        }
        var node3 = Xml.SetValue(node, "Recurrence", string.Empty);
        Xml.SetAttribute(node3, "type", Recurrence.ToString());
        Xml.SetValue(node3, "StartDate", _recurrenceStart.ToShortDateString());
        Xml.SetValue(node3, "EndDate", _recurrenceEnd.ToShortDateString());
        switch (Recurrence)
        {
            case RecurrenceType.Weekly:
                Xml.SetValue(node3, "Data", ((int) RecurrenceData).ToString(CultureInfo.InvariantCulture));
                break;

            case RecurrenceType.Monthly:
                Xml.SetValue(node3, "Data", (string) RecurrenceData);
                break;

            case RecurrenceType.Yearly:
                Xml.SetValue(node3, "Data", ((DateTime) RecurrenceData).ToString(CultureInfo.InvariantCulture));
                break;
        }
    }
*/


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