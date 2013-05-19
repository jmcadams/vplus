using System.Collections.Generic;
using System.Xml;

namespace VixenPlus {
    public class Group {
        public static Dictionary<string, string> LoadGroups(string groupFile) {
            Dictionary<string, string> groups = null;
            var doc = Xml.LoadDocument(groupFile).DocumentElement;
            if (doc != null && doc.ParentNode != null) {
                XmlNode nodes = doc.ParentNode["Groups"];
                if (nodes != null) {
                    groups = new Dictionary<string, string>();
                    foreach (XmlNode node in nodes) {
                        if (node == null || node.Attributes == null) {
                            continue;
                        }
                        var name = node.Attributes["Name"] != null ? node.Attributes["Name"].Value : null;
                        var contains = node.Attributes["Contains"] != null ? node.Attributes["Contains"].Value : null;
                        var text = node.InnerText != "" ? node.InnerText : string.Empty;
                        if (contains != null) {
                            groups.Add(name, contains + ":" + text);
                        }
                        else {
                            groups.Add(name, text);
                        }
                    }
                }
            }
            return groups;
        }
    }
}
