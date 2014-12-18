using System;
using System.Globalization;
using System.Xml;

using VixenPlus;

namespace SeqIOHelpers {
    public class SeqIOBase : ISeqIOHandler {

        public virtual string DialogFilterList() {
            throw new System.NotImplementedException();
        }


        public virtual int VendorId() {
            throw new System.NotImplementedException();
        }


        public virtual int PreferredOrder() {
            throw new System.NotImplementedException();
        }


        public virtual long VGUID() {
            throw new System.NotImplementedException();
        }


        public virtual bool IsNativeToVixenPlus() {
            return false;
        }


        public virtual bool CanSave() {
            return false;
        }


        public virtual void Save(EventSequence eventSequence) {
            throw new System.NotImplementedException();
        }


        public virtual bool CanLoad() {
            return false;
        }


        public virtual void Load() {
            throw new System.NotImplementedException();
        }


        protected static void SaveCommon(XmlDocument contextNode, EventSequence eventSequence) {
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