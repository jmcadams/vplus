using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

using VixenPlus;

using VixenPlusCommon;

namespace SeqIOHelpers {
    public abstract class VixenFileIOBase : IFileIOHandler {

        public abstract string DialogFilterList();
        public abstract void SaveSequence(EventSequence eventSequence);
        public abstract void SaveProfile(Profile profile);

        public virtual string FileExtension() {
            return ".vix";
        }

        public virtual int PreferredOrder() {
            return int.MaxValue;
        }

        public virtual bool IsNativeToVixenPlus() {
            return false;
        }

        public virtual bool CanSave() {
            return false;
        }

        public virtual bool CanOpen() {
            return false;
        }

        //public void LoadEmbeddedData(XmlNode contextNode) {
        //    _fullChannels.Clear();
        //    var xmlNodeList = contextNode.SelectNodes("Channels/Channel");
        //    if (xmlNodeList != null) {
        //        foreach (XmlNode node in xmlNodeList) {
        //            _fullChannels.Add(new Channel(node));
        //        }
        //    }
        //    PlugInData = new SetupData();
        //    PlugInData.LoadFromXml(contextNode);
        //    Groups = Group.LoadFromXml(contextNode) ?? new Dictionary<string, GroupData>();
        //    Group.LoadFromFile(contextNode, Groups);
        //}

        public EventSequence OpenSequence(string fileName) {
            var contextNode = new XmlDocument();
            contextNode.Load(fileName);
            var requiredNode = Xml.GetRequiredNode(contextNode, "Program");

            var es = new EventSequence {
                FileName = fileName, FullChannels = new List<Channel>(), Channels = new List<Channel>(), PlugInData = new SetupData(),
                LoadableData = new LoadableData(), Extensions = new SequenceExtensions(),
                AudioDeviceVolume = int.Parse(Xml.GetNodeAlways(requiredNode, "AudioVolume", "100").InnerText)
            };

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
                es.MinimumLevel = (byte) Convert.ToInt32(minLevelNode.InnerText);
            }

            var mnaxLevelNode = requiredNode.SelectSingleNode("MaximumLevel");
            if (mnaxLevelNode != null) {
                es.MaximumLevel = (byte) Convert.ToInt32(mnaxLevelNode.InnerText);
            }

            var audioDeviceNode = requiredNode.SelectSingleNode("AudioDevice");
            if (audioDeviceNode != null) {
                es.AudioDeviceIndex = int.Parse(audioDeviceNode.InnerText);
            }


            var profileNode = requiredNode.SelectSingleNode("Profile");
            if (profileNode == null) {
                LoadEmbeddedData(requiredNode, es);
            }
            else {
                //private void LoadEmbeddedData(string fileName) {
                //    if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName)) {
                //        var document = new XmlDocument();
                //        document.Load(fileName);
                //        LoadEmbeddedData(document.SelectSingleNode("//Program"));
                //    }
                //    else {
                //        PlugInData = new SetupData();
                //    }
                //}
                
                var path = Path.Combine(Paths.ProfilePath, es.Profile.Name + Vendor.ProfileExtension);
                if (File.Exists(path)) {
                    es.AttachToProfile(es.FileIOHandler.OpenProfile(path));
                    es.Groups = es.Profile.Groups;
                }
                else {
                    LoadEmbeddedData(es.FileName, es);
                }

                es.AttachToProfile(profileNode.InnerText);
            }

            es.UpdateEventValueArray();

            var audioFileNode = requiredNode.SelectSingleNode("Audio");
            if (audioFileNode != null) {
                if (audioFileNode.Attributes != null) {
                    es.Audio = new Audio(audioFileNode.InnerText, audioFileNode.Attributes["filename"].Value,
                        Convert.ToInt32(audioFileNode.Attributes["duration"].Value));
                }
            }


            var eventValueNode = requiredNode.SelectSingleNode("EventValues");
            if (eventValueNode != null) {
                var buffer = Convert.FromBase64String(eventValueNode.InnerText);
                var index = 0;
                var count = es.FullChannels.Count;
                for (var row = 0; (row < count) && (index < buffer.Length); row++) {
                    for (var column = 0; (column < es.TotalEventPeriods) && (index < buffer.Length); column++) {
                        es.EventValues[row, column] = buffer[index++];
                    }
                }
            }

