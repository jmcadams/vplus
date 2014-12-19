using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;

using VixenPlus;

namespace SeqIOHelpers {
    public class SeqIOBase : ISeqIOHandler {

        public virtual string DialogFilterList() {
            return "Vixen Plus format (*.vix)|*.vix";
        }


        public virtual int VendorId() {
            return int.MinValue;
        }


        public virtual int PreferredOrder() {
            return int.MaxValue;
        }


        public virtual long VGUID() {
            throw new NotImplementedException();
        }


        public virtual bool IsNativeToVixenPlus() {
            return false;
        }


        public virtual bool CanSave() {
            return false;
        }


        public virtual void Save(EventSequence eventSequence) {
            throw new NotImplementedException();
        }


        public virtual bool CanLoad() {
            return false;
        }


        public virtual void Load(XmlNode contextNode, EventSequence es) {
            var requiredNode = Xml.GetRequiredNode(contextNode, "Program");
            es.FullChannels = new List<Channel>();
            es.Channels = new List<Channel>();
            es.PlugInData = new SetupData();
            es.LoadableData = new LoadableData();
            es.Extensions = new SequenceExtensions();
            var timeNode = requiredNode.SelectSingleNode("Time");
            if (timeNode != null) {
                es.Time = Convert.ToInt32(timeNode.InnerText);
            }
            var eventPeriodNode = requiredNode.SelectSingleNode("EventPeriodInMilliseconds");
            if (eventPeriodNode != null) {
                es.EventPeriod = Convert.ToInt32(eventPeriodNode.InnerText);
            }
            var minLevelNode = requiredNode.SelectSingleNode("MinimumLevel");
            if (minLevelNode != null) {
                es.MinimumLevel = (byte)Convert.ToInt32(minLevelNode.InnerText);
            }
            var mnaxLevelNode = requiredNode.SelectSingleNode("MaximumLevel");
            if (mnaxLevelNode != null) {
                es.MaximumLevel = (byte)Convert.ToInt32(mnaxLevelNode.InnerText);
            }
            var audioDeviceNode = requiredNode.SelectSingleNode("AudioDevice");
            if (audioDeviceNode != null) {
                es.AudioDeviceIndex = int.Parse(audioDeviceNode.InnerText);
            }
            es.AudioDeviceVolume = int.Parse(Xml.GetNodeAlways(requiredNode, "AudioVolume", "100").InnerText);
            var node2 = requiredNode.SelectSingleNode("Profile");
            if (node2 == null) {
                es.LoadEmbeddedData(requiredNode);
            }
            else {
                es.AttachToProfile(node2.InnerText);
            }

            es.UpdateEventValueArray();
            var audioFileNode = requiredNode.SelectSingleNode("Audio");
            if (audioFileNode != null) {
                if (audioFileNode.Attributes != null) {
                    es.Audio = new Audio(audioFileNode.InnerText, audioFileNode.Attributes["filename"].Value,
                        Convert.ToInt32(audioFileNode.Attributes["duration"].Value));
                }
            }
            var count = es.FullChannels.Count;

            var node4 = requiredNode.SelectSingleNode("EventValues");
            if (node4 != null) {
                var buffer = Convert.FromBase64String(node4.InnerText);
                var index = 0;
                for (var row = 0; (row < count) && (index < buffer.Length); row++) {
                    for (var column = 0; (column < es.TotalEventPeriods) && (index < buffer.Length); column++) {
                        es.EventValues[row, column] = buffer[index++];
                    }
                }
            }
            var node5 = requiredNode.SelectSingleNode("WindowSize");
            if (node5 != null) {
                var strArray = node5.InnerText.Split(new[] { ',' });
                try {
                    es.WindowWidth = Convert.ToInt32(strArray[0]);
                }
                catch {
                    es.WindowWidth = 0;
                }
                try {
                    es.WindowHeight = Convert.ToInt32(strArray[1]);
                }
                catch {
                    es.WindowHeight = 0;
                }
            }
            node5 = requiredNode.SelectSingleNode("ChannelWidth");
            if (node5 != null) {
                try {
                    es.ChannelWidth = Convert.ToInt32(node5.InnerText);
                }
                catch {
                    es.ChannelWidth = 0;
                }
            }
            var node6 = requiredNode.SelectSingleNode("EngineType");
            if (node6 != null) {
                try {
                    es.EngineType = (EngineType)Enum.Parse(typeof(EngineType), node6.InnerText);
                }
                // ReSharper disable EmptyGeneralCatchClause
                catch
                    // ReSharper restore EmptyGeneralCatchClause
                { }
            }
            es.LoadableData.LoadFromXml(requiredNode);
            es.Extensions.LoadFromXml(requiredNode);

            es.ApplyGroup();
        }


