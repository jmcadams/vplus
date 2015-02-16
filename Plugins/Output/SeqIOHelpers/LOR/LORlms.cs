using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
                        new Channel(c.Attributes["name"].Value, Color.FromArgb((int)(int.Parse(c.Attributes["color"].Value) | 0xff000000)),
                            int.Parse(c.Attributes["savedIndex"].Value))));
            }

            return seqChannels;
        }


        private void SetChannelEvents(IEnumerable<Channel> seqChannels, IEnumerable channelsInfoNode) {
            var row = 0;
            var twinkle = new Twinkle();

            foreach (var x in
                seqChannels.Select(c => c.Name).Select(
                    name => (from XmlNode ch in channelsInfoNode where null != ch.Attributes && ch.Attributes["name"].Value == name select ch))) {
                foreach (XmlNode y in x.First().ChildNodes) {
                    if (null == y.Attributes) {
                        continue;
                    }

                    var effectType = y.Attributes["type"].Value.ToLower();

                    var intensity = GetIntAttributeOrDefault(y, "intensity", 0).ToValue();

                    var eventStart = GetIntAttributeOrDefault(y, "startCentisecond", 0);
                    var eventEnd = GetIntAttributeOrDefault(y, "endCentisecond", 0);
                    var eventCount = eventEnd - eventStart;

                    var intensityStart = GetIntAttributeOrDefault(y, "startIntensity", 0).ToValue();
                    var intensityEnd = GetIntAttributeOrDefault(y, "endIntensity", 0).ToValue();
                    var intensityDifference = intensityEnd - intensityStart;

                    if (intensity == 0 && intensityStart == 0 && intensityEnd == 0) {
                        intensity = Utils.Cell8BitMax;
                    }

                    switch (effectType) {
                        case "intensity":
                            if (intensity > 0) {
                                for (var i = 0; i < eventCount; i++) {
                                    _eventValues[row, eventStart + i] = (byte)intensity;
                                }
                            }
                            else {
                                for (var i = 0; i < eventCount; i++) {
                                    _eventValues[row, eventStart + i] = (byte)((double)i / eventCount * intensityDifference + intensityStart);
                                }
                            }
                            break;
                        case "twinkle":
                            twinkle.Set();
                            if (intensity > 0) {
                                for (var i = 0; i < eventCount; i++) {
                                    _eventValues[row, eventStart + i] = (byte)(intensity * twinkle.State);
                                    twinkle.Update();
                                }
                            }
                            else {
                                for (var i = 0; i < eventCount; i++) {
                                    _eventValues[row, eventStart + i] = (byte)(((double)i / eventCount * intensityDifference + intensityStart) * twinkle.State);
                                    twinkle.Update();
                                }
                            }
                            break;
                        case "shimmer":
                            var shimmerState = eventStart & 0x01;
                            if (intensity > 0) {
                                for (var i = 0; i < eventCount; i++) {
                                    _eventValues[row, eventStart + i] = (byte)(intensity * shimmerState);
                                    shimmerState = 1 - shimmerState;
                                }
                            }
                            else {
                                for (var i = 0; i < eventCount; i++) {
                                    _eventValues[row, eventStart + i] = (byte)((((double)i / eventCount) * intensityDifference + intensityStart) * shimmerState);
                                    shimmerState = 1 - shimmerState;
                                }
                            }
                            break;
                        default:
                            string.Format("Unknow effect type {0} - skipped", effectType).CrashLog();
                            break;
                    }
                }
                row++;
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
            Debug.Assert(false);
            return null;
        }


        public void LoadEmbeddedData(string fileName, EventSequence es) {
            Debug.Assert(false);
        }


        public bool SupportsProfiles {
            get { return false; }
        }
    }

   internal class Twinkle {
        private const int TwinklePeriods = 80;
        private const int MinTwinklePeriods = 20;
        
        private readonly Random _rand = new Random();
        
        private int _counter;
        
        public int State { get; private set; }

        public void Set() {
            State = _rand.Next(2);
            SetCounter();
        }


        public void Update() {
            if (--_counter >= 0) {
                return;
            }

            State = 1 - State;
            SetCounter();
        }


        private void SetCounter() {
            _counter = _rand.Next(TwinklePeriods) + MinTwinklePeriods;
        }
    }
}
