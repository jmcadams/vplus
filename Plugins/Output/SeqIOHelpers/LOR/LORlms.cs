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
                var orderedNodes = (from XmlNode channel in channelsNode
                    where null != channel.Attributes
                    select channel.Attributes["savedIndex"].Value
                    into savedIndex
                    orderby int.Parse(savedIndex)
                    from c in (from XmlNode channelInfo in channelsInfoNode
                        where null != channelInfo.Attributes && channelInfo.Attributes["savedIndex"].Value == savedIndex
                        select channelInfo)
                    select c);

                var outputChannel = 0;

                foreach (var node in orderedNodes.Where(n => null != n.Attributes && null != n.Attributes["deviceType"])) {
                    // ReSharper disable once PossibleNullReferenceException
                    switch (node.Attributes["deviceType"].Value) {
                        case "LOR":
                            seqChannels.Add(new Channel(node.Attributes["name"].Value,
                                Color.FromArgb((int) (int.Parse(node.Attributes["color"].Value) | 0xff000000)),
                                outputChannel++));
                            break;
                        case "DMX":
                            seqChannels.AddRange(new[] {
                                new Channel(node.Attributes["name"].Value, Color.FromArgb((int) ((int.Parse(node.Attributes["color"].Value) | 0xff000000) & 0xffff0000)),
                                    outputChannel++),
                                new Channel(node.Attributes["name"].Value, Color.FromArgb((int) ((int.Parse(node.Attributes["color"].Value) | 0xff000000) & 0xff00ff00)),
                                    outputChannel++),
                                new Channel(node.Attributes["name"].Value, Color.FromArgb((int) ((int.Parse(node.Attributes["color"].Value) | 0xff000000) & 0xff0000ff)),
                                    outputChannel++)
                            });
                            break;
                    }
                }
            }

            return seqChannels;
        }


        private void SetChannelEvents(IEnumerable<Channel> seqChannels, IEnumerable channelsInfoNode) {
            var row = 0;
            var twinkle = new Twinkle();

            foreach (
                var firstNode in
                    from x in
                        seqChannels.Select(c => c.Name).Select(
                            name => (from XmlNode ch in channelsInfoNode where null != ch.Attributes && ch.Attributes["name"].Value == name select ch))
                    select null != x ? x.First() : null
                    into firstNode
                    let attributes = firstNode.Attributes
                    where attributes != null
                    where null != firstNode && null != attributes
                    let channelColor = int.Parse(attributes["color"].Value)
                    select firstNode) {
                // ReSharper disable once PossibleNullReferenceException
                var isDMX = firstNode.Attributes["deviceType"].Value.Equals("DMX");
                var channelColor = int.Parse(firstNode.Attributes["color"].Value);
                foreach (XmlNode y in firstNode.ChildNodes) {
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
                                    SetChannelEvents(isDMX, row, eventStart + i, channelColor, intensity);
                                }
                            }
                            else {
                                for (var i = 0; i < eventCount; i++) {
                                    var currentIntensity = (int) ((double) i / eventCount * intensityDifference + intensityStart);
                                    SetChannelEvents(isDMX, row, eventStart + i, channelColor, currentIntensity);
                                }
                            }
                            break;
                        case "twinkle":
                            twinkle.Set();
                            if (intensity > 0) {
                                for (var i = 0; i < eventCount; i++) {
                                    SetChannelEvents(isDMX, row, eventStart + i, channelColor, intensity * twinkle.State);
                                    twinkle.Update();
                                }
                            }
                            else {
                                for (var i = 0; i < eventCount; i++) {
                                    var currentIntensity = (int)(((double)i / eventCount * intensityDifference + intensityStart) * twinkle.State);
                                    SetChannelEvents(isDMX, row, eventStart + i, channelColor, currentIntensity);
                                    twinkle.Update();
                                }
                            }
                            break;
                        case "shimmer":
                            var shimmerState = eventStart & 0x01;
                            if (intensity > 0) {
                                for (var i = 0; i < eventCount; i++) {
                                    var currentIntensity = intensity * shimmerState;
                                    SetChannelEvents(isDMX, row, eventStart + i, channelColor, currentIntensity);
                                    shimmerState = 1 - shimmerState;
                                }
                            }
                            else {
                                for (var i = 0; i < eventCount; i++) {
                                    var currentIntensity = (int)((((double)i / eventCount) * intensityDifference + intensityStart) * shimmerState);
                                    SetChannelEvents(isDMX, row, eventStart + i, channelColor, currentIntensity);
                                    shimmerState = 1 - shimmerState;
                                }
                            }
                            break;
                        default:
                            string.Format("Unknow effect type {0} - skipped", effectType).CrashLog();
                            break;
                    }
                }
                row += isDMX ? 3 : 1;
            }
        }


        private void SetChannelEvents(bool isDMX, int row, int eventStart, int channelColor, int currentIntensity) {
            if (isDMX) {
                SetDMXChannels(row, eventStart, channelColor, currentIntensity);
            }
            else {
                _eventValues[row, eventStart] = (byte) currentIntensity;
            }
        }


        private void SetDMXChannels(int row, int eventStart, int channelColor, int intensity) {
            _eventValues[row, eventStart] = ComputeRed(channelColor, intensity);
            _eventValues[row + 1, eventStart] = ComputeGreen(channelColor, intensity);
            _eventValues[row + 2, eventStart] = ComputeBlue(channelColor, intensity); 
        }


        private static byte ComputeRed(int color, int intensity) {
            return (byte)(((color & 0xff0000) >> 16) / Utils.Cell8BitMax * intensity);
        }

        private static byte ComputeGreen(int color, int intensity) {
            return (byte)(((color & 0xff00) >> 8) / Utils.Cell8BitMax * intensity);
        }

        private static byte ComputeBlue(int color, int intensity) {
            return (byte)((color & 0xff) / Utils.Cell8BitMax * intensity);
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