        protected delegate XmlNode FormatChannel(XmlDocument doc, Channel ch);

        protected static void BaseSave(XmlDocument contextNode, EventSequence eventSequence, FormatChannel fc) {
            var programNode = Xml.GetEmptyNodeAlways(contextNode, "Program");
            Xml.SetValue(programNode, "Time", eventSequence.Length.ToString(CultureInfo.InvariantCulture));
            Xml.SetValue(programNode, "EventPeriodInMilliseconds", eventSequence.EventPeriod.ToString(CultureInfo.InvariantCulture));
            Xml.SetValue(programNode, "MinimumLevel", eventSequence.MinimumLevel.ToString(CultureInfo.InvariantCulture));
            Xml.SetValue(programNode, "MaximumLevel", eventSequence.MaximumLevel.ToString(CultureInfo.InvariantCulture));
            Xml.SetValue(programNode, "AudioDevice", eventSequence.AudioDeviceIndex.ToString(CultureInfo.InvariantCulture));
            Xml.SetValue(programNode, "AudioVolume", eventSequence.AudioDeviceVolume.ToString(CultureInfo.InvariantCulture));

            if (eventSequence.Audio != null) {
                var node = Xml.SetNewValue(programNode, "Audio", eventSequence.Audio.Name);
                Xml.SetAttribute(node, "filename", eventSequence.Audio.FileName);
                Xml.SetAttribute(node, "duration", eventSequence.Audio.Duration.ToString(CultureInfo.InvariantCulture));
            }

            var doc = contextNode.OwnerDocument ?? contextNode;

            if (eventSequence.Profile == null) {
                //Channels
                var channelNodes = Xml.GetEmptyNodeAlways(programNode, "Channels");
                foreach (var channel in eventSequence.FullChannels) {
                    channelNodes.AppendChild(fc(doc, channel));
                }

                //Plugins
                if (programNode.OwnerDocument != null) {
                    programNode.AppendChild(programNode.OwnerDocument.ImportNode(eventSequence.PlugInData.RootNode, true));
                }
            }
            else {
                Xml.SetValue(programNode, "Profile", eventSequence.Profile.Name);
                //todo: maybe we should ask if they want the profile converted as well.
            }

            var channelCount = eventSequence.FullChannels.Count;
            var totalEventPeriods = eventSequence.TotalEventPeriods;
            var inArray = new byte[channelCount * totalEventPeriods];
            var eventIndex = 0;
            for (var row = 0; row < channelCount; row++) {
                for (var col = 0; col < totalEventPeriods; col++) {
                    inArray[eventIndex++] = eventSequence.EventValues[row, col];
                }
            }
            //todo GZIP inArray before encoding?
            Xml.GetNodeAlways(programNode, "EventValues").InnerText = Convert.ToBase64String(inArray);
            if (programNode.OwnerDocument != null && eventSequence.LoadableData != null) {
                programNode.AppendChild(programNode.OwnerDocument.ImportNode(eventSequence.LoadableData.RootNode, true));
            }

            Xml.SetValue(programNode, "EngineType", eventSequence.EngineType.ToString());

            if (programNode.OwnerDocument != null) {
                programNode.AppendChild(programNode.OwnerDocument.ImportNode(eventSequence.Extensions.RootNode, true));
            }
        }


    }
}