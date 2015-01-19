 // todo: this is all terribly hacky, need to revist when the time allows

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

using VixenPlus.Properties;

using VixenPlusCommon;

namespace VixenPlus {
    public class Group {
        public static readonly string AllChannels = Resources.AllChannels;
        public const string GroupTextDivider = "~";

        private readonly List<Channel> _currentList = new List<Channel>();

        private static Dictionary<string, GroupData> ParseGroups(XmlNode doc){
            Dictionary<string, GroupData> groups = null;
            try {
                if (doc != null && doc.ParentNode != null) {
                    var nodes = doc.ParentNode["Groups"];
                    if (nodes != null) {
                        groups = new Dictionary<string, GroupData>();
                        foreach (XmlNode node in nodes) {
                            AddNodeToGroup(doc, node, groups);
                        }
                    }
                }
            }
            catch (Exception) {
                var msg = String.Format(Resources.ErrorLoadingGroup, doc, Vendor.ProductName);
                if (MessageBox.Show(msg, Resources.GroupLoadingError, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel) {
                    throw;
                }
            }
            return groups;
        }


        private static void AddNodeToGroup(XmlNode doc, XmlNode node, IDictionary<string, GroupData> groups) {
            if (node == null || node.Attributes == null) {
                return;
            }

            var name = node.Attributes["Name"] == null ? null : node.Attributes["Name"].Value;
            if (name == null) {
                throw new XmlSyntaxException(String.Format(Resources.MissingNameAttribute, doc, node));
            }
            var isSortOrder = node.Attributes["IsSortOrder"] != null && bool.Parse(node.Attributes["IsSortOrder"].Value);
            var color = node.Attributes["Color"] == null ? (isSortOrder ? Color.Black : Color.White) : Color.FromArgb(Int32.Parse(node.Attributes["Color"].Value));
            var zoom = node.Attributes["Zoom"] == null ? "100%" : node.Attributes["Zoom"].Value;
            var text = String.Empty;
            foreach (var child in node.ChildNodes.Cast<XmlNode>().Where(child => child.InnerText != "")) {
                switch (child.Name) {
                    case "Channels":
                        text += child.InnerText + ",";
                        break;
                    case "Contains":
                        text = child.InnerText.Split(',').Aggregate(text, (current, @group) => current + (GroupTextDivider + @group + ","));
                        break;
                }
            }
            groups.Add(name,
                new GroupData {Name = name, GroupColor = color, GroupChannels = text.TrimEnd(','), Zoom = zoom, IsSortOrder = isSortOrder});
        }


        private static void AddInnerText(string nodeData, XContainer thisNode) {
            var previousType = String.Empty;
            var treeData = new StringBuilder();
            foreach (var child in nodeData.Split(',')) {
                var nodeValue = child;
                var currentType = "Channels";
                if (child.Contains("~")) {
                    nodeValue = child.Remove(0, 1);
                    currentType = "Contains";
                }
                if (string.IsNullOrEmpty(previousType)) {
                    previousType = currentType;
                }
                if (currentType != previousType) {
                    thisNode.Add(new XElement(previousType, treeData.Remove(treeData.Length - 1, 1)));
                    previousType = currentType;
                    treeData.Remove(0, treeData.Length);
                }
                treeData.Append(nodeValue + ",");
            }
            if (treeData.Length > 0) {
                thisNode.Add(new XElement(previousType, treeData.Remove(treeData.Length - 1, 1)));
            }
        }


        public static Dictionary<string, GroupData> GetGroups(TreeView groupTree) {
            var xml = new XmlDocument();
            xml.LoadXml(GetGroupXml(groupTree).ToString());

            return ParseGroups(xml.DocumentElement);
        }


        public static void SaveToXml(XmlNode contextNode, Dictionary<string, GroupData> groupDictionary) {
            var groupsNode = new XElement("Groups");
            foreach (var entry in groupDictionary) {
                var val = entry.Value;
                XContainer node = new XElement("Group");
                node.Add(new XAttribute("Name", val.Name), new XAttribute("Zoom", val.Zoom),
                    new XAttribute("Color", val.GroupColor.ToArgb()), new XAttribute("IsSortOrder", val.IsSortOrder));
                AddInnerText(val.GroupChannels, node);
                groupsNode.Add(node);
            }
            ImportXElementToXml(contextNode, groupsNode);
        }


        private static void ImportXElementToXml(XmlNode contextNode, XElement groupsNode) {
            if (null == contextNode.OwnerDocument) {
                return;
            }

            var xD = new XmlDocument();
            xD.LoadXml(groupsNode.ToString());
            var xN = xD.FirstChild;
            var importNode = contextNode.OwnerDocument.ImportNode(xN, true);

            contextNode.AppendChild(importNode);   
        }


        private static XElement GetGroupXml(TreeView groupData) {
            var doc = new XElement("Groups");
            foreach (TreeNode node in groupData.Nodes) {
                var nodeData = ((GroupTagData)node.Tag);
                var thisNode = new XElement("Group");
                thisNode.Add(new XAttribute("Name", node.Name), new XAttribute("Zoom", nodeData.Zoom),
                    new XAttribute("Color", nodeData.NodeColor.ToArgb()), new XAttribute("IsSortOrder", nodeData.IsSortOrder));
                var previousType = String.Empty;
                var treeData = String.Empty;
                foreach (TreeNode child in node.Nodes) {
                    var tag = ((GroupTagData)child.Tag);
                    var currentType = tag.IsLeafNode ? "Channels" : "Contains";
                    if (string.IsNullOrEmpty(previousType)) {
                        previousType = currentType;
                    }
                    if (currentType != previousType) {
                        thisNode.Add(new XElement(previousType, treeData.TrimEnd(',')));
                        treeData = (tag.IsLeafNode ? tag.UnderlyingChannel : child.Name) + ",";
                        previousType = currentType;
                    }
                    else {
                        treeData += (tag.IsLeafNode ? tag.UnderlyingChannel : child.Name) + ",";
                    }
                }
                if (!String.IsNullOrEmpty(treeData)) {
                    thisNode.Add(new XElement(previousType, treeData.TrimEnd(',')));
                }
                doc.Add(thisNode);
            }

            return doc;
        }


        internal List<Channel> GetGroupChannels(string nodeData, Dictionary<string, GroupData> groups, List<Channel> fullChannelList) {
            try {
                var groupChannels = groups[nodeData].GroupChannels;
                foreach (var node in groupChannels.Split(',')) {
                    if (node.StartsWith(GroupTextDivider)) {
                        GetGroupChannels(node.TrimStart(GroupTextDivider.ToCharArray()), groups, fullChannelList);
                    }
                    else {
                        int channel;
                        if (Int32.TryParse(node, out channel) && channel < fullChannelList.Count && !_currentList.Contains(fullChannelList[channel])) {
                            _currentList.Add(fullChannelList[channel]);
                        }
                    }
                }
            }
            catch (KeyNotFoundException) {
                // we just build the group anyhow since it may have channels missing because of an improper formatted group file.
            }
            return _currentList;
        }

        public static Dictionary<string, GroupData> LoadFromXml(XmlNode contextNode) {
            var groups = new Dictionary<string, GroupData>();

            var baseNode = contextNode.SelectSingleNode("SortOrders");
            if (null != baseNode) {
                var sortNodes = baseNode.SelectNodes("SortOrder");
                if (null != sortNodes) {
                    foreach (XmlNode so in sortNodes) {
                        if (null == so.Attributes || null == so.Attributes["name"]) {
                            continue;
                        }
                        XContainer node = new XElement("Group");
                        node.Add(new XAttribute("Name", so.Attributes["name"].Value + " (Sort Order)"), new XAttribute("Zoom", "100%"),
                            new XAttribute("Color", Color.Black.ToArgb()), new XAttribute("IsSortOrder", "True"));
                        var channels = so.InnerText;
                        AddInnerText(channels, node);
                        var xD = new XmlDocument();
                        xD.LoadXml(node.ToString());
                        var sortNode = xD.FirstChild;
                        AddNodeToGroup(baseNode, sortNode, groups);
                    }
                }
            }

            // Get the Groups node or exit
            baseNode = contextNode.SelectSingleNode("Groups");
            // ReSharper disable InvertIf
            if (null != baseNode) {
                // ReSharper enable InvertIf
                foreach (XmlNode groupNode in baseNode) {
                    AddNodeToGroup(baseNode, groupNode, groups);
                }
            }

            return groups.Count > 0 ? groups : null;
        }


        public static bool LoadFromFile(XmlNode contextNode, Dictionary<string, GroupData> groups) {
            var uri = contextNode.BaseURI;

            var path = Path.GetDirectoryName(new Uri(uri).LocalPath);
            if (path == null) {
                return false;
            }

            var file = Path.Combine(path, Path.GetFileNameWithoutExtension(uri) + Vendor.GroupExtension);

            if (!File.Exists(file)) {
                return false;
            }

            var dirtied = false;

            var doc = Xml.LoadDocument(file).DocumentElement;
            if (doc != null && doc.ParentNode != null && doc.SelectSingleNode("Groups") != null ) {
                var nodes = doc.SelectSingleNode("Groups");
                if (nodes != null) {
                    if (null == groups) {
                        groups = new Dictionary<string, GroupData>();
                    }
                    foreach (var node in nodes.Cast<XmlNode>().Where(node => null != node.Attributes && null != node.Attributes["Name"] && !groups.ContainsKey(node.Attributes["Name"].Value))) {
                        AddNodeToGroup(doc, node, groups);
                        dirtied = true;
                    }
                }
            }

            return dirtied;
        }
    }
}