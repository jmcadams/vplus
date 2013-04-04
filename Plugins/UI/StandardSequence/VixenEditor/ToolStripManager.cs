namespace VixenEditor
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Forms;
    using System.Xml;
    using VixenPlus;
    using System.Drawing;

    internal static class ToolStripManager
    {

        internal const int ICON_SIZE_SMALL = 24;
        internal const int ICON_SIZE_MEDIUM = 32;
        internal const int ICON_SIZE_LARGE = 48;

        public static void LoadSettings(Form form, XmlNode parentNode)
        {
            LoadSettings(form, parentNode, form.GetType().ToString());
        }

        public static void LoadSettings(Form form, XmlNode parentNode, string key)
        {
            XmlNode node = parentNode["ToolStripConfiguration"];
            if (node != null)
            {
                XmlNode node2 = node[key];
                if (node2 != null)
                {
                    foreach (Control control in form.Controls)
                    {
                        if (control is ToolStripContainer)
                        {
                            ToolStripContainer container = (ToolStripContainer) control;
                            List<ToolStrip> toolStrips = new List<ToolStrip>();
                            foreach (ToolStrip strip in container.TopToolStripPanel.Controls)
                            {
                                toolStrips.Add(strip);
                            }
                            foreach (ToolStrip strip in container.LeftToolStripPanel.Controls)
                            {
                                toolStrips.Add(strip);
                            }
                            foreach (ToolStrip strip in container.RightToolStripPanel.Controls)
                            {
                                toolStrips.Add(strip);
                            }
                            foreach (ToolStrip strip in container.BottomToolStripPanel.Controls)
                            {
                                toolStrips.Add(strip);
                            }
                            container.TopToolStripPanel.Controls.Clear();
                            container.LeftToolStripPanel.Controls.Clear();
                            container.RightToolStripPanel.Controls.Clear();
                            container.BottomToolStripPanel.Controls.Clear();
                            ReadPanel(node2.SelectSingleNode("*[@name=\"TopToolStripPanel\"]"), container.TopToolStripPanel, toolStrips);
                            ReadPanel(node2.SelectSingleNode("*[@name=\"LeftToolStripPanel\"]"), container.LeftToolStripPanel, toolStrips);
                            ReadPanel(node2.SelectSingleNode("*[@name=\"RightToolStripPanel\"]"), container.RightToolStripPanel, toolStrips);
                            ReadPanel(node2.SelectSingleNode("*[@name=\"BottomToolStripPanel\"]"), container.BottomToolStripPanel, toolStrips);
                            break;
                        }
                    }
                }
            }
        }

        private static void ReadPanel(XmlNode panelNode, ToolStripPanel toolStripPanel, List<ToolStrip> toolStrips)
        {
            if (toolStripPanel != null)
            {
                foreach (XmlNode node in panelNode.SelectNodes("Strip"))
                {
                    ToolStrip item = null;
                    foreach (ToolStrip strip2 in toolStrips)
                    {
                        if (node.Attributes["size"] != null)
                        {
                            var strArray = node.Attributes["size"].Value.Split(new char[] { ',' });
                            strip2.ImageScalingSize = new System.Drawing.Size(int.Parse(strArray[0]), int.Parse(strArray[1]));
                            resizeChildren(strip2.Items, int.Parse(strArray[0]), int.Parse(strArray[1]));
                        }

                        if (strip2.Name == node.Attributes["name"].Value)
                        {
                            item = strip2;
                            break;
                        }

                    }
                    if (item != null)
                    {
                        toolStrips.Remove(item);

                        string[] strArray = node.Attributes["location"].Value.Split(new char[] { ',' });
                        toolStripPanel.Join(item, int.Parse(strArray[0]), int.Parse(strArray[1]));

                        item.Visible = bool.Parse(node.Attributes["visible"].Value);
                    }
                }
            }
        }

        public static void SaveSettings(Form form, XmlNode parentNode)
        {
            SaveSettings(form, parentNode, form.GetType().ToString());
        }

        public static void SaveSettings(Form form, XmlNode parentNode, string key)
        {
            XmlNode emptyNodeAlways = Xml.GetEmptyNodeAlways(Xml.GetNodeAlways(parentNode, "ToolStripConfiguration"), key);
            foreach (Control control in form.Controls)
            {
                if (control is ToolStripContainer)
                {
                    ToolStripContainer container = (ToolStripContainer) control;
                    WritePanel(emptyNodeAlways, container.TopToolStripPanel, "TopToolStripPanel");
                    WritePanel(emptyNodeAlways, container.LeftToolStripPanel, "LeftToolStripPanel");
                    WritePanel(emptyNodeAlways, container.RightToolStripPanel, "RightToolStripPanel");
                    WritePanel(emptyNodeAlways, container.BottomToolStripPanel, "BottomToolStripPanel");
                    break;
                }
            }
        }

        private static void WritePanel(XmlNode parentNode, ToolStripPanel toolStripPanel, string toolStripPanelName)
        {
            XmlNode node = Xml.SetNewValue(parentNode, "Panel", string.Empty);
            Xml.SetAttribute(node, "name", toolStripPanelName);
            List<ToolStrip> list = new List<ToolStrip>();
            List<Control> list2 = new List<Control>();
            foreach (ToolStripPanelRow row in toolStripPanel.Rows)
            {
                list2.Clear();
                list2.AddRange(row.Controls);
                list2.Sort(delegate (Control control1, Control control2) {
                    if (control1.Location.Y < control2.Location.Y)
                    {
                        return -1;
                    }
                    if (control2.Location.Y >= control1.Location.Y)
                    {
                        if (control1.Location.X == control2.Location.X)
                        {
                            return 0;
                        }
                        if (control1.Location.X < control2.Location.X)
                        {
                            return -1;
                        }
                    }
                    return 1;
                });
                foreach (ToolStrip strip in list2)
                {
                    list.Add(strip);
                }
            }
            foreach (Control control in list)
            {
                XmlNode node2 = Xml.SetNewValue(node, "Strip", string.Empty);
                Xml.SetAttribute(node2, "name", control.Name);
                Xml.SetAttribute(node2, "location", string.Format("{0},{1}", control.Location.X, control.Location.Y));
                Xml.SetAttribute(node2, "visible", control.Visible.ToString());
                if (control.GetType() == typeof(ToolStrip))
                {
                    Xml.SetAttribute(node2, "size", string.Format("{0},{1}", ((ToolStrip)control).ImageScalingSize.Height, ((ToolStrip)control).ImageScalingSize.Width));
                }
            }
        }

        public static void resizeToolStrips(Form form, int width, int height)
        {
            foreach (Control control in form.Controls)
            {
                System.Diagnostics.Debug.WriteLine("Control: " + control.Name);
                if (control is ToolStripContainer)
                {
                    foreach (ToolStrip toolStrip in ((ToolStripContainer)control).TopToolStripPanel.Controls)
                    {
                        System.Diagnostics.Debug.WriteLine("ToolStrip: " + toolStrip.Name);
                        toolStrip.ImageScalingSize = new Size(width, height);
                        resizeChildren(toolStrip.Items, width, height);
                    }
                }
                if (control is MenuStrip)
                {
                    foreach (ToolStripItemCollection item in ((MenuStrip)control).Items)
                    {
                        if (item.
                    }
                    var name = ((MenuStrip)control).Name;
                    //if (name == "smallToolStripMenuItem")
                    //{
                    //    ((MenuStrip)control).Checked = (width == ICON_SIZE_SMALL);
                    //}
                    //if (name == "mediumToolStripMenuItem")
                    //{
                    //}
                    //if (name == "largeToolStripMenuItem")
                    //{
                    //}
                    System.Diagnostics.Debug.WriteLine("Name: " + name);
                }
            }
        }


        private static void resizeChildren(ToolStripItemCollection toolStripItems, int width, int height)
        {
            foreach (var item in toolStripItems)
            {
                if (item is ToolStripButton)
                {
                    ((ToolStripButton)item).Size = new Size(width, height);
                }
            }
        }

        //private static void (
    }
}

