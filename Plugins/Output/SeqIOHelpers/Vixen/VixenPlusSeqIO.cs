using System;
using System.Globalization;
using System.Linq;
using System.Xml;

using VixenPlus;
using VixenPlus.Annotations;

using VixenPlusCommon;

namespace SeqIOHelpers {
    [UsedImplicitly]
    public class VixenPlusSeqIO : SeqIOBase {

        public override string DialogFilterList() {
            return "Vixen Plus format (*.vix)|*.vix";
        }


        public override int VendorId() {
            return Vendor.VixenPlus;
        }


        public override int PreferredOrder() {
            return 0;
        }


        public override long VGUID() {
            return 2271965L;
        }


        public override bool IsNativeToVixenPlus() {
            return true;
        }


        public override bool CanSave() {
            return true;
        }


        public override void Save(EventSequence eventSequence) {
            var contextNode = Xml.CreateXmlDocument();
            SaveCommon(contextNode, eventSequence);
            SaveSpecific(contextNode,eventSequence);
            contextNode.Save(eventSequence.FileName);
        }



        private static void SaveSpecific(XmlNode contextNode, EventSequence eventSequence) {
            var doc = contextNode.OwnerDocument ?? ((XmlDocument)contextNode);
            var programNode = Xml.GetNodeAlways(contextNode, "Program");

            if (eventSequence.Profile == null) {
                //Channels
                var channelNodes = Xml.GetEmptyNodeAlways(programNode, "Channels");
                foreach (var channel in eventSequence.FullChannels) {
                    channelNodes.AppendChild(FormatChannel(doc, channel));
                }

                //Plugins
                if (programNode.OwnerDocument != null) {
                    programNode.AppendChild(programNode.OwnerDocument.ImportNode(eventSequence.PlugInData.RootNode, true));
                }

                //Groups
                Group.SaveToXml(programNode, eventSequence.Groups);
            } else  {
                Xml.SetValue(programNode, "Profile", eventSequence.Profile.Name);
            }
        }


        private static XmlNode FormatChannel(XmlDocument doc, Channel ch) {
            XmlNode node = doc.CreateElement("Channel");

            Xml.SetAttribute(node, "name", ch.Name);
            Xml.SetAttribute(node, "color", ch.Color.ToArgb().ToString(CultureInfo.InvariantCulture));
            Xml.SetAttribute(node, "output", (ch.OutputChannel - 1).ToString(CultureInfo.InvariantCulture));
            Xml.SetAttribute(node, "enabled", ch.Enabled.ToString());
            
            if (ch.DimmingCurve != null) {
                Xml.SetValue(node, "Curve", string.Join(",", ch.DimmingCurve.Select(num => num.ToString(CultureInfo.InvariantCulture)).ToArray()));
            }
            
            return node;
        }

        public override bool CanLoad() {
            return true;
        }


        public override void Load() {
            throw new NotImplementedException();
        }
    }
}