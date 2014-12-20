using System;
using System.Globalization;
using System.Linq;
using System.Xml;

using VixenPlus;
using VixenPlus.Annotations;

using VixenPlusCommon;

namespace SeqIOHelpers {
    [UsedImplicitly]
    public class Vixen21SeqIO : SeqIOBase {

        public override string DialogFilterList() {
            return string.Format("Vixen 2.1 Sequence (*{0})|*{0}", FileExtension());
        }


        public override int VendorId() {
            return Vendor.Vixen21;
        }


        public override int PreferredOrder() {
            return 1;
        }


        public override long VGUID() {
            return 21L;
        }


        public override bool IsNativeToVixenPlus() {
            return false;
        }


        public override bool CanSave() {
            return true;
        }


        public override void Save(EventSequence eventSequence) {
            var contextNode = Xml.CreateXmlDocument();
            BaseSave(contextNode, eventSequence, FormatChannel);
            contextNode.Save(eventSequence.FileName);
        }


        private static XmlNode FormatChannel(XmlDocument doc, Channel ch) {
            XmlNode node = doc.CreateElement("Channel");
            node.InnerText = ch.Name;

            Xml.SetAttribute(node, "color", ch.Color.ToArgb().ToString(CultureInfo.InvariantCulture));
            Xml.SetAttribute(node, "output", (ch.OutputChannel - 1).ToString(CultureInfo.InvariantCulture));
            Xml.SetAttribute(node, "id", ch.Id.ToString(CultureInfo.InvariantCulture));
            Xml.SetAttribute(node, "enabled", ch.Enabled.ToString());

            if (ch.DimmingCurve != null) {
                Xml.SetValue(node, "Curve", string.Join(",", ch.DimmingCurve.Select(num => num.ToString(CultureInfo.InvariantCulture)).ToArray()));
            }

            return node;
        }


        public override bool CanLoad() {
            return true;
        }
    }
}
