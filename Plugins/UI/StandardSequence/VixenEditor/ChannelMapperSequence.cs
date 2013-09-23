using System;
using System.Xml;

namespace VixenEditor {
    class ChannelMapperSequence {

        private readonly string _fileName;

        private readonly XmlDocument _xml = new XmlDocument();
        private int _eventCount;

        public ChannelMapperSequence(string fileName)
        {
            _fileName = fileName;
            ParseFile();
        }

        internal string EventData { get; set; }

        private void ParseFile()
        {
            _xml.Load(_fileName);


            var timeNode = _xml.SelectSingleNode("Program/Time");
            if (timeNode != null)
            {
                var time = int.Parse(timeNode.InnerText);
                var periodNode = _xml.SelectSingleNode("Program/EventPeriodInMilliseconds");
                if (periodNode != null)
                {
                    var mills = int.Parse(periodNode.InnerText);
                    _eventCount = time/mills;

                    if (_eventCount*mills != time)
                    {
                        _eventCount++;
                    }
                }
            }

            var eventsNode = _xml.SelectSingleNode("Program/EventValues");
            if (eventsNode != null)
            {
                EventData = eventsNode.InnerText;
            }
        }


        internal int GetEventCount()
        {
            return _eventCount;
        }


        internal string SaveNewData(string newProfileName)
        {
            var profileNode = _xml.SelectSingleNode("Program/Profile");
            if (profileNode != null)
            {
                profileNode.InnerText = newProfileName;
            }

            var eventsNode = _xml.SelectSingleNode("Program/EventValues");
            if (eventsNode != null)
            {
                eventsNode.InnerText = EventData;
            }

            var newFileName = _fileName.Replace(".vix", "." + DateTime.Now.ToString("yyyMMddHHmmssffff") + ".vix");
            _xml.Save(newFileName);

            return newFileName;
        }
    }
}
