namespace StandardScript
{
    using ScriptEngine;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;
    using System.Xml;
    using Vixen;
    using Vixen.Dialogs;

    public class StandardScript : UIBase
    {
        private ToolStripMenuItem addFileToolStripMenuItem;
        private ToolStripMenuItem attachedPluginsToolStripMenuItem;
        private ColumnHeader columnFileName;
        private ColumnHeader columnLine;
        private ColumnHeader columnMessage;
        private ToolStripMenuItem compileToolStripMenuItem;
        private IContainer components;
        private ContextMenuStrip contextMenuStripProject;
        private ToolStripMenuItem debugOutputToolStripMenuItem;
        private ToolStripMenuItem executeToolStripMenuItem;
        private ImageList imageList;
        private ToolStripMenuItem importChannelsToolStripMenuItem;
        private ToolStripMenuItem importsToolStripMenuItem;
        private ListBox listBoxChannels;
        private ListView listViewMessages;
        private ICompile m_compiler;
        private XmlDocument m_engineCommDoc = null;
        private IExecution m_executionContext;
        private int m_executionContextHandle;
        private TabPageDoc.OnPositionChange m_positionChange;
        private Preference2 m_preferences;
        private TreeNode m_projectNode;
        private EventSequence m_sequence;
        private MenuStrip menuStrip;
        private ToolStripMenuItem modulesToolStripMenuItem;
        private OpenFileDialog openFileDialog;
        private ToolStripMenuItem referencesToolStripMenuItem;
        private SaveFileDialog saveFileDialog;
        private ToolStripMenuItem scriptToolStripMenuItem;
        private SplitContainer splitContainer1;
        private Splitter splitter1;
        private StatusStrip statusStrip1;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabControl tabControlLeft;
        private TabPage tabPageChannels;
        private TabPage tabPageProject;
        private ToolStrip toolStrip1;
        private ToolStripButton toolStripButtonExecute;
        private ToolStripButton toolStripButtonStop;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripSeparator toolStripMenuItem2;
        private ToolStripStatusLabel toolStripStatusLabelPosition;
        private TreeView treeViewProjectFiles;

        public StandardScript()
        {
            this.InitializeComponent();
            object obj2 = null;
            if (Interfaces.Available.TryGetValue("ISystem", out obj2))
            {
                this.m_preferences = ((ISystem) obj2).UserPreferences;
            }
            if (Interfaces.Available.TryGetValue("IExecution", out obj2))
            {
                this.m_executionContext = (IExecution) obj2;
            }
            this.m_projectNode = this.treeViewProjectFiles.Nodes[0];
            this.m_compiler = Compiler.CreateCompiler();
            this.tabControlLeft.SelectedTab = this.tabPageProject;
            this.m_positionChange = new TabPageDoc.OnPositionChange(this.CursorPositionChange);
            TabPageDoc.DirtyChanged += new EventHandler(this.TabPageDoc_DirtyChanged);
        }

        private void AddFile()
        {
            this.AddFile(string.Empty);
        }

        private void AddFile(string filePath)
        {
            TabPageDoc doc = new TabPageDoc(filePath);
            this.tabControl.TabPages.Add(doc);
            this.treeViewProjectFiles.BeginUpdate();
            TreeNode node = new TreeNode(doc.DisplayName, 1, 1);
            this.m_projectNode.Nodes.Add(node);
            if (!this.m_projectNode.IsExpanded)
            {
                this.m_projectNode.Expand();
            }
            this.treeViewProjectFiles.SelectedNode = node;
            this.treeViewProjectFiles.EndUpdate();
            doc.Tag = node;
            doc.PositionChange += this.m_positionChange;
            base.ActiveControl = doc.TextBox;
        }

        private void addFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.AddFile();
            base.IsDirty = true;
        }

