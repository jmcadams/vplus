using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

using CommonUtils;

using VixenPlus;
using VixenPlus.Properties;

namespace VixenEditor {
    internal static class ToolStripManager {

        internal const int IconSizeSmall = 24;
        internal const int IconSizeMedium = 36;
        internal const int IconSizeLarge = 48;

        private static int _iconSize = IconSizeMedium;
        private static Image _onBackground;


        public static int IconSize {
            get { return _iconSize; }
            set {
                if (value == _iconSize || (value != IconSizeSmall && value != IconSizeMedium && value != IconSizeLarge)) {
                    return;
                }
                _iconSize = value;
                ResizeBackgroundImages();
            }
        }


        private static void ResizeBackgroundImages() {
            _onBackground = Resources.Ball_Green.ResizeImage(_iconSize);
        }


        public static Image GetBackground(bool isOn) {
            if (_onBackground == null) {
                ResizeBackgroundImages();
            }
            return isOn ? _onBackground : null;
        }


        public static void LoadSettings(Form form, XmlNode parentNode) {
            LoadSettings(form, parentNode, form.GetType().ToString());
            ResizeToolStrips(form);
            ResizeBackgroundImages();
        }


        public static void LoadSettings(Form form, XmlNode parentNode, string key) {
            XmlNode node = parentNode["ToolStripConfiguration"];
            if (node == null) {
                return;
            }
            XmlNode node2 = node[key];
            if (node2 == null) {
                return;
            }
            var sizeNode = node2.SelectSingleNode("Size");
            if (sizeNode != null) {
                IconSize = int.Parse(sizeNode.InnerText);
            }

            var lockedNode = node2.SelectSingleNode("Locked");
            if (lockedNode != null) {
                Locked = bool.Parse(lockedNode.InnerText);
            }

            var crosshairNode = node2.SelectSingleNode("Crosshairs");
            if (crosshairNode != null) {
                Crosshairs = bool.Parse(crosshairNode.InnerText);
            }

            var waveformShownNode = node2.SelectSingleNode("WaveformShown");
            if (waveformShownNode != null) {
                WaveformShown = bool.Parse(waveformShownNode.InnerText);
            }

            foreach (Control control in form.Controls) {
                var toolStripContainer = control as ToolStripContainer;
                if (toolStripContainer == null) {
                    continue;
                }
                var toolStrips = toolStripContainer.TopToolStripPanel.Controls.Cast<ToolStrip>().ToList();
                toolStrips.AddRange(toolStripContainer.LeftToolStripPanel.Controls.Cast<ToolStrip>());
                toolStrips.AddRange(toolStripContainer.RightToolStripPanel.Controls.Cast<ToolStrip>());
                toolStrips.AddRange(toolStripContainer.BottomToolStripPanel.Controls.Cast<ToolStrip>());
                toolStripContainer.TopToolStripPanel.Controls.Clear();
                toolStripContainer.LeftToolStripPanel.Controls.Clear();
                toolStripContainer.RightToolStripPanel.Controls.Clear();
                toolStripContainer.BottomToolStripPanel.Controls.Clear();
                ReadPanel(node2.SelectSingleNode("*[@name=\"TopToolStripPanel\"]"), toolStripContainer.TopToolStripPanel, toolStrips);
                ReadPanel(node2.SelectSingleNode("*[@name=\"LeftToolStripPanel\"]"), toolStripContainer.LeftToolStripPanel, toolStrips);
                ReadPanel(node2.SelectSingleNode("*[@name=\"RightToolStripPanel\"]"), toolStripContainer.RightToolStripPanel, toolStrips);
                ReadPanel(node2.SelectSingleNode("*[@name=\"BottomToolStripPanel\"]"), toolStripContainer.BottomToolStripPanel, toolStrips);
                break;
            }
        }


        private static void ReadPanel(XmlNode panelNode, ToolStripPanel toolStripPanel, ICollection<ToolStrip> toolStrips) {
            if (toolStripPanel == null) {
                return;
            }
            var stripNodes = panelNode.SelectNodes("Strip");
            if (stripNodes == null) {
                return;
            }
            foreach (XmlNode node in stripNodes) {
                ToolStrip item = null;
                foreach (var strip2 in toolStrips) {
                    strip2.ImageScalingSize = new Size(IconSize, IconSize);
                    ResizeChildren(strip2.Items);

                    if (node.Attributes == null || strip2.Name != node.Attributes["name"].Value) {
                        continue;
                    }
                    item = strip2;
                    break;
                }
                if (item == null) {
                    continue;
                }
                toolStrips.Remove(item);

                var strArray = node.Attributes["location"].Value.Split(new[] {','});
                toolStripPanel.Join(item, int.Parse(strArray[0]), int.Parse(strArray[1]));

                item.Visible = bool.Parse(node.Attributes["visible"].Value);
                item.GripStyle = Locked ? ToolStripGripStyle.Hidden : ToolStripGripStyle.Visible;
            }
        }


