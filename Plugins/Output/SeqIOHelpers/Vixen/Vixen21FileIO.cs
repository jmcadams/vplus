using System.Globalization;
using System.Xml;

using VixenPlus;
using VixenPlus.Annotations;

namespace SeqIOHelpers {
    [UsedImplicitly]
    public class Vixen21FileIO : VixenFileIOBase {

        public override string DialogFilterList() {
            return string.Format("Vixen 2.1 Sequence (*{0})|*{0}", FileExtension());
        }


        public override string Name() {
            return "Vixen 2.1";
        }


        public override int PreferredOrder() {
            return 2;
        }


        public override bool IsNativeToVixenPlus() {
            return false;
        }


        public override bool CanSave() {
            return true;
        }


        public override Profile OpenProfile(string fileName) {
            return BaseOpenProfile(fileName, this);
        }


        public override EventSequence OpenSequence(string fileName) {
            return BaseOpenSequence(fileName, this);
        }


        public override void SaveSequence(EventSequence eventSequence) {
            var contextNode = Xml.CreateXmlDocument();
            BaseSaveSequence(contextNode, eventSequence, FormatChannel);
            contextNode.Save(eventSequence.FileName);
        }


        public override void SaveProfile(Profile profile) {
            var profileXml = Xml.CreateXmlDocument("Profile");

            BaseSaveProfile(profileXml, profile, FormatChannel);
            var profileNode = Xml.GetNodeAlways(profileXml, "Profile");
            BaseSaveSortOrders(profileNode, profile);
            BaseSaveNativeData(profile.FileName, profile.Groups);

            profileXml.Save(profile.FileName);
            profile.IsDirty = false;
        }


        private static XmlNode FormatChannel(XmlDocument doc, Channel ch) {
            XmlNode node = doc.CreateElement("Channel");
            node.InnerText = ch.Name;

            Xml.SetAttribute(node, "color", ch.Color.ToArgb().ToString(CultureInfo.InvariantCulture));
            Xml.SetAttribute(node, "output", (ch.OutputChannel).ToString(CultureInfo.InvariantCulture));
            Xml.SetAttribute(node, "id", ch.Id.ToString(CultureInfo.InvariantCulture));
            Xml.SetAttribute(node, "enabled", ch.Enabled.ToString());

            return node;
        }


        public override bool CanOpen() {
            return true;
        }
    }
}