            var windowSizeNode = requiredNode.SelectSingleNode("WindowSize");
            if (windowSizeNode != null) {
                var strArray = windowSizeNode.InnerText.Split(',');
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

            windowSizeNode = requiredNode.SelectSingleNode("ChannelWidth");
            if (windowSizeNode != null) {
                try {
                    es.ChannelWidth = Convert.ToInt32(windowSizeNode.InnerText);
                }
                catch {
                    es.ChannelWidth = 0;
                }
            }

            var engineTypeNode = requiredNode.SelectSingleNode("EngineType");
            if (engineTypeNode != null) {
                try {
                    es.EngineType = (EngineType) Enum.Parse(typeof (EngineType), engineTypeNode.InnerText);
                }
                    // ReSharper disable EmptyGeneralCatchClause
                catch
                    // ReSharper restore EmptyGeneralCatchClause
                {}
            }

            es.LoadableData.LoadFromXml(requiredNode);
            es.Extensions.LoadFromXml(requiredNode);

            es.ApplyGroup();

            return es;
        }

        private static void LoadEmbeddedData(XmlNode requiredNode, EventSequence es) {
            var fullChannels = new List<Channel>();
            var xmlNodeList = requiredNode.SelectNodes("Channels/Channel");
            if (xmlNodeList != null) {
                fullChannels.AddRange(from XmlNode node in xmlNodeList select new Channel(node));
            }
            es.SetFullChannels(fullChannels);

            es.PlugInData = new SetupData();
            es.PlugInData.LoadFromXml(requiredNode);
            es.Groups = Group.LoadFromXml(requiredNode) ?? new Dictionary<string, GroupData>();
            Group.LoadFromFile(requiredNode, es.Groups);
        }

        public static void LoadEmbeddedData(string fileName, EventSequence es) {
            if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName)) {
                var document = new XmlDocument();
                document.Load(fileName);
                LoadEmbeddedData(document.SelectSingleNode("//Program"), es);
            }
            else {
                es.PlugInData = new SetupData();
            }
        }


        public virtual Profile OpenProfile(string filename) {
            var p = new Profile();

            var document = new XmlDocument();
            document.Load(filename);
            XmlNode documentElement = document.DocumentElement;
            p.FileName = filename;
            p.ClearChannels();
            if (documentElement != null) {
                var channelObjectsNode = documentElement.SelectNodes("ChannelObjects/*");
                if (channelObjectsNode != null) {
                    foreach (XmlNode channelObject in channelObjectsNode) {
                        p.AddChannelObject(new Channel(channelObject), false);
                    }
                }

                var outputNodes = documentElement.SelectSingleNode("Outputs");
                if (outputNodes != null) {
                    foreach (var outputChannel in outputNodes.InnerText.Split(',').Where(outputChannel => outputChannel.Length > 0)) {
                        p.AddChannelOutput(Convert.ToInt32(outputChannel));
                    }
                }
            }
            p.PlugInData.LoadFromXml(documentElement);
            p.Groups = Group.LoadFromXml(documentElement) ?? new Dictionary<string, GroupData>();
            p.IsDirty = Group.LoadFromFile(documentElement, p.Groups);
            if (documentElement != null) {
                var disabledChannelsNode = documentElement.SelectSingleNode("DisabledChannels");
                if (disabledChannelsNode != null) {
                    foreach (
                        var disabledChannel in disabledChannelsNode.InnerText.Split(',').Where(disabledChannel => disabledChannel != string.Empty)) {
                        p.Channels[Convert.ToInt32(disabledChannel)].Enabled = false;
                    }
                }
            }


            p.Freeze();

            return p;
        }


        protected delegate XmlNode FormatChannel(XmlDocument doc, Channel ch);


        protected static void BaseSaveSequence(XmlDocument contextNode, EventSequence eventSequence, FormatChannel fc) {
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


        protected static void BaseSaveProfile(XmlDocument doc, Profile profileObject, FormatChannel fc) {
            XmlNode profileDoc = doc.DocumentElement;

            var channelObjectsNode = Xml.GetEmptyNodeAlways(profileDoc, "ChannelObjects");
            foreach (var channel in profileObject.Channels) {
                channelObjectsNode.AppendChild(fc(doc, channel));
            }

            var builder = new StringBuilder();
            foreach (var channel in profileObject.Channels) {
                builder.AppendFormat("{0},", channel.OutputChannel);
            }
            Xml.GetEmptyNodeAlways(profileDoc, "Outputs").InnerText = builder.ToString().TrimEnd(',');
            
            if (profileDoc != null) {
                profileDoc.AppendChild(doc.ImportNode(profileObject.PlugInData.RootNode, true));
            }
            
            var disabledChannels = new List<string>();
            for (var i = 0; i < profileObject.Channels.Count; i++) {
                if (!profileObject.Channels[i].Enabled) {
                    disabledChannels.Add(i.ToString(CultureInfo.InvariantCulture));
                }
            }
            Xml.SetValue(profileDoc, "DisabledChannels", string.Join(",", disabledChannels.ToArray()));
       }
   }
}