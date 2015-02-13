using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml;

using VixenPlus;

using VixenPlusCommon;
using VixenPlusCommon.Annotations;

// ReSharper disable once CheckNamespace
namespace SeqIOHelpers {
    [UsedImplicitly]
    public class LORlms : IFileIOHandler {
        private byte[,] _eventValues;

        public string DialogFilterList() {
            return string.Format("LOR Music Sequence (*{0})|*{0}", FileExtension());
        }


        public string Name() {
            return "LOR lms";
        }


        public string FileExtension() {
            return ".lms";
        }


        public int PreferredOrder() {
            return 3;
        }


        public bool IsNativeToVixenPlus() {
            return false;
        }


        public bool CanSave() {
            return false;
        }


        public void SaveSequence(EventSequence eventSequence) {
            throw new NotSupportedException("Saving LOR Files is not supported");
        }


        public void SaveProfile(Profile profile) {
            throw new NotSupportedException("LOR does not support profiles.");
        }


        public bool CanOpen() {
            return true;
        }

        private int TotalCentiseconds { get; set; }

        public EventSequence OpenSequence(string fileName) {
            var seqDoc = Xml.LoadDocument(fileName);
            var trackNode = seqDoc.SelectSingleNode("sequence/tracks");
            var channelsInfoNode = seqDoc.SelectSingleNode("sequence/channels");

            SetTotalCentiseconds(trackNode);

            var seqChannels = GetChannels(trackNode, channelsInfoNode);

            _eventValues = new byte[seqChannels.Count, TotalCentiseconds];
            SetChannelEvents(seqChannels, channelsInfoNode);

            var es = new EventSequence();
            es.SetFullChannels(seqChannels);
            es.Time = TotalCentiseconds * 10;
            es.EventPeriod = 10;
            es.FileIOHandler = this;
            es.EventValues = _eventValues;
            es.Name = Path.GetFileNameWithoutExtension(fileName);

            return es;
        }


        private void SetTotalCentiseconds(IEnumerable trackNode) {
            TotalCentiseconds = 0;
            if (null == trackNode) {
                return;
            }

            foreach (var track in trackNode.Cast<XmlNode>().Where(track => null != track.Attributes)) {
                // ReSharper disable once PossibleNullReferenceException
                TotalCentiseconds += int.Parse(track.Attributes["totalCentiseconds"].Value);
            }
        }


        private static List<Channel> GetChannels(IEnumerable trackNode, IEnumerable channelsInfoNode) {
            var seqChannels = new List<Channel>();

            if (null == trackNode || null == channelsInfoNode) {
                return seqChannels;
            }

            foreach (var channelsNode in trackNode.Cast<XmlNode>()
                .Where(track => null != track.Attributes)
                .Select(track => track.SelectSingleNode("channels"))
                .Where(channelsNode => null != channelsNode)) {
                seqChannels.AddRange((from XmlNode channel in channelsNode
                    where null != channel.Attributes
                    select channel.Attributes["savedIndex"].Value
                    into savedIndex
                    orderby int.Parse(savedIndex)
                    from c in (from XmlNode channelInfo in channelsInfoNode
                        where null != channelInfo.Attributes && channelInfo.Attributes["savedIndex"].Value == savedIndex
                        select channelInfo)
                    select c).Select(c =>
                        // ReSharper disable once PossibleNullReferenceException
                        new Channel(c.Attributes["name"].Value, Color.FromArgb(int.Parse(c.Attributes["color"].Value)),
                            int.Parse(c.Attributes["savedIndex"].Value))));
            }

            return seqChannels;
        }


        private void SetChannelEvents(IEnumerable<Channel> seqChannels, IEnumerable channelsInfoNode) {
            var row = 0;

            foreach (var x in
                seqChannels.Select(c => c.Name).Select(
                    name => (from XmlNode ch in channelsInfoNode where null != ch.Attributes && ch.Attributes["name"].Value == name select ch))) {
                foreach (XmlNode y in x.First().ChildNodes) {
                    if (null == y.Attributes) {
                        continue;
                    }

                    var effectType = y.Attributes["type"].Value;

                    var intensity = GetIntAttributeOrDefault(y, "intensity", 0).ToValue();

                    var startEvt = GetIntAttributeOrDefault(y, "startCentisecond", 0);
                    var endEvt = GetIntAttributeOrDefault(y, "endCentisecond", 0);
                    var diffEvt = endEvt - startEvt;

                    var startInt = GetIntAttributeOrDefault(y, "startIntensity", 0).ToValue();
                    var endInt = GetIntAttributeOrDefault(y, "endIntensity", 0).ToValue();
                    var diffInt = endInt - startInt;

                    switch (effectType) {
                        case "intensity":
                            if (intensity > 0 && startInt == 0 && endInt == 0) {
                                SetIntensity(row, startEvt, diffEvt, intensity);
                            }
                            else if (startInt > 0 || endInt > 0) {
                                SetRampFade(row, startEvt, diffEvt, diffInt);
                            }
                            else {
                                string.Format("Unknown intensity settings: I:{0} SI:{1} EI:{2} SE:{3} EE:{4} - Skipped", intensity, startInt, endInt, startEvt, endEvt)
                                    .CrashLog();
                            }
                            break;
                        case "twinkle":
                            throw new NotImplementedException();
                            break;
                        case "shimmer":
                            throw new NotImplementedException();
                            break;
                        default:
                            string.Format("Unknow effect type {0} - skipped", effectType).CrashLog();
                            break;
                    }
                }
                row++;
            }
        }


        private void SetIntensity(int row, int start, int count, int value) {
            for (var i = 0; i < count; i++) {
                _eventValues[row, start + i] = (byte)value;
            }
        }


        private void SetRampFade(int row, int start, int count, int diff) {
            for (var i = 0; i < count; i++) {
                _eventValues[row, start + i] = (byte)((double)(i) / count * diff + start);
            }          
        }


        private static int GetIntAttributeOrDefault(XmlNode xmlNode, string attributeName, int defaultValue) {
            int retVal;
            if (!(null != xmlNode.Attributes && null != xmlNode.Attributes[attributeName] && int.TryParse(xmlNode.Attributes[attributeName].Value, out retVal))) {
                retVal = defaultValue;
            }

            return retVal;
        }


        public Profile OpenProfile(string fileName) {
            throw new NotImplementedException();
        }


        public void LoadEmbeddedData(string fileName, EventSequence es) {
            throw new NotImplementedException();
        }
    }
}
