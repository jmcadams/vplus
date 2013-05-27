using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Security;
using System.Windows.Forms;
using System.Xml;

using Properties;

namespace VixenPlus {
    public class Group {
        public static Dictionary<string, GroupData> LoadGroups(string groupFile) {
            Dictionary<string, GroupData> groups = null;
            try {
                var doc = Xml.LoadDocument(groupFile).DocumentElement;
                if (doc != null && doc.ParentNode != null) {
                    XmlNode nodes = doc.ParentNode["Groups"];
                    if (nodes != null) {
                        groups = new Dictionary<string, GroupData>();
                        foreach (XmlNode node in nodes) {
                            if (node == null || node.Attributes == null) {
                                continue;
                            }
                            var name = node.Attributes["Name"] == null ? null : node.Attributes["Name"].Value;
                            if (name == null) {
                                throw new XmlSyntaxException(String.Format(Resources.MissingNameAttribute, Path.GetFileName(groupFile), node));
                            }
                            var contains = node.Attributes["Contains"] == null ? null : node.Attributes["Contains"].Value;
                            var color = node.Attributes["Color"] == null ? Color.White : Color.FromArgb(int.Parse(node.Attributes["Color"].Value));
                            var zoom = node.Attributes["Zoom"] == null ? "100%" : node.Attributes["Zoom"].Value;
                            var text = node.InnerText != "" ? node.InnerText : string.Empty;
                            groups.Add(name,
                                       new GroupData
                                       {Name = name, GroupColor = color, GroupChannels = (contains == null ? "" : contains + ":") + text, Zoom = zoom});
                        }
                    }
                }
            }
            catch (Exception) {
                var msg = String.Format(Resources.ErrorLoadingGroup, Path.GetFileName(groupFile), Vendor.ProductName);
                if (MessageBox.Show(msg, Resources.GroupLoadingError, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel) {
                    throw;
                }
            }
            return groups;
        }
    }

    public class GroupData {
        public string Name { get; set; }
        public Color GroupColor { get; set; }
        public string GroupChannels { get; set; }
        public string Zoom { get; set; }
    }
}
