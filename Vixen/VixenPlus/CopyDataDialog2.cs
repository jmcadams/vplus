namespace Vixen
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;
    using System.Xml;

    internal partial class CopyDataDialog2 : Form
    {
        private XmlDocument m_destDoc;
        private string[] m_programPaths = new string[] { "Program/PlugInData" };
        private string[] m_sequencePaths = new string[] { "Program/Channels", "Program/EventValues", "Program/PlugInData", "Program/LoadableData" };
        private NodeSource m_source;
        private XmlDocument m_sourceDoc;
        
        
        

        public CopyDataDialog2()
        {
            this.InitializeComponent();
            this.treeViewFrom.PathSeparator = "/";
            this.m_sourceDoc = new XmlDocument();
            this.m_destDoc = new XmlDocument();
        }

        private void AddNode(XmlNode node, TreeNode parentNode)
        {
            if (node is XmlElement)
            {
                TreeNode node2 = this.AddNodeFormatted(node, parentNode);
                foreach (XmlNode node3 in node.ChildNodes)
                {
                    this.AddNode(node3, node2);
                }
            }
        }

        private TreeNode AddNodeFormatted(XmlNode node, TreeNode parentNode)
        {
            TreeNode node2;
            if (parentNode == null)
            {
                node2 = this.treeViewFrom.Nodes.Add(string.Empty);
                node2.Tag = node.Name;
            }
            else
            {
                node2 = parentNode.Nodes.Add(string.Empty);
                node2.Tag = string.Format("{0}/{1}", parentNode.Tag, node.Name);
            }
            if (node.Attributes["name"] != null)
            {
                node2.Text = node.Attributes["name"].Value;
                node2.Name = string.Format("[@name=\"{0}\"]", node2.Text);
                return node2;
            }
            if (node.Name == "Channel")
            {
                node2.Text = node.InnerText;
                node2.Name = string.Format("[contains(.,\"{0}\") and string-length(substring-after(.,\"{0}\")) = 0]", node2.Text);
                return node2;
            }
            node2.Text = node.Name;
            node2.Name = node.Name;
            return node2;
        }

        private void AddTreeNode(TreeNode node, List<TreeNode> nodes)
        {
            if (node.Checked)
            {
                nodes.Add(node);
            }
            foreach (TreeNode node2 in node.Nodes)
            {
                this.AddTreeNode(node2, nodes);
            }
        }

        private void buttonCopy_Click(object sender, EventArgs e)
        {
            if (this.m_sourceDoc.BaseURI == string.Empty)
            {
                MessageBox.Show("You need to select something to copy from.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else if (this.m_destDoc.BaseURI == string.Empty)
            {
                MessageBox.Show("You need to select something to copy to.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else if (this.m_sourceDoc.BaseURI == this.m_destDoc.BaseURI)
            {
                MessageBox.Show("Source and destination cannot be the same.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else if (MessageBox.Show("If the selected items already exist in the destination, they will be overwritten.\nClick 'Yes' to confirm that you approve of this.", Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.No)
            {
                int count = 0;
                int num2 = 0;
                int num3 = 0;
                XmlNode node = this.m_destDoc.SelectSingleNode("//Program/Time");
                if (node != null)
                {
                    count = this.m_destDoc.SelectNodes("//Program/Channels/*").Count;
                    try
                    {
                        num2 = Convert.ToInt32(node.InnerText);
                    }
                    catch
                    {
                        num2 = 0;
                    }
                    try
                    {
                        num3 = Convert.ToInt32(this.m_destDoc.SelectSingleNode("//Program/EventPeriodInMilliseconds").InnerText);
                    }
                    catch
                    {
                        num3 = 0;
                    }
                }
                List<string> list = new List<string>();
                foreach (TreeNode node5 in this.GetAllNodes())
                {
                    string str;
                    list.Add(node5.Text);
                    if (node5.Name != node5.Text)
                    {
                        str = node5.Tag.ToString() + node5.Name;
                    }
                    else
                    {
                        str = node5.Tag.ToString();
                    }
                    XmlNode node2 = this.m_sourceDoc.SelectSingleNode("//" + str);
                    if (node2 == null)
                    {
                        MessageBox.Show("Error encountered when trying to find the specified node.\nThis is a program error due to the data.\nPlease visit the support forum to get this resolved.\n\nNode: " + str, Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        return;
                    }
                    XmlNode oldChild = this.m_destDoc.SelectSingleNode(str);
                    if (oldChild != null)
                    {
                        oldChild.ParentNode.RemoveChild(oldChild);
                    }
                    if ((node2.ParentNode != null) && (node2.ParentNode is XmlElement))
                    {
                        Xml.CloneNode(this.m_destDoc, node2.ParentNode, false).AppendChild(this.m_destDoc.ImportNode(node2, true));
                    }
                }
                if ((list.Contains("EventPeriodInMilliseconds") || list.Contains("Time")) || list.Contains("EventValues"))
                {
                    XmlNode node6 = this.m_destDoc.SelectSingleNode("//Program/Time");
                    if (node6 != null)
                    {
                        int num6;
                        int num4 = this.m_sourceDoc.SelectNodes("//Program/Channels/Channel").Count;
                        int num5 = this.m_destDoc.SelectNodes("//Program/Channels/*").Count;
                        try
                        {
                            num6 = Convert.ToInt32(node6.InnerText);
                        }
                        catch
                        {
                            num6 = 0;
                        }
                        if (num6 != 0)
                        {
                            byte[] buffer = Convert.FromBase64String(this.m_destDoc.SelectSingleNode("//Program/EventValues").InnerText);
                            int num7 = int.Parse(this.m_sourceDoc.SelectSingleNode("//Program/EventPeriodInMilliseconds").InnerText);
                            int num8 = int.Parse(this.m_destDoc.SelectSingleNode("//Program/EventPeriodInMilliseconds").InnerText);
                            int num9 = (int) Math.Ceiling((double) ((num5 * num6) / ((float) num8)));
                            if (((num9 != 0) && (num9 < buffer.Length)) && (MessageBox.Show("This change requires that events are to be truncated in order to maintain data integrity.\nThis will cause data loss in the destination sequence.\n\nProceed?\n\nIf you select 'No', your change will still be in effect, but no data will be truncated.\nKnow that this will leave the destination sequence in an unusable state.", Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No))
                            {
                                return;
                            }
                            if ((num9 != 0) && (num9 != buffer.Length))
                            {
                                byte[] inArray = new byte[num9];
                                int num10 = inArray.Length / num5;
                                int num11 = buffer.Length / num4;
                                int num12 = Math.Min(num4, num5);
                                float num14 = Math.Max(num7, num8);
                                float num15 = num14 / ((float) num7);
                                float num16 = num14 / ((float) num8);
                                int num17 = (int) Math.Min((float) (((float) num11) / num15), (float) (((float) num10) / num16));
                                for (int i = 0; i < num12; i++)
                                {
                                    for (float j = 0f; j < num17; j++)
                                    {
                                        float num18 = (i * num11) + (j * num15);
                                        float num19 = (i * num10) + (j * num16);
                                        byte num22 = 0;
                                        for (float k = 0f; k < num15; k++)
                                        {
                                            num22 = Math.Max(num22, buffer[(int) (num18 + k)]);
                                        }
                                        for (float m = 0f; m < num16; m++)
                                        {
                                            inArray[(int) (num19 + m)] = num22;
                                        }
                                    }
                                }
                                this.m_destDoc.SelectSingleNode("//Program/EventValues").InnerText = Convert.ToBase64String(inArray);
                            }
                        }
                    }
                }
                int num24 = 0;
                foreach (XmlNode node7 in this.m_destDoc.SelectNodes("//Program/PlugInData/PlugIn"))
                {
                    node7.Attributes["id"].Value = num24.ToString();
                    num24++;
                }
                this.m_destDoc.Save(this.m_destDoc.BaseURI.Substring(8).Replace('/', '\\'));
                MessageBox.Show(this.labelTo.Text + " has been updated.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void buttonFromFile_Click(object sender, EventArgs e)
        {
            this.openFileDialog.FileName = string.Empty;
            this.openFileDialog.Filter = string.Format("{0} data|*.{1}", Vendor.ProductName, Vendor.DataExtension);
            this.openFileDialog.DefaultExt = Vendor.DataExtension;
            this.openFileDialog.InitialDirectory = Paths.ImportExportPath;
            if (this.openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    this.m_sourceDoc.Load(this.openFileDialog.FileName);
                    XmlNode node = this.m_sourceDoc.SelectSingleNode("//Program");
                    this.labelFrom.Text = "file: " + Path.GetFileName(this.openFileDialog.FileName);
                    this.m_source = NodeSource.File;
                    if (this.checkBoxShowAllNodes.Checked)
                    {
                        this.checkBoxShowAllNodes_CheckedChanged(null, null);
                    }
                    this.checkBoxShowAllNodes.Checked = true;
                    this.checkBoxShowAllNodes.Enabled = false;
                }
                catch
                {
                    MessageBox.Show("This does not appear to be a valid file.\nPlease choose another.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        private void buttonFromProgram_Click(object sender, EventArgs e)
        {
            this.openFileDialog.FileName = string.Empty;
            this.openFileDialog.Filter = string.Format("{0} program|*.{1}", Vendor.ProductName, Vendor.ProgramExtension);
            this.openFileDialog.DefaultExt = Vendor.ProgramExtension;
            this.openFileDialog.InitialDirectory = Paths.ProgramPath;
            if (this.openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    this.m_sourceDoc.Load(this.openFileDialog.FileName);
                    XmlNode node = this.m_sourceDoc.SelectSingleNode("//Program");
                    this.labelFrom.Text = Path.GetFileNameWithoutExtension(this.openFileDialog.FileName);
                    this.m_source = NodeSource.Program;
                    if (!this.checkBoxShowAllNodes.Checked)
                    {
                        this.checkBoxShowAllNodes_CheckedChanged(null, null);
                    }
                    this.checkBoxShowAllNodes.Checked = false;
                    this.checkBoxShowAllNodes.Enabled = true;
                }
                catch
                {
                    MessageBox.Show("This does not appear to be a valid file.\nPlease choose another.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        private void buttonFromSequence_Click(object sender, EventArgs e)
        {
            this.openFileDialog.FileName = string.Empty;
            this.openFileDialog.Filter = string.Format("{0} sequence|*.{1}", Vendor.ProductName, Vendor.SequenceExtension);
            this.openFileDialog.DefaultExt = Vendor.SequenceExtension;
            this.openFileDialog.InitialDirectory = Paths.SequencePath;
            if (this.openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    this.m_sourceDoc.Load(this.openFileDialog.FileName);
                    XmlNode node = this.m_sourceDoc.SelectSingleNode("//Program");
                    this.labelFrom.Text = Path.GetFileNameWithoutExtension(this.openFileDialog.FileName);
                    this.m_source = NodeSource.Sequence;
                    if (!this.checkBoxShowAllNodes.Checked)
                    {
                        this.checkBoxShowAllNodes_CheckedChanged(null, null);
                    }
                    this.checkBoxShowAllNodes.Checked = false;
                    this.checkBoxShowAllNodes.Enabled = true;
                }
                catch
                {
                    MessageBox.Show("This does not appear to be a valid file.\nPlease choose another.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        private void buttontoFile_Click(object sender, EventArgs e)
        {
            this.saveFileDialog.FileName = string.Empty;
            this.saveFileDialog.Filter = string.Format("{0} data|*.{1}", Vendor.ProductName, Vendor.DataExtension);
            this.saveFileDialog.DefaultExt = Vendor.DataExtension;
            this.saveFileDialog.InitialDirectory = Paths.ImportExportPath;
            if (this.saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(this.saveFileDialog.FileName))
                {
                    this.m_destDoc.Load(this.saveFileDialog.FileName);
                }
                else
                {
                    this.m_destDoc = Xml.CreateXmlDocument("Program");
                    this.m_destDoc.Save(this.saveFileDialog.FileName);
                    this.m_destDoc.Load(this.saveFileDialog.FileName);
                }
                this.labelTo.Text = "file: " + Path.GetFileName(this.saveFileDialog.FileName);
            }
        }

        private void buttonToProgram_Click(object sender, EventArgs e)
        {
            this.openFileDialog.FileName = string.Empty;
            this.openFileDialog.Filter = string.Format("{0} program|*.{1}", Vendor.ProductName, Vendor.ProgramExtension);
            this.openFileDialog.DefaultExt = Vendor.ProgramExtension;
            this.openFileDialog.InitialDirectory = Paths.ProgramPath;
            if (this.openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    this.m_destDoc.Load(this.openFileDialog.FileName);
                    XmlNode node = this.m_destDoc.SelectSingleNode("//Program");
                    this.labelTo.Text = Path.GetFileNameWithoutExtension(this.openFileDialog.FileName);
                }
                catch
                {
                    MessageBox.Show("This does not appear to be a valid file.\nPlease choose another.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        private void buttonToSequence_Click(object sender, EventArgs e)
        {
            this.openFileDialog.FileName = string.Empty;
            this.openFileDialog.Filter = string.Format("{0} sequence|*.{1}", Vendor.ProductName, Vendor.SequenceExtension);
            this.openFileDialog.DefaultExt = Vendor.SequenceExtension;
            this.openFileDialog.InitialDirectory = Paths.SequencePath;
            if (this.openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    this.m_destDoc.Load(this.openFileDialog.FileName);
                    XmlNode node = this.m_destDoc.SelectSingleNode("//Program");
                    this.labelTo.Text = Path.GetFileNameWithoutExtension(this.openFileDialog.FileName);
                }
                catch
                {
                    MessageBox.Show("This does not appear to be a valid file.\nPlease choose another.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        private void checkBoxShowAllNodes_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.checkBoxShowAllNodes.Checked)
            {
                this.ShowPrescribedNodes();
            }
            else
            {
                this.ShowAllNodes();
            }
            if (this.treeViewFrom.Nodes.Count == 1)
            {
                this.treeViewFrom.Nodes[0].Expand();
            }
        }

        

        private TreeNode[] GetAllNodes()
        {
            List<TreeNode> nodes = new List<TreeNode>();
            foreach (TreeNode node in this.treeViewFrom.Nodes)
            {
                this.AddTreeNode(node, nodes);
            }
            return nodes.ToArray();
        }

        

        private void ShowAllNodes()
        {
            this.treeViewFrom.Nodes.Clear();
            this.treeViewFrom.BeginUpdate();
            this.AddNode(this.m_sourceDoc.SelectSingleNode("//Program"), null);
            this.treeViewFrom.EndUpdate();
        }

        private void ShowPrescribedNodes()
        {
            Array programPaths;
            this.treeViewFrom.Nodes.Clear();
            this.treeViewFrom.BeginUpdate();
            if (this.m_source == NodeSource.Program)
            {
                programPaths = this.m_programPaths;
            }
            else if (this.m_source == NodeSource.Sequence)
            {
                programPaths = this.m_sequencePaths;
            }
            else
            {
                return;
            }
            foreach (string str2 in programPaths)
            {
                if (this.m_sourceDoc.SelectSingleNode("//" + str2) != null)
                {
                    string text = str2.Substring(str2.LastIndexOf('/') + 1);
                    TreeNode parentNode = this.treeViewFrom.Nodes.Add(text);
                    parentNode.Tag = str2;
                    parentNode.Name = text;
                    foreach (XmlNode node2 in this.m_sourceDoc.SelectNodes("//" + str2 + "/*"))
                    {
                        this.AddNodeFormatted(node2, parentNode);
                    }
                }
            }
            this.treeViewFrom.EndUpdate();
        }

        private enum NodeSource
        {
            Sequence,
            Program,
            File
        }
    }
}