        private void attachedPluginsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PluginListDialog dialog = new PluginListDialog(this.m_sequence);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                base.IsDirty = true;
            }
            dialog.Dispose();
        }

        private bool Compile()
        {
            string path = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            bool flag = false;
            this.listViewMessages.Items.Clear();
            if (this.m_sequence.Extensions[this.FileExtension].SelectNodes("ModuleReferences/*").Count == 0)
            {
                this.listViewMessages.Items.Add(new ListViewItem(new string[] { string.Empty, string.Empty, "A script module needs to be selected" }));
                return false;
            }
            this.Cursor = Cursors.WaitCursor;
            try
            {
                EventSequence sequence;
                Directory.CreateDirectory(path);
                if (this.debugOutputToolStripMenuItem.Checked)
                {
                    this.m_compiler.SetFlag(0x370010fe);
                }
                if (((sequence = this.CreateTempSequence(path)) != null) && !(flag = this.m_compiler.Compile(sequence)))
                {
                    this.listViewMessages.BeginUpdate();
                    foreach (CompileError error in this.m_compiler.Errors)
                    {
                        if (error.Line > 0)
                        {
                            this.listViewMessages.Items.Add(new ListViewItem(new string[] { error.FileName, error.Line.ToString(), error.ErrorMessage }));
                        }
                        else
                        {
                            this.listViewMessages.Items.Add(new ListViewItem(new string[] { error.FileName, string.Empty, error.ErrorMessage }));
                        }
                    }
                    this.listViewMessages.EndUpdate();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message + "\n\n" + exception.StackTrace);
            }
            finally
            {
                Directory.Delete(path, true);
                this.Cursor = Cursors.Default;
            }
            return flag;
        }

        private void compileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Compile();
        }

        private void contextMenuStripProject_Opening(object sender, CancelEventArgs e)
        {
        }

        private EventSequence CreateTempSequence(string directory)
        {
            try
            {
                string str;
                XmlNode contextNode = this.m_sequence.Extensions[this.FileExtension];
                XmlNode emptyNodeAlways = Xml.GetEmptyNodeAlways(contextNode, "SourceFiles");
                foreach (TabPageDoc doc in this.tabControl.TabPages)
                {
                    File.WriteAllText(Path.Combine(directory, doc.DisplayName), doc.TextBox.Text);
                    Xml.SetNewValue(emptyNodeAlways, "SourceFile", doc.DisplayName);
                }
                Xml.SetValue(contextNode, "SourceDirectory", directory);
                if (this.m_sequence.Name == string.Empty)
                {
                    str = Path.Combine(directory, "temp");
                }
                else
                {
                    str = Path.Combine(directory, Path.GetFileName(this.m_sequence.FileName));
                }
                return this.m_sequence;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message + "\n\n" + exception.StackTrace);
                return null;
            }
        }

        private void CursorPositionChange(Point pt)
        {
            this.toolStripStatusLabelPosition.Text = string.Format("Line {0}   Col {1}", pt.Y + 1, pt.X + 1);
        }

        private bool DocsDirty()
        {
            foreach (TabPageDoc doc in this.tabControl.TabPages)
            {
                if (doc.IsDirty)
                {
                    return true;
                }
            }
            return false;
        }

        private bool DuplicateChannelNames(IEnumerable<Channel> channels)
        {
            List<string> list = new List<string>();
            foreach (Channel channel in channels)
            {
                if (list.Contains(channel.Name))
                {
                    return true;
                }
                list.Add(channel.Name);
            }
            return false;
        }

        private void Execute()
        {
            string path = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
            Directory.CreateDirectory(path);
            try
            {
                this.Cursor = Cursors.WaitCursor;
                EventSequence executableObject = this.CreateTempSequence(path);
                if (executableObject != null)
                {
                    this.m_executionContext.SetSynchronousContext(this.m_executionContextHandle, executableObject);
                    this.listViewMessages.Items.Clear();
                    if (!this.m_executionContext.ExecutePlay(this.m_executionContextHandle))
                    {
                        this.listViewMessages.BeginUpdate();
                        foreach (XmlNode node in this.m_engineCommDoc.SelectNodes("//Engine/Result/Errors/Error"))
                        {
                            if (int.Parse(node.Attributes["line"].Value) > 0)
                            {
                                this.listViewMessages.Items.Add(new ListViewItem(new string[] { node.Attributes["filename"].Value, node.Attributes["line"].Value, node.InnerText }));
                            }
                            else
                            {
                                this.listViewMessages.Items.Add(new ListViewItem(new string[] { node.Attributes["filename"].Value, string.Empty, node.InnerText }));
                            }
                        }
                        this.listViewMessages.EndUpdate();
                    }
                    else
                    {
                        this.SetUIState(true);
                    }
                }
            }
            finally
            {
                Directory.Delete(path, true);
                this.Cursor = Cursors.Default;
            }
        }

        private void executeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Execute();
        }

        private bool ImportChannels(IEnumerable<Channel> sourceChannelCollection)
        {
            if (this.DuplicateChannelNames(sourceChannelCollection) && (MessageBox.Show("There are channels with duplicate names.\nDo you want this correctly automatically?", Vendor.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes))
            {
                MessageBox.Show("Channel names will not be imported.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return false;
            }
            List<string> list = new List<string>();
            this.m_sequence.Channels.Clear();
            foreach (Channel channel in sourceChannelCollection)
            {
                string str;
                string item = str = this.Rationalize(channel.Name);
                int num = 2;
                while (list.Contains(item))
                {
                    item = string.Format("{0}_{1}", str, num++);
                }
                list.Add(item);
                this.m_sequence.Channels.Add(new Channel(item, channel.OutputChannel));
            }
            return true;
        }

        private void importChannelsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.openFileDialog.Title = "Import channels from...";
            this.openFileDialog.FileName = string.Empty;
            this.openFileDialog.InitialDirectory = Paths.SequencePath;
            if (this.openFileDialog.ShowDialog() == DialogResult.OK)
            {
                EventSequence sequence = new EventSequence(this.openFileDialog.FileName);
                if (this.ImportChannels(sequence.Channels))
                {
                    this.UpdateChannelList();
                }
                MessageBox.Show(this.m_sequence.Channels.Count.ToString() + " channels have been imported.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void importsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XmlNode contextNode = this.m_sequence.Extensions[this.FileExtension];
            TextListDialog dialog = new TextListDialog(Xml.GetNodeAlways(contextNode, "Imports"), "Import", true);
            dialog.Text = "Namespace Imports";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                base.IsDirty = true;
            }
            dialog.Dispose();
        }

        private void Init()
        {
            this.m_sequence.EngineType = EngineType.Procedural;
            this.m_executionContextHandle = this.m_executionContext.RequestContext(true, false, null, ref this.m_engineCommDoc);
            this.m_executionContext.SetSynchronousProgramChangeHandler(this.m_executionContextHandle, new ProgramChangeHandler(this.ProgramChange));
            if (this.ImportChannels(this.m_sequence.Channels.ToArray()))
            {
                this.UpdateChannelList();
            }
            base.IsDirty = false;
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(StandardScript));
            TreeNode node = new TreeNode("Node0");
            this.menuStrip = new MenuStrip();
            this.scriptToolStripMenuItem = new ToolStripMenuItem();
            this.compileToolStripMenuItem = new ToolStripMenuItem();
            this.executeToolStripMenuItem = new ToolStripMenuItem();
            this.debugOutputToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripMenuItem1 = new ToolStripSeparator();
            this.referencesToolStripMenuItem = new ToolStripMenuItem();
            this.modulesToolStripMenuItem = new ToolStripMenuItem();
            this.importsToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripMenuItem2 = new ToolStripSeparator();
            this.importChannelsToolStripMenuItem = new ToolStripMenuItem();
            this.attachedPluginsToolStripMenuItem = new ToolStripMenuItem();
            this.toolStrip1 = new ToolStrip();
            this.toolStripButtonExecute = new ToolStripButton();
            this.toolStripButtonStop = new ToolStripButton();
            this.imageList = new ImageList(this.components);
            this.listViewMessages = new ListView();
            this.columnFileName = new ColumnHeader();
            this.columnLine = new ColumnHeader();
            this.columnMessage = new ColumnHeader();
            this.splitter1 = new Splitter();
            this.openFileDialog = new OpenFileDialog();
            this.splitContainer1 = new SplitContainer();
            this.tabControlLeft = new System.Windows.Forms.TabControl();
            this.tabPageProject = new TabPage();
            this.treeViewProjectFiles = new TreeView();
            this.contextMenuStripProject = new ContextMenuStrip(this.components);
            this.addFileToolStripMenuItem = new ToolStripMenuItem();
            this.tabPageChannels = new TabPage();
            this.listBoxChannels = new ListBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.statusStrip1 = new StatusStrip();
            this.toolStripStatusLabelPosition = new ToolStripStatusLabel();
            this.saveFileDialog = new SaveFileDialog();
            this.menuStrip.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControlLeft.SuspendLayout();
            this.tabPageProject.SuspendLayout();
            this.contextMenuStripProject.SuspendLayout();
            this.tabPageChannels.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            base.SuspendLayout();
            this.menuStrip.Items.AddRange(new ToolStripItem[] { this.scriptToolStripMenuItem });
            this.menuStrip.Location = new Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new Size(740, 0x18);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            this.menuStrip.Visible = false;
            this.scriptToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.compileToolStripMenuItem, this.executeToolStripMenuItem, this.debugOutputToolStripMenuItem, this.toolStripMenuItem1, this.referencesToolStripMenuItem, this.modulesToolStripMenuItem, this.importsToolStripMenuItem, this.toolStripMenuItem2, this.importChannelsToolStripMenuItem, this.attachedPluginsToolStripMenuItem });
            this.scriptToolStripMenuItem.MergeAction = MergeAction.Insert;
            this.scriptToolStripMenuItem.MergeIndex = 5;
            this.scriptToolStripMenuItem.Name = "scriptToolStripMenuItem";
            this.scriptToolStripMenuItem.Size = new Size(0x31, 20);
            this.scriptToolStripMenuItem.Text = "Script";
            this.scriptToolStripMenuItem.DropDownOpening += new EventHandler(this.scriptToolStripMenuItem_DropDownOpening);
            this.compileToolStripMenuItem.Name = "compileToolStripMenuItem";
            this.compileToolStripMenuItem.ShortcutKeys = Keys.F6;
            this.compileToolStripMenuItem.Size = new Size(0xcc, 0x16);
            this.compileToolStripMenuItem.Text = "Compile";
            this.compileToolStripMenuItem.Click += new EventHandler(this.compileToolStripMenuItem_Click);
            this.executeToolStripMenuItem.Name = "executeToolStripMenuItem";
            this.executeToolStripMenuItem.ShortcutKeys = Keys.F5;
            this.executeToolStripMenuItem.Size = new Size(0xcc, 0x16);
            this.executeToolStripMenuItem.Text = "Compile and execute";
            this.executeToolStripMenuItem.Click += new EventHandler(this.executeToolStripMenuItem_Click);
            this.debugOutputToolStripMenuItem.CheckOnClick = true;
            this.debugOutputToolStripMenuItem.Name = "debugOutputToolStripMenuItem";
            this.debugOutputToolStripMenuItem.Size = new Size(0xcc, 0x16);
            this.debugOutputToolStripMenuItem.Text = "Debug output";
            this.debugOutputToolStripMenuItem.Visible = false;
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new Size(0xc9, 6);
            this.referencesToolStripMenuItem.Name = "referencesToolStripMenuItem";
            this.referencesToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.R;
            this.referencesToolStripMenuItem.Size = new Size(0xcc, 0x16);
            this.referencesToolStripMenuItem.Text = "References";
            this.referencesToolStripMenuItem.Click += new EventHandler(this.referencesToolStripMenuItem_Click);
            this.modulesToolStripMenuItem.Name = "modulesToolStripMenuItem";
            this.modulesToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.M;
            this.modulesToolStripMenuItem.Size = new Size(0xcc, 0x16);
            this.modulesToolStripMenuItem.Text = "Modules";
            this.modulesToolStripMenuItem.Click += new EventHandler(this.modulesToolStripMenuItem_Click);
            this.importsToolStripMenuItem.Name = "importsToolStripMenuItem";
            this.importsToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.I;
            this.importsToolStripMenuItem.Size = new Size(0xcc, 0x16);
            this.importsToolStripMenuItem.Text = "Imports";
            this.importsToolStripMenuItem.Click += new EventHandler(this.importsToolStripMenuItem_Click);
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new Size(0xc9, 6);
            this.importChannelsToolStripMenuItem.Name = "importChannelsToolStripMenuItem";
            this.importChannelsToolStripMenuItem.Size = new Size(0xcc, 0x16);
            this.importChannelsToolStripMenuItem.Text = "Import channels";
            this.importChannelsToolStripMenuItem.Click += new EventHandler(this.importChannelsToolStripMenuItem_Click);
            this.attachedPluginsToolStripMenuItem.Name = "attachedPluginsToolStripMenuItem";
            this.attachedPluginsToolStripMenuItem.Size = new Size(0xcc, 0x16);
            this.attachedPluginsToolStripMenuItem.Text = "Attached plugins";
            this.attachedPluginsToolStripMenuItem.Click += new EventHandler(this.attachedPluginsToolStripMenuItem_Click);
            this.toolStrip1.Items.AddRange(new ToolStripItem[] { this.toolStripButtonExecute, this.toolStripButtonStop });
            this.toolStrip1.Location = new Point(0, 0x18);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new Size(740, 0x19);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStripButtonExecute.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButtonExecute.Image = (Image) manager.GetObject("toolStripButtonExecute.Image");
            this.toolStripButtonExecute.ImageTransparentColor = Color.Magenta;
            this.toolStripButtonExecute.Name = "toolStripButtonExecute";
            this.toolStripButtonExecute.Size = new Size(0x17, 0x16);
            this.toolStripButtonExecute.Text = "Execute";
            this.toolStripButtonExecute.Click += new EventHandler(this.toolStripButtonExecute_Click);
            this.toolStripButtonStop.DisplayStyle = ToolStripItemDisplayStyle.Image;
            this.toolStripButtonStop.Enabled = false;
            this.toolStripButtonStop.Image = (Image) manager.GetObject("toolStripButtonStop.Image");
            this.toolStripButtonStop.ImageTransparentColor = Color.Magenta;
            this.toolStripButtonStop.Name = "toolStripButtonStop";
            this.toolStripButtonStop.Size = new Size(0x17, 0x16);
            this.toolStripButtonStop.Text = "Stop";
            this.toolStripButtonStop.Click += new EventHandler(this.toolStripButtonStop_Click);
            this.imageList.ImageStream = (ImageListStreamer) manager.GetObject("imageList.ImageStream");
            this.imageList.TransparentColor = Color.Transparent;
            this.imageList.Images.SetKeyName(0, "Folder.bmp");
            this.imageList.Images.SetKeyName(1, "Doc.bmp");
            this.listViewMessages.Columns.AddRange(new ColumnHeader[] { this.columnFileName, this.columnLine, this.columnMessage });
            this.listViewMessages.Dock = DockStyle.Bottom;
            this.listViewMessages.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            this.listViewMessages.Location = new Point(0, 0x1a9);
            this.listViewMessages.Name = "listViewMessages";
            this.listViewMessages.Size = new Size(740, 0x62);
            this.listViewMessages.TabIndex = 3;
            this.listViewMessages.UseCompatibleStateImageBehavior = false;
            this.listViewMessages.View = View.Details;
            this.columnFileName.Text = "File";
            this.columnFileName.Width = 120;
            this.columnLine.Text = "Line";
            this.columnLine.Width = 40;
            this.columnMessage.Text = "Message";
            this.columnMessage.Width = 0x22b;
            this.splitter1.Dock = DockStyle.Bottom;
            this.splitter1.Location = new Point(0, 0x1a5);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new Size(740, 4);
            this.splitter1.TabIndex = 4;
            this.splitter1.TabStop = false;
            this.openFileDialog.FileName = "openFileDialog1";
            this.splitContainer1.Dock = DockStyle.Fill;
            this.splitContainer1.Location = new Point(0, 0x31);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Panel1.Controls.Add(this.tabControlLeft);
            this.splitContainer1.Panel1MinSize = 150;
            this.splitContainer1.Panel2.Controls.Add(this.tabControl);
            this.splitContainer1.Panel2.Controls.Add(this.statusStrip1);
            this.splitContainer1.Size = new Size(740, 0x174);
            this.splitContainer1.SplitterDistance = 0xc3;
            this.splitContainer1.TabIndex = 5;
            this.tabControlLeft.Controls.Add(this.tabPageProject);
            this.tabControlLeft.Controls.Add(this.tabPageChannels);
            this.tabControlLeft.Dock = DockStyle.Fill;
            this.tabControlLeft.Location = new Point(0, 0);
            this.tabControlLeft.Multiline = true;
            this.tabControlLeft.Name = "tabControlLeft";
            this.tabControlLeft.SelectedIndex = 0;
            this.tabControlLeft.Size = new Size(0xc3, 0x174);
            this.tabControlLeft.TabIndex = 0;
            this.tabPageProject.Controls.Add(this.treeViewProjectFiles);
            this.tabPageProject.Location = new Point(4, 0x16);
            this.tabPageProject.Name = "tabPageProject";
            this.tabPageProject.Padding = new Padding(3);
            this.tabPageProject.Size = new Size(0xbb, 0x15a);
            this.tabPageProject.TabIndex = 0;
            this.tabPageProject.Text = "Project";
            this.tabPageProject.UseVisualStyleBackColor = true;
            this.treeViewProjectFiles.BorderStyle = BorderStyle.None;
            this.treeViewProjectFiles.ContextMenuStrip = this.contextMenuStripProject;
            this.treeViewProjectFiles.Dock = DockStyle.Fill;
            this.treeViewProjectFiles.ImageIndex = 0;
            this.treeViewProjectFiles.ImageList = this.imageList;
            this.treeViewProjectFiles.Location = new Point(3, 3);
            this.treeViewProjectFiles.Name = "treeViewProjectFiles";
            node.Name = "ProjectNode";
            node.Text = "Node0";
            this.treeViewProjectFiles.Nodes.AddRange(new TreeNode[] { node });
            this.treeViewProjectFiles.SelectedImageIndex = 0;
            this.treeViewProjectFiles.Size = new Size(0xb5, 340);
            this.treeViewProjectFiles.TabIndex = 2;
            this.contextMenuStripProject.Items.AddRange(new ToolStripItem[] { this.addFileToolStripMenuItem });
            this.contextMenuStripProject.Name = "contextMenuStripProject";
            this.contextMenuStripProject.Size = new Size(0x74, 0x1a);
            this.contextMenuStripProject.Opening += new CancelEventHandler(this.contextMenuStripProject_Opening);
            this.addFileToolStripMenuItem.Name = "addFileToolStripMenuItem";
            this.addFileToolStripMenuItem.Size = new Size(0x73, 0x16);
            this.addFileToolStripMenuItem.Text = "Add file";
            this.addFileToolStripMenuItem.Click += new EventHandler(this.addFileToolStripMenuItem_Click);
            this.tabPageChannels.Controls.Add(this.listBoxChannels);
            this.tabPageChannels.Location = new Point(4, 0x16);
            this.tabPageChannels.Name = "tabPageChannels";
            this.tabPageChannels.Padding = new Padding(3);
            this.tabPageChannels.Size = new Size(0xbb, 0x15a);
            this.tabPageChannels.TabIndex = 1;
            this.tabPageChannels.Text = "Channels";
            this.tabPageChannels.UseVisualStyleBackColor = true;
            this.listBoxChannels.BorderStyle = BorderStyle.None;
            this.listBoxChannels.Dock = DockStyle.Fill;
            this.listBoxChannels.FormattingEnabled = true;
            this.listBoxChannels.Location = new Point(3, 3);
            this.listBoxChannels.Name = "listBoxChannels";
            this.listBoxChannels.SelectionMode = SelectionMode.None;
            this.listBoxChannels.Size = new Size(0xb5, 0x152);
            this.listBoxChannels.TabIndex = 0;
            this.listBoxChannels.MouseDoubleClick += new MouseEventHandler(this.listBoxChannels_MouseDoubleClick);
            this.tabControl.Dock = DockStyle.Fill;
            this.tabControl.Location = new Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new Size(0x21d, 350);
            this.tabControl.TabIndex = 2;
            this.statusStrip1.Items.AddRange(new ToolStripItem[] { this.toolStripStatusLabelPosition });
            this.statusStrip1.Location = new Point(0, 350);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new Size(0x21d, 0x16);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            this.toolStripStatusLabelPosition.Name = "toolStripStatusLabelPosition";
            this.toolStripStatusLabelPosition.Size = new Size(0, 0x11);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.ClientSize = new Size(740, 0x20b);
            base.Controls.Add(this.splitContainer1);
            base.Controls.Add(this.splitter1);
            base.Controls.Add(this.listViewMessages);
            base.Controls.Add(this.toolStrip1);
            base.Controls.Add(this.menuStrip);
            base.MainMenuStrip = this.menuStrip;
            base.Name = "StandardScript";
            base.FormClosing += new FormClosingEventHandler(this.StandardScript_FormClosing);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.tabControlLeft.ResumeLayout(false);
            this.tabPageProject.ResumeLayout(false);
            this.contextMenuStripProject.ResumeLayout(false);
            this.tabPageChannels.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void listBoxChannels_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.m_sequence.Profile == null)
            {
                ChannelNameEditDialog dialog = new ChannelNameEditDialog(this.m_sequence, this.listBoxChannels.TopIndex + (e.Y / this.listBoxChannels.ItemHeight));
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string[] channelNames = dialog.ChannelNames;
                    this.m_sequence.ChannelCount = channelNames.Length;
                    for (int i = 0; i < channelNames.Length; i++)
                    {
                        this.m_sequence.Channels[i].Name = this.Rationalize(channelNames[i]);
                    }
                    this.UpdateChannelList();
                }
                dialog.Dispose();
            }
            else
            {
                MessageBox.Show("Can't edit channel names when the sequence is attached to a profile.", Vendor.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void modulesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ModuleListDialog dialog = new ModuleListDialog(this.m_sequence.Extensions[this.FileExtension]);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                base.IsDirty = true;
            }
            dialog.Dispose();
        }

        public override EventSequence New()
        {
            this.m_sequence = new EventSequence(this.m_preferences);
            this.Init();
            this.m_sequence.Profile = null;
            this.m_sequence.ChannelCount = 0;
            this.m_sequence.Time = 0;
            this.m_projectNode.Text = "Untitled project";
            this.AddFile();
            ((TabPageDoc) this.tabControl.TabPages[0]).TextBox.Text = "void Start() {\r\n\t\r\n}\r\n";
            ((TabPageDoc) this.tabControl.TabPages[0]).TextBox.Select(0x10, 0);
            this.treeViewProjectFiles.Nodes[0].Text = "New project";
            this.Text = "Untitled project";
            base.IsDirty = true;
            return this.m_sequence;
        }

        public override EventSequence New(EventSequence seedSequence)
        {
            return null;
        }

        public override void Notify(Notification notification, object data)
        {
        }

        public override EventSequence Open(string filePath)
        {
            this.m_sequence = new EventSequence(filePath);
            this.Init();
            this.treeViewProjectFiles.Nodes[0].Text = this.m_sequence.Name;
            XmlNode node = this.m_sequence.Extensions[this.FileExtension];
            string path = Path.Combine(Paths.SourceFilePath, this.m_sequence.Name);
            if (Directory.Exists(path))
            {
                foreach (string str2 in Directory.GetFiles(path, "*.cs"))
                {
                    if (File.Exists(str2))
                    {
                        this.AddFile(str2);
                    }
                }
            }
            this.Text = this.m_sequence.Name;
            return this.m_sequence;
        }

        private void ProgramChange(Vixen.ProgramChange changeType)
        {
            if (changeType == Vixen.ProgramChange.End)
            {
                this.SetUIState(false);
            }
        }

        private string Rationalize(string str)
        {
            if (str.Length == 0)
            {
                return str;
            }
            if (char.IsDigit(str[0]))
            {
                str = "_" + str;
            }
            string str2 = str;
            foreach (char ch in str)
            {
                if (!(char.IsLetterOrDigit(ch) || (ch == '_')))
                {
                    str2 = str2.Replace(ch, '_');
                }
            }
            return str2;
        }

        private void referencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XmlNode contextNode = this.m_sequence.Extensions[this.FileExtension];
            TextListDialog dialog = new TextListDialog(Xml.GetNodeAlways(contextNode, "AssemblyReferences"), "AssemblyReference", true);
            dialog.Text = "References";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                base.IsDirty = true;
            }
            dialog.Dispose();
        }

        public override DialogResult RunWizard(ref EventSequence resultSequence)
        {
            return DialogResult.None;
        }

        public override void SaveTo(string filePath)
        {
            this.SaveUsing(Paths.SourceFilePath);
            this.WriteTo(Paths.SourceFilePath, filePath);
            this.treeViewProjectFiles.Nodes[0].Text = this.m_sequence.Name;
            this.Text = this.m_sequence.Name;
            base.IsDirty = false;
        }

        private void SaveUsing(string sourceDirectory)
        {
            this.treeViewProjectFiles.BeginUpdate();
            TextQueryDialog dialog = new TextQueryDialog("Save File As", string.Empty, string.Empty);
            foreach (TabPageDoc doc in this.tabControl.TabPages)
            {
                if (doc.IsNew)
                {
                    dialog.Query = string.Format("Save {0} as...", doc.DisplayName);
                    if (dialog.ShowDialog() == DialogResult.Cancel)
                    {
                        dialog.Dispose();
                        return;
                    }
                    doc.FileName = Path.ChangeExtension(dialog.Response, ".cs");
                    ((TreeNode) doc.Tag).Text = doc.DisplayName;
                }
            }
            this.treeViewProjectFiles.EndUpdate();
            dialog.Dispose();
            XmlNode contextNode = this.m_sequence.Extensions[this.FileExtension];
            Xml.SetValue(contextNode, "SourceDirectory", this.m_sequence.Name);
            XmlNode node2 = Xml.SetValue(contextNode, "SourceFiles", string.Empty);
            foreach (TabPageDoc doc in this.tabControl.TabPages)
            {
                Xml.SetNewValue(node2, "SourceFile", doc.FileName);
            }
        }

        private void scriptToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            Keys modifierKeys = Control.ModifierKeys;
            this.debugOutputToolStripMenuItem.Visible = ((modifierKeys & (Keys.Control | Keys.Shift)) == (Keys.Control | Keys.Shift)) || this.debugOutputToolStripMenuItem.Checked;
            this.attachedPluginsToolStripMenuItem.ForeColor = this.m_sequence.PlugInData.IsEmpty ? Color.Red : Color.Black;
        }

        private void SetUIState(bool executing)
        {
            this.toolStripButtonStop.Enabled = executing;
            this.toolStripButtonExecute.Enabled = !executing;
        }

        private void StandardScript_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((e.CloseReason == CloseReason.UserClosing) && (base.IsDirty || this.DocsDirty()))
            {
                switch (MessageBox.Show("Save changes?", Vendor.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                {
                    case DialogResult.OK:
                        ((ISystem) Interfaces.Available["ISystem"]).InvokeSave(this);
                        break;

                    case DialogResult.Cancel:
                        e.Cancel = true;
                        return;
                }
            }
            this.m_executionContext.ReleaseContext(this.m_executionContextHandle);
        }

        private void TabPageDoc_DirtyChanged(object sender, EventArgs e)
        {
            base.IsDirty = true;
        }

        private void toolStripButtonExecute_Click(object sender, EventArgs e)
        {
            this.Execute();
        }

        private void toolStripButtonStop_Click(object sender, EventArgs e)
        {
            this.m_executionContext.ExecuteStop(this.m_executionContextHandle);
        }

        private void UpdateChannelList()
        {
            int topIndex = this.listBoxChannels.TopIndex;
            this.listBoxChannels.BeginUpdate();
            this.listBoxChannels.Items.Clear();
            foreach (Channel channel in this.m_sequence.Channels)
            {
                this.listBoxChannels.Items.Add(channel.Name);
            }
            this.listBoxChannels.TopIndex = topIndex;
            this.listBoxChannels.EndUpdate();
            base.IsDirty = true;
        }

        private void WriteTo(string sourceDirectory, string sequenceFileName)
        {
            string path = Path.Combine(sourceDirectory, Path.GetFileNameWithoutExtension(sequenceFileName));
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            foreach (TabPageDoc doc in this.tabControl.TabPages)
            {
                doc.SaveTo(path);
            }
            this.m_sequence.SaveTo(Path.Combine(Paths.SequencePath, Path.GetFileName(sequenceFileName)));
        }

        public override string Author
        {
            get
            {
                return "Vixen and VixenPlus Developers";
            }
        }

        public override string Description
        {
            get
            {
                return "Editor for the standard script engine";
            }
        }

        public override string FileExtension
        {
            get
            {
                return ".vsp";
            }
        }

        public override string FileTypeDescription
        {
            get
            {
                return "Script project";
            }
        }

        public override EventSequence Sequence
        {
            get
            {
                return this.m_sequence;
            }
            set
            {
                this.m_sequence = value;
            }
        }
    }
}

