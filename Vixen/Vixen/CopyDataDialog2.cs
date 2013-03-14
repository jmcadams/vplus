namespace Vixen
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;
    using System.Xml;

    internal class CopyDataDialog2 : Form
    {
        private Button buttonCopy;
        private Button buttonDone;
        private Button buttonFromFile;
        private Button buttonFromProgram;
        private Button buttonFromSequence;
        private Button buttontoFile;
        private Button buttonToProgram;
        private Button buttonToSequence;
        private CheckBox checkBoxShowAllNodes;
        private IContainer components = null;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Label label1;
        private Label label2;
        private Label labelFrom;
        private Label labelTo;
        private XmlDocument m_destDoc;
        private string[] m_programPaths = new string[] { "Program/PlugInData" };
        private string[] m_sequencePaths = new string[] { "Program/Channels", "Program/EventValues", "Program/PlugInData", "Program/LoadableData" };
        private NodeSource m_source;
        private XmlDocument m_sourceDoc;
        private OpenFileDialog openFileDialog;
        private SaveFileDialog saveFileDialog;
        private TreeView treeViewFrom;

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

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
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

        private void InitializeComponent()
        {
            this.buttonCopy = new Button();
            this.treeViewFrom = new TreeView();
            this.buttonDone = new Button();
            this.groupBox2 = new GroupBox();
            this.buttontoFile = new Button();
            this.labelTo = new Label();
            this.buttonToProgram = new Button();
            this.buttonToSequence = new Button();
            this.groupBox1 = new GroupBox();
            this.buttonFromFile = new Button();
            this.labelFrom = new Label();
            this.buttonFromProgram = new Button();
            this.buttonFromSequence = new Button();
            this.label1 = new Label();
            this.checkBoxShowAllNodes = new CheckBox();
            this.openFileDialog = new OpenFileDialog();
            this.saveFileDialog = new SaveFileDialog();
            this.label2 = new Label();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            base.SuspendLayout();
            this.buttonCopy.Location = new Point(0x124, 0xb1);
            this.buttonCopy.Name = "buttonCopy";
            this.buttonCopy.Size = new Size(0x4b, 0x17);
            this.buttonCopy.TabIndex = 6;
            this.buttonCopy.Text = "Copy";
            this.buttonCopy.UseVisualStyleBackColor = true;
            this.buttonCopy.Click += new EventHandler(this.buttonCopy_Click);
            this.treeViewFrom.CheckBoxes = true;
            this.treeViewFrom.HideSelection = false;
            this.treeViewFrom.Location = new Point(0x124, 0x1f);
            this.treeViewFrom.Name = "treeViewFrom";
            this.treeViewFrom.Size = new Size(0xd8, 0x77);
            this.treeViewFrom.TabIndex = 7;
            this.buttonDone.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonDone.DialogResult = DialogResult.OK;
            this.buttonDone.Location = new Point(0x1af, 0x108);
            this.buttonDone.Name = "buttonDone";
            this.buttonDone.Size = new Size(0x4b, 0x17);
            this.buttonDone.TabIndex = 8;
            this.buttonDone.Text = "Done";
            this.buttonDone.UseVisualStyleBackColor = true;
            this.groupBox2.Controls.Add(this.buttontoFile);
            this.groupBox2.Controls.Add(this.labelTo);
            this.groupBox2.Controls.Add(this.buttonToProgram);
            this.groupBox2.Controls.Add(this.buttonToSequence);
            this.groupBox2.Location = new Point(12, 0x6c);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(0xfd, 0x55);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Copy To";
            this.buttontoFile.Location = new Point(0xa6, 0x13);
            this.buttontoFile.Name = "buttontoFile";
            this.buttontoFile.Size = new Size(70, 0x17);
            this.buttontoFile.TabIndex = 2;
            this.buttontoFile.Text = "File";
            this.buttontoFile.UseVisualStyleBackColor = true;
            this.buttontoFile.Click += new EventHandler(this.buttontoFile_Click);
            this.labelTo.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labelTo.Location = new Point(20, 50);
            this.labelTo.Name = "labelTo";
            this.labelTo.Size = new Size(0xd8, 0x20);
            this.labelTo.TabIndex = 3;
            this.buttonToProgram.Location = new Point(90, 0x13);
            this.buttonToProgram.Name = "buttonToProgram";
            this.buttonToProgram.Size = new Size(70, 0x17);
            this.buttonToProgram.TabIndex = 1;
            this.buttonToProgram.Text = "Program";
            this.buttonToProgram.UseVisualStyleBackColor = true;
            this.buttonToProgram.Click += new EventHandler(this.buttonToProgram_Click);
            this.buttonToSequence.Location = new Point(14, 0x13);
            this.buttonToSequence.Name = "buttonToSequence";
            this.buttonToSequence.Size = new Size(70, 0x17);
            this.buttonToSequence.TabIndex = 0;
            this.buttonToSequence.Text = "Sequence";
            this.buttonToSequence.UseVisualStyleBackColor = true;
            this.buttonToSequence.Click += new EventHandler(this.buttonToSequence_Click);
            this.groupBox1.Controls.Add(this.buttonFromFile);
            this.groupBox1.Controls.Add(this.labelFrom);
            this.groupBox1.Controls.Add(this.buttonFromProgram);
            this.groupBox1.Controls.Add(this.buttonFromSequence);
            this.groupBox1.Location = new Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new Size(0xfd, 0x55);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Copy From";
            this.buttonFromFile.Location = new Point(0xa6, 0x13);
            this.buttonFromFile.Name = "buttonFromFile";
            this.buttonFromFile.Size = new Size(70, 0x17);
            this.buttonFromFile.TabIndex = 2;
            this.buttonFromFile.Text = "File";
            this.buttonFromFile.UseVisualStyleBackColor = true;
            this.buttonFromFile.Click += new EventHandler(this.buttonFromFile_Click);
            this.labelFrom.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labelFrom.Location = new Point(0x13, 0x33);
            this.labelFrom.Name = "labelFrom";
            this.labelFrom.Size = new Size(0xd9, 0x1f);
            this.labelFrom.TabIndex = 3;
            this.buttonFromProgram.Location = new Point(90, 0x13);
            this.buttonFromProgram.Name = "buttonFromProgram";
            this.buttonFromProgram.Size = new Size(70, 0x17);
            this.buttonFromProgram.TabIndex = 1;
            this.buttonFromProgram.Text = "Program";
            this.buttonFromProgram.UseVisualStyleBackColor = true;
            this.buttonFromProgram.Click += new EventHandler(this.buttonFromProgram_Click);
            this.buttonFromSequence.Location = new Point(14, 0x13);
            this.buttonFromSequence.Name = "buttonFromSequence";
            this.buttonFromSequence.Size = new Size(70, 0x17);
            this.buttonFromSequence.TabIndex = 0;
            this.buttonFromSequence.Text = "Sequence";
            this.buttonFromSequence.UseVisualStyleBackColor = true;
            this.buttonFromSequence.Click += new EventHandler(this.buttonFromSequence_Click);
            this.label1.AutoSize = true;
            this.label1.Location = new Point(290, 12);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x47, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Data to copy:";
            this.checkBoxShowAllNodes.AutoSize = true;
            this.checkBoxShowAllNodes.Location = new Point(0x124, 0x9a);
            this.checkBoxShowAllNodes.Name = "checkBoxShowAllNodes";
            this.checkBoxShowAllNodes.Size = new Size(0x62, 0x11);
            this.checkBoxShowAllNodes.TabIndex = 10;
            this.checkBoxShowAllNodes.Text = "Show all nodes";
            this.checkBoxShowAllNodes.UseVisualStyleBackColor = true;
            this.checkBoxShowAllNodes.CheckedChanged += new EventHandler(this.checkBoxShowAllNodes_CheckedChanged);
            this.label2.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.label2.Location = new Point(13, 0xda);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x1ec, 0x25);
            this.label2.TabIndex = 11;
            this.label2.Text = "WARNING: This tool allows you to copy anything to your benefit or detriment.  Please be aware of this as you use it.";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(0x206, 0x12b);
            base.Controls.Add(this.label2);
            base.Controls.Add(this.checkBoxShowAllNodes);
            base.Controls.Add(this.label1);
            base.Controls.Add(this.buttonCopy);
            base.Controls.Add(this.treeViewFrom);
            base.Controls.Add(this.buttonDone);
            base.Controls.Add(this.groupBox2);
            base.Controls.Add(this.groupBox1);
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.Name = "CopyDataDialog2";
            base.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Copy Data";
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            base.ResumeLayout(false);
            base.PerformLayout();
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

