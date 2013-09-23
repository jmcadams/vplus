using System.Collections;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml;
using System.Collections.Generic;
using System;

namespace VixenEditor {
    internal class ChannelMapperProfile {
        private List<ChannelMapperChannel> _channels = new List<ChannelMapperChannel>();
        private readonly Color _defaultColor = Color.Black;
        private readonly string _fileName;
        private int _channelCount;


        public ChannelMapperProfile(string fileName) {
            _fileName = fileName;
            ParseFile();
        }


        public string GetFileName() {
            return Path.GetFileNameWithoutExtension(_fileName);
        } 

        private void ParseFile() {
            var xml = new XmlDocument();
            xml.Load(_fileName);
            var nodes = xml.SelectNodes("Profile/ChannelObjects/Channel");

            if (nodes == null) return;

            _channelCount = nodes.Count;
            InitializeChannelInfo();

            ProcessChannels(nodes);
        }


        private void InitializeChannelInfo() {
            for (var i = 0; i < _channelCount; i++) {
                _channels.Add(new ChannelMapperChannel(i, i, _defaultColor, "Undefined", false));
            }
        }


        private void ProcessChannels(IEnumerable nodes) {
            var holdColor = _defaultColor;
            var channelNum = 0;
            var currentNode = 0;

            foreach (XmlNode node in nodes) {
                var enabled = false;
                var holdName = node.InnerText;
                if (node.Attributes != null) {
                    foreach (XmlAttribute attribute in node.Attributes) {
                        switch (attribute.Name) {
                            case "color":
                                holdColor = Color.FromArgb(int.Parse(attribute.Value));
                                break;

                            case "output":
                                channelNum = int.Parse(attribute.Value);
                                break;

                            case "enabled":
                                enabled = bool.Parse(attribute.Value);
                                break;

                            case "name":
                                holdName = attribute.Value;
                                break;
                        }
                    }
                }

                _channels[channelNum].Enabled = enabled;
                _channels[channelNum].Name = holdName;
                _channels[channelNum].Color = holdColor.ToArgb();
                _channels[channelNum].Location = currentNode++;
            }
        }


        internal int GetChannelCount() {
            return _channelCount;
        }


        internal string GetChannelName(int channel) {
            CheckChannel(channel);
            return (_channels[channel].Enabled ? "" : "<Disabled> ") + _channels[channel].Number + ":" + _channels[channel].Name;
        }


        internal int GetChannelLocation(int channel) {
            return _channels[channel].Location;
        }


        internal Color GetChannelColor(int channel) {
            return Color.FromArgb(_channels[channel].Color);
        }


        private void CheckChannel(int channel) {
            if (channel > _channelCount) {
                throw new ArgumentOutOfRangeException("Maximum value: " + _channelCount + " received: " + channel);
            }
        }


        public ChannelMapperChannel GetChannel(int i) {
            return _channels[i];
        }

        public void SetSortOrder(int sortType) {
            switch (sortType) {
                case 0:
                    _channels = _channels.OrderBy(channel => channel.Number).ToList();
                    break;
                case 1:
                    _channels = _channels.OrderBy(channel => channel.Name).ToList();
                    break;
                case 2:
                    _channels = _channels.OrderBy(channel => channel.Color).ToList();
                    break;
                case 3:
                    _channels = _channels.OrderBy(channel => channel.Name).ThenBy(channel => channel.Color).ToList();
                    break;
                case 4:
                    _channels = _channels.OrderBy(channel => channel.Color).ThenBy(channel => channel.Name).ToList();
                    break;
            }
        }
    }
}
