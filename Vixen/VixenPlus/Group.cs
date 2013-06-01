using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Security;
using System.Windows.Forms;
using System.Xml;

using Properties;

namespace VixenPlus {
    public class Group {
        public const char GroupTextDivider = '~';
        public static string AllChannels = Resources.AllChannels;
        public static string ManageGroups = "Manage Groups";
        private readonly List<Channel> _currentList = new List<Channel>();


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
                            if (String.IsNullOrEmpty(contains)) contains = null;
                            var color = node.Attributes["Color"] == null ? Color.White : Color.FromArgb(Int32.Parse(node.Attributes["Color"].Value));
                            var zoom = node.Attributes["Zoom"] == null ? "100%" : node.Attributes["Zoom"].Value;
                            var text = node.InnerText != "" ? node.InnerText : String.Empty;
                            groups.Add(name,
                                       new GroupData {
                                           Name = name, GroupColor = color, GroupChannels = (contains == null ? "" : contains + GroupTextDivider) + text,
                                           Zoom = zoom
                                       });
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


        public List<Channel> GetGroupChannels(string nodeData, Dictionary<string, GroupData> groups, List<Channel> fullChannelList) {
            try {
                var groupChannels = groups[nodeData].GroupChannels;
                if (groupChannels.Contains(GroupTextDivider.ToString(CultureInfo.InvariantCulture))) {
                    var nodes = groupChannels.Split(new[] {GroupTextDivider});
                    foreach (var recursiveNode in nodes[0].Split(new[] {','})) {
                        GetGroupChannels(recursiveNode, groups, fullChannelList);
                    }
                    if (nodes[1] != string.Empty) {
                        AddChannels(groupChannels, fullChannelList);
                    }
                }
                else {
                    AddChannels(groupChannels, fullChannelList);
                }
            }
            catch (KeyNotFoundException) {
                // we just build the group anyhow since it may have channels missing because of an improper formatted group file.
            }
            return _currentList;
        }


        private void AddChannels(string nodeData, IList<Channel> fullChannelList) {
            if (nodeData.Contains(GroupTextDivider.ToString(CultureInfo.InvariantCulture))) {
                nodeData = nodeData.Split(new[] {GroupTextDivider})[1];
            }
            var node = nodeData.Split(new[] {','});
            foreach (var channelNumber in node) {
                int channel;
                if (!Int32.TryParse(channelNumber, out channel)) {
                    continue;
                }
                if (!_currentList.Contains(fullChannelList[channel])) {
                    _currentList.Add(fullChannelList[channel]);
                }
            }
        }
    }
}
