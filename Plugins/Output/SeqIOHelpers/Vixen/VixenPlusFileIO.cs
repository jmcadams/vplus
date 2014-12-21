﻿using System.Globalization;
using System.Linq;
using System.Xml;

using VixenPlus;
using VixenPlus.Annotations;

namespace SeqIOHelpers {
    [UsedImplicitly]
    public class VixenPlusFileIO : VixenFileIOBase {

        public override string DialogFilterList() {
            return string.Format("Vixen Plus Sequence (*{0})|*{0}", FileExtension());
        }

        public override int PreferredOrder() {
            return 0;
        }


        public override bool IsNativeToVixenPlus() {
            return true;
        }


        public override bool CanSave() {
            return true;
        }


        public override void SaveSequence(EventSequence eventSequence) {
            var contextNode = Xml.CreateXmlDocument();

            BaseSaveSequence(contextNode, eventSequence, FormatChannel);

            var programNode = Xml.GetNodeAlways(contextNode, "Program");

            if (eventSequence.Profile == null) {
                Group.SaveToXml(programNode, eventSequence.Groups);
            }

            contextNode.Save(eventSequence.FileName);
        }


        public override void SaveProfile(Profile profile) {
            throw new System.NotImplementedException();
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

        public override bool CanOpen() {
            return true;
        }
    }
}