        public static void SaveSettings(Form form, XmlNode parentNode) {
            SaveSettings(form, parentNode, form.GetType().ToString());
        }


        public static void SaveSettings(Form form, XmlNode parentNode, string key) {
            var emptyNodeAlways = Xml.GetEmptyNodeAlways(Xml.GetNodeAlways(parentNode, "ToolStripConfiguration"), key);
            Xml.SetNewValue(emptyNodeAlways, "Size", IconSize.ToString(CultureInfo.InvariantCulture));
            Xml.SetNewValue(emptyNodeAlways, "Locked", Locked.ToString());
            Xml.SetNewValue(emptyNodeAlways, "Crosshairs", Crosshairs.ToString());
            Xml.SetNewValue(emptyNodeAlways, "WaveformShown", WaveformShown.ToString());
            foreach (Control control in form.Controls) {
                var stripContainer = control as ToolStripContainer;
                if (stripContainer == null) {
                    continue;
                }
                WritePanel(emptyNodeAlways, stripContainer.TopToolStripPanel, "TopToolStripPanel");
                WritePanel(emptyNodeAlways, stripContainer.LeftToolStripPanel, "LeftToolStripPanel");
                WritePanel(emptyNodeAlways, stripContainer.RightToolStripPanel, "RightToolStripPanel");
                WritePanel(emptyNodeAlways, stripContainer.BottomToolStripPanel, "BottomToolStripPanel");
                break;
            }
        }


        private static void WritePanel(XmlNode parentNode, ToolStripPanel toolStripPanel, string toolStripPanelName) {
            var node = Xml.SetNewValue(parentNode, "Panel", string.Empty);
            Xml.SetAttribute(node, "name", toolStripPanelName);
            var list = new List<ToolStrip>();
            var list2 = new List<Control>();
            foreach (var row in toolStripPanel.Rows) {
                list2.Clear();
                list2.AddRange(row.Controls);
                list2.Sort(delegate(Control control1, Control control2) {
                    if (control1.Location.Y < control2.Location.Y) {
                        return -1;
                    }
                    if (control2.Location.Y >= control1.Location.Y) {
                        if (control1.Location.X == control2.Location.X) {
                            return 0;
                        }
                        if (control1.Location.X < control2.Location.X) {
                            return -1;
                        }
                    }
                    return 1;
                });
                list.AddRange(list2.Cast<ToolStrip>());
            }
            foreach (var control in list) {
                var node2 = Xml.SetNewValue(node, "Strip", string.Empty);
                Xml.SetAttribute(node2, "name", control.Name);
                Xml.SetAttribute(node2, "location", string.Format("{0},{1}", control.Location.X, control.Location.Y));
                Xml.SetAttribute(node2, "visible", control.Visible.ToString());
            }
        }


        public static void ResizeToolStrips(Form form) {
            foreach (Control control in form.Controls) {
                var container = control as ToolStripContainer;
                if (container == null) {
                    continue;
                }
                foreach (ToolStrip toolStrip in container.TopToolStripPanel.Controls) {
                    toolStrip.ImageScalingSize = new Size(IconSize, IconSize);
                    ResizeChildren(toolStrip.Items);
                }
            }
        }


        private static void ResizeChildren(IEnumerable toolStripItems) {
            foreach (var item in toolStripItems) {
                var button = item as ToolStripButton;
                if (button == null) {
                    var label = item as ToolStripLabel;
                    if (label != null) {
                        label.Size = new Size(label.Width, _iconSize);
                    }
                }
                else {
                    button.Size = new Size(IconSize, IconSize);
                    if (button.BackgroundImage != null) {
                        button.BackgroundImage = GetBackground(true);
                    }
                }
            }
        }


        public static bool Locked { get; set; }

        public static bool Crosshairs { get; set; }

        public static bool WaveformShown { get; set; }
    }
}