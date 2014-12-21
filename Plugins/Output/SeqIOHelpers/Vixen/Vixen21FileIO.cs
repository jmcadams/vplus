using System.Globalization;
using System.Linq;
using System.Xml;

using VixenPlus;
using VixenPlus.Annotations;

namespace SeqIOHelpers {
    [UsedImplicitly]
    public class Vixen21FileIO : VixenFileIOBase {

        public override string DialogFilterList() {
            return string.Format("Vixen 2.1 Sequence (*{0})|*{0}", FileExtension());
        }


        public override int PreferredOrder() {
            return 1;
        }


        public override bool IsNativeToVixenPlus() {
            return false;
        }


        public override bool CanSave() {
            return true;
        }


        public override void SaveSequence(EventSequence eventSequence) {
            var contextNode = Xml.CreateXmlDocument();
            BaseSaveSequence(contextNode, eventSequence, FormatChannel);
            contextNode.Save(eventSequence.FileName);
        }




        private static XmlNode FormatChannel(XmlDocument doc, Channel ch) {
            XmlNode node = doc.CreateElement("Channel");
            node.InnerText = ch.Name;

            Xml.SetAttribute(node, "color", ch.Color.ToArgb().ToString(CultureInfo.InvariantCulture));
            Xml.SetAttribute(node, "output", (ch.OutputChannel - 1).ToString(CultureInfo.InvariantCulture));
            Xml.SetAttribute(node, "id", ch.Id.ToString(CultureInfo.InvariantCulture));
            Xml.SetAttribute(node, "enabled", ch.Enabled.ToString());

            return node;
        }


        public override bool CanOpen() {
            return true;
        }
    }
}
