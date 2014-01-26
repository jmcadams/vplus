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

public class Group {
    public static readonly string AllChannels = Resources.AllChannels;
    public const string ManageGroups = "Manage Groups";
    public const string GroupTextDivider = "~";

    private readonly List<Channel> _currentList = new List<Channel>();

    public static Dictionary<string, GroupData> LoadGroups(string groupFile) {
        return ParseGroups(Xml.LoadDocument(groupFile).DocumentElement);
    }


    private static Dictionary<string, GroupData> ParseGroups(XmlElement doc){
        Dictionary<string, GroupData> groups = null;
        try {
            if (doc != null && doc.ParentNode != null) {
                var nodes = doc.ParentNode["Groups"];
                if (nodes != null) {
                    groups = new Dictionary<string, GroupData>();
                    foreach (XmlNode node in nodes) {
                        if (node == null || node.Attributes == null) {
                            continue;
                        }
                        var name = node.Attributes["Name"] == null ? null : node.Attributes["Name"].Value;
                        if (name == null) {
                            throw new XmlSyntaxException(String.Format(Resources.MissingNameAttribute, doc, node));
                        }
                        var color = node.Attributes["Color"] == null ? Color.White : Color.FromArgb(Int32.Parse(node.Attributes["Color"].Value));
                        var zoom = node.Attributes["Zoom"] == null ? "100%" : node.Attributes["Zoom"].Value;
                        var text = String.Empty;
                        foreach (XmlNode child in node.ChildNodes) {
                            if (child.InnerText == "") {
                                continue;
                            }
                            switch (child.Name) {
                                case "Channels":
                                    text += child.InnerText + ",";
                                    break;
                                case "Contains":
                                    text = child.InnerText.Split(new[] {','}).Aggregate(text,
                                        (current, @group) =>
                                            current + (GroupTextDivider + @group + ","));
                                    break;
                            }
                        }
                        groups.Add(name, new GroupData {Name = name, GroupColor = color, GroupChannels = text.TrimEnd(new[] {','}), Zoom = zoom});
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


    public static void SaveGroups(Dictionary<string, GroupData> groups, string filename) {
        var doc = new XElement("Groups");
        foreach (var node in groups) {
            var nodeData = node.Value;
            var thisNode = new XElement("Group");
            thisNode.Add(new XAttribute("Name", nodeData.Name), new XAttribute("Zoom", nodeData.Zoom),
                new XAttribute("Color", nodeData.GroupColor.ToArgb()));
            var previousType = String.Empty;
            var treeData = new StringBuilder();
            foreach (var child in nodeData.GroupChannels.Split(new[] {','})) {
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
            doc.Add(thisNode);
        }
        // ReSharper disable once AssignNullToNotNullAttribute
        var groupFilename = (Path.Combine(Path.GetDirectoryName(filename), Path.GetFileNameWithoutExtension(filename) + Vendor.GroupExtension));
        doc.Save(groupFilename);
    }


    public static Dictionary<string, GroupData> ParseDialogResults(TreeView groupTree) {
        var doc = new XElement("Groups");
        foreach (TreeNode node in groupTree.Nodes) {
            var nodeData = ((GroupTagData) node.Tag);
            var thisNode = new XElement("Group");
            thisNode.Add(new XAttribute("Name", node.Name), new XAttribute("Zoom", nodeData.Zoom), new XAttribute("Color", nodeData.NodeColor.ToArgb()));
            var previousType = String.Empty;
            var treeData = String.Empty;
            foreach (TreeNode child in node.Nodes) {
                var tag = ((GroupTagData) child.Tag);
                var currentType = tag.IsLeafNode ? "Channels" : "Contains";
                if (string.IsNullOrEmpty(previousType)) {
                    previousType = currentType;
                }
                if (currentType != previousType) {
                    thisNode.Add(new XElement(previousType, treeData.TrimEnd(new[] {','})));
                    treeData = (tag.IsLeafNode ? tag.UnderlyingChannel : child.Name) + ",";
                    previousType = currentType;
                }
                else {
                    treeData += (tag.IsLeafNode ? tag.UnderlyingChannel : child.Name) + ",";
                }
            }
            if (!String.IsNullOrEmpty(treeData)) {
                thisNode.Add(new XElement(previousType, treeData.TrimEnd(new[] { ',' })));
            }
            doc.Add(thisNode);
        }
        var groupFilename = (Path.Combine(Paths.ProfilePath,
            Path.GetTempFileName() + Vendor.GroupExtension));
        doc.Save(groupFilename);
        var result = LoadGroups(groupFilename);
        File.Delete(groupFilename);

        return result;
    }


    internal List<Channel> GetGroupChannels(string nodeData, Dictionary<string, GroupData> groups, List<Channel> fullChannelList) {
        try {
            var groupChannels = groups[nodeData].GroupChannels;
            foreach (var node in groupChannels.Split(new[] {','})) {
                if (node.StartsWith(GroupTextDivider)) {
                    GetGroupChannels(node.TrimStart(GroupTextDivider.ToCharArray()), groups, fullChannelList);
                }
                else {
                    int channel;
                    if (Int32.TryParse(node, out channel) && !_currentList.Contains(fullChannelList[channel])) {
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
}