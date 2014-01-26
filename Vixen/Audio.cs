using System;
using System.Globalization;
using System.Xml;

public class Audio {
    public Audio() {}


    public Audio(XmlNode node) {
        if (node.Attributes == null) {
            return;
        }

        Name = node.Attributes["name"].Value;
        FileName = node.InnerText;
        Duration = Convert.ToInt32(node.Attributes["duration"].Value);
    }


    public Audio(string name, string filename, int duration) {
        Name = name;
        FileName = filename;
        Duration = duration;
    }


    public int Duration { get; set; }

    public string FileName { get; set; }

    public string Name { get; set; }


    public XmlNode SaveToXml(XmlDocument doc) {
        XmlNode node = doc.CreateElement("Audio");
        node.InnerText = FileName;
        Xml.SetAttribute(node, "name", Name);
        Xml.SetAttribute(node, "duration", Duration.ToString(CultureInfo.InvariantCulture));
        return node;
    }


    public override string ToString() {
        return Name;
    }
}