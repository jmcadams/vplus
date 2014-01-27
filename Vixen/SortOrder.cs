using System.Collections.Generic;
using System.Text;
using System.Xml;

public class SortOrder {
    public readonly List<int> ChannelIndexes;
    public readonly string Name;


    public SortOrder(XmlNode node) {
        if (node.Attributes != null) {
            Name = node.Attributes["name"].Value;
        }
        ChannelIndexes = new List<int>();
        foreach (var str in node.InnerText.Split(new[] {','})) {
            int num;
            if (int.TryParse(str, out num)) {
                ChannelIndexes.Add(num);
            }
        }
    }


    public SortOrder(string name, IEnumerable<int> indexes) {
        Name = name;
        ChannelIndexes = new List<int>();
        ChannelIndexes.AddRange(indexes);
    }


/*
    internal SortOrder Clone() {
        return new SortOrder(Name, ChannelIndexes);
    }
*/


/*
    public int FindNaturalIndex(int sortedIndex) {
        return ChannelIndexes.IndexOf(sortedIndex);
    }
*/


    public XmlNode SaveToXml(XmlDocument doc) {
        XmlNode node = doc.CreateElement("SortOrder");
        Xml.SetAttribute(node, "name", Name);
        var builder = new StringBuilder();
        foreach (var num in ChannelIndexes) {
            builder.AppendFormat("{0},", num);
        }
        node.InnerText = builder.ToString().TrimEnd(new[] {','});
        return node;
    }


    public override string ToString() {
        return Name;
    }
}