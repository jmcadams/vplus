namespace LedTriks
{
    using LedTriksUtil;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Windows.Forms;
    using System.Xml;
    using Vixen;
    using Vixen.Dialogs;

    public class LedTriks : UIBase
    {
        private ToolStripMenuItem attachedPluginsToolStripMenuItem;
        private ToolStripMenuItem autoresetAutoTextDefaultsToolStripMenuItem;
        private const int BOARD_HEIGHT = 0x10;
        private const int BOARD_WIDTH = 0x30;
        private ToolStripMenuItem boardLayoutToolStripMenuItem;
        private Button buttonClear;
        private Button buttonClone;
        private Button buttonCreateAutoText;
        private Button buttonFont;
        private Button buttonNext;
        private Button buttonOrganize;
        private Button buttonPrevious;
        private Button buttonRemove;
        private Button buttonRemoveAll;
        private Button buttonSaveAndCreateNew;
        private Button buttonSaveFrame;
        private ToolStripMenuItem cellSizeToolStripMenuItem;
        private CheckBox checkBoxLoop;
        private IContainer components;
        private ToolStripMenuItem confirmOnFrameRemoveToolStripMenuItem;
        private ToolStripMenuItem copyThisFrameToolStripMenuItem;
        private ToolStripMenuItem defaultFrameLengthToolStripMenuItem;
        private ToolStripMenuItem executeToolStripMenuItem;
        private FontDialog fontDialog;
        private ToolStripMenuItem ghostThePreviousFrameToolStripMenuItem;
        private GroupBox groupBoxAutoText;
        private GroupBox groupBoxCurrentFrame;
        private GroupBox groupBoxDisplay;
        private GroupBox groupBoxPreviewControl;
        private ToolStripMenuItem ignoreFontDescentToolStripMenuItem;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label labelCurrentFrame;
        private Label labelFontName;
        private Label labelLength;
        private Label labelPosition;
        private ToolStripMenuItem ledTriksToolStripMenuItem;
        private LinkLabel linkLabelMoreOptions;
        private Size m_boardLayout;
        private int m_cellSize;
        private List<uint> m_clipboard;
        private Dictionary<uint, uint> m_currentFrameCells;
        private int m_currentFrameIndex;
        private XmlNode m_dataNode;
        private int m_defaultFrameLength;
        private bool m_execute;
        private int m_executionContextHandle;
        private IExecution m_executionInterface;
        private List<Frame> m_frames;
        private Generator m_generator;
        private bool m_ghostPreviousState;
        private Point m_mouseAt;
        private MouseButtons m_mouseDown;
        private bool m_pasting;
        private bool m_playing;
        private int m_playStartFrame;
        private Thread m_playThread;
        private Preference2 m_preferences;
        private Point m_previousPosition;
        private bool m_resizing;
        private EventSequence m_sequence;
        private SetFrameDelegate m_setFrame;
        private Stopwatch m_stopwatch;
        private MenuStrip menuStrip;
        private Panel panel;
        private Panel panel1;
        private ToolStripMenuItem pasteToolStripMenuItem;
        private PictureBox pictureBoxPlay;
        private PictureBox pictureBoxPlayFromBeginning;
        private PictureBox pictureBoxPreview;
        private PictureBox pictureBoxShiftDown;
        private PictureBox pictureBoxShiftLeft;
        private PictureBox pictureBoxShiftRight;
        private PictureBox pictureBoxShiftUp;
        private PictureBox pictureBoxStop;
        private ToolStripMenuItem playControlButtonsToolStripMenuItem;
        private ToolStripMenuItem previewToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private SaveFileDialog saveFileDialog;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem sequenceToolStripMenuItem;
        private TextBox textBoxAutoText;
        private TextBox textBoxFrameLength;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem toolStripMenuItem10;
        private ToolStripMenuItem toolStripMenuItem11;
        private ToolStripMenuItem toolStripMenuItem12;
        private ToolStripMenuItem toolStripMenuItem13;
        private ToolStripSeparator toolStripMenuItem5;
        private ToolStripSeparator toolStripMenuItem6;
        private ToolStripSeparator toolStripMenuItem7;
        private ToolStripTextBox toolStripTextBoxFrameLength;
        private ToolTip toolTip;
        private TrackBar trackBarFrame;

        public LedTriks()
        {
            object obj3;
            this.m_sequence = null;
            this.m_resizing = false;
            this.m_mouseDown = MouseButtons.None;
            this.m_cellSize = 5;
            this.m_mouseAt = new Point();
            this.m_currentFrameIndex = -1;
            this.m_pasting = false;
            this.m_execute = false;
            this.m_executionInterface = null;
            this.m_executionContextHandle = 0;
            this.m_previousPosition = new Point(0, 0);
            this.m_generator = null;
            this.m_playing = false;
            this.InitializeComponent();
            object obj2 = null;
            if (Interfaces.Available.TryGetValue("ISystem", out obj2))
            {
                this.m_preferences = ((ISystem) obj2).UserPreferences;
            }
            this.m_generator = new Generator();
            this.m_clipboard = new List<uint>();
            this.m_setFrame = new SetFrameDelegate(this.SetFrame);
            this.SetDefaultFrameLength(50);
            this.m_currentFrameCells = new Dictionary<uint, uint>();
            this.m_frames = new List<Frame>();
            this.m_stopwatch = new Stopwatch();
            this.ResetAutoText();
            if (Interfaces.Available.TryGetValue("IExecution", out obj3))
            {
                this.m_executionInterface = (IExecution) obj3;
            }
        }

        private void attachedPluginsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PluginListDialog dialog = new PluginListDialog(this.m_sequence);
            dialog.ShowDialog();
            dialog.Dispose();
            base.IsDirty = true;
        }

        private void boardLayoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LedTriks.BoardLayoutDialog dialog = new LedTriks.BoardLayoutDialog(this.m_boardLayout);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.SetBoardLayout(dialog.BoardLayout.Width, dialog.BoardLayout.Height);
            }
            dialog.Dispose();
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            this.m_currentFrameCells.Clear();
            base.IsDirty = true;
            this.pictureBoxPreview.Refresh();
        }

        private void buttonClone_Click(object sender, EventArgs e)
        {
            this.m_frames.Insert(this.m_currentFrameIndex + 1, this.m_frames[this.m_currentFrameIndex].Clone());
            this.SetFrame(this.m_currentFrameIndex + 1);
            base.IsDirty = true;
        }

        private void buttonCreateAutoText_Click(object sender, EventArgs e)
        {
            if (this.m_generator.TextFont == null)
            {
                MessageBox.Show("Please select a font first.", "Vixen", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
            else
            {
                this.Cursor = Cursors.WaitCursor;
                try
                {
                    this.m_generator.IgnoreFontDescent = this.ignoreFontDescentToolStripMenuItem.Checked;
                    List<Frame> collection = this.m_generator.GenerateTextFrames(this.textBoxAutoText.Text);
                    if (this.autoresetAutoTextDefaultsToolStripMenuItem.Checked)
                    {
                        this.ResetAutoText();
                    }
                    this.m_frames.AddRange(collection);
                    this.SetFrame(this.m_frames.Count - 1);
                    this.UpdateTotalLength();
                    base.IsDirty = true;
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }

        private void buttonFont_Click(object sender, EventArgs e)
        {
            if (this.fontDialog.ShowDialog() == DialogResult.OK)
            {
                this.SetFont(this.fontDialog.Font);
            }
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (this.m_currentFrameIndex < (this.m_frames.Count - 1))
            {
                this.SetFrame(this.m_currentFrameIndex + 1);
            }
        }

        private void buttonOrganize_Click(object sender, EventArgs e)
        {
            FrameOrganizationDialog dialog = new FrameOrganizationDialog(this.m_frames, this.m_boardLayout.Width * 0x30, this.m_boardLayout.Height * 0x10);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.m_frames = dialog.Frames;
                if (this.m_currentFrameIndex >= this.m_frames.Count)
                {
                    if (this.m_frames.Count > 0)
                    {
                        this.SetFrame(this.m_frames.Count - 1);
                    }
                    else
                    {
                        this.NewFrame();
                    }
                }
                else
                {
                    this.SetFrame(this.m_currentFrameIndex);
                }
                base.IsDirty = true;
            }
            dialog.Dispose();
        }

        private void buttonPrevious_Click(object sender, EventArgs e)
        {
            if (this.m_currentFrameIndex > 0)
            {
                this.SetFrame(this.m_currentFrameIndex - 1);
            }
        }

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            if ((this.m_frames.Count != 0) && (!this.confirmOnFrameRemoveToolStripMenuItem.Checked || (MessageBox.Show("Remove this frame?", "Vixen", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)))
            {
                this.m_frames.RemoveAt(this.m_currentFrameIndex);
                if (this.m_frames.Count > 0)
                {
                    this.UpdateTotalLength();
                    this.trackBarFrame.Maximum--;
                    this.SetFrame((this.m_currentFrameIndex == this.m_frames.Count) ? (this.m_frames.Count - 1) : this.m_currentFrameIndex);
                }
                else
                {
                    this.NewFrame();
                }
                base.IsDirty = true;
            }
        }

        private void buttonRemoveAll_Click(object sender, EventArgs e)
        {
            if ((this.m_frames.Count != 0) && (!this.confirmOnFrameRemoveToolStripMenuItem.Checked || (MessageBox.Show("Remove ALL frames?", "Vixen", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)))
            {
                this.m_frames.Clear();
                this.trackBarFrame.Maximum = this.trackBarFrame.Minimum;
                this.NewFrame();
            }
        }

        private void buttonSaveAndCreateNew_Click(object sender, EventArgs e)
        {
            this.SaveFrame();
            this.NewFrame();
        }

        private void buttonSaveFrame_Click(object sender, EventArgs e)
        {
            this.SaveFrame();
        }

        private void checkBoxLoop_CheckedChanged(object sender, EventArgs e)
        {
            this.m_executionInterface.SetLoopState(this.m_executionContextHandle, this.checkBoxLoop.Checked);
        }

        private void copyThisFrameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.m_clipboard.Clear();
            this.m_clipboard.AddRange(this.m_frames[this.m_currentFrameIndex].Cells);
        }

        protected override void Dispose(bool disposing)
        {
            if (this.m_generator != null)
            {
                this.m_generator.Dispose();
            }
            base.Dispose(disposing);
            GC.SuppressFinalize(this);
        }

        private void executeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.m_execute = true;
            this.previewToolStripMenuItem.Checked = false;
            this.executeToolStripMenuItem.Checked = true;
        }

        ~LedTriks()
        {
            base.Dispose();
        }

        private void ghostThePreviousFrameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.pictureBoxPreview.Refresh();
        }

        private void Init()
        {
            this.m_executionContextHandle = this.m_executionInterface.RequestContext(true, false, null);
            this.m_executionInterface.SetSynchronousContext(this.m_executionContextHandle, this.m_sequence);
        }

        private void InitializeComponent()
        {
            this.components = new Container();
            ComponentResourceManager manager = new ComponentResourceManager(typeof(LedTriks.LedTriks));
            this.menuStrip = new MenuStrip();
            this.sequenceToolStripMenuItem = new ToolStripMenuItem();
            this.saveToolStripMenuItem = new ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripMenuItem6 = new ToolStripSeparator();
            this.ledTriksToolStripMenuItem = new ToolStripMenuItem();
            this.boardLayoutToolStripMenuItem = new ToolStripMenuItem();
            this.cellSizeToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripMenuItem10 = new ToolStripMenuItem();
            this.toolStripMenuItem11 = new ToolStripMenuItem();
            this.toolStripMenuItem12 = new ToolStripMenuItem();
            this.toolStripMenuItem13 = new ToolStripMenuItem();
            this.defaultFrameLengthToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripTextBoxFrameLength = new ToolStripTextBox();
            this.toolStripMenuItem1 = new ToolStripSeparator();
            this.ignoreFontDescentToolStripMenuItem = new ToolStripMenuItem();
            this.confirmOnFrameRemoveToolStripMenuItem = new ToolStripMenuItem();
            this.ghostThePreviousFrameToolStripMenuItem = new ToolStripMenuItem();
            this.autoresetAutoTextDefaultsToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripMenuItem5 = new ToolStripSeparator();
            this.copyThisFrameToolStripMenuItem = new ToolStripMenuItem();
            this.pasteToolStripMenuItem = new ToolStripMenuItem();
            this.toolStripMenuItem7 = new ToolStripSeparator();
            this.attachedPluginsToolStripMenuItem = new ToolStripMenuItem();
            this.playControlButtonsToolStripMenuItem = new ToolStripMenuItem();
            this.previewToolStripMenuItem = new ToolStripMenuItem();
            this.executeToolStripMenuItem = new ToolStripMenuItem();
            this.panel = new Panel();
            this.checkBoxLoop = new CheckBox();
            this.groupBoxCurrentFrame = new GroupBox();
            this.buttonClone = new Button();
            this.label2 = new Label();
            this.textBoxFrameLength = new TextBox();
            this.label3 = new Label();
            this.buttonSaveFrame = new Button();
            this.buttonSaveAndCreateNew = new Button();
            this.pictureBoxShiftUp = new PictureBox();
            this.buttonClear = new Button();
            this.pictureBoxShiftDown = new PictureBox();
            this.pictureBoxShiftLeft = new PictureBox();
            this.pictureBoxShiftRight = new PictureBox();
            this.groupBoxAutoText = new GroupBox();
            this.linkLabelMoreOptions = new LinkLabel();
            this.buttonCreateAutoText = new Button();
            this.textBoxAutoText = new TextBox();
            this.labelFontName = new Label();
            this.buttonFont = new Button();
            this.buttonOrganize = new Button();
            this.labelLength = new Label();
            this.label1 = new Label();
            this.groupBoxDisplay = new GroupBox();
            this.panel1 = new Panel();
            this.pictureBoxPreview = new PictureBox();
            this.buttonNext = new Button();
            this.labelPosition = new Label();
            this.buttonPrevious = new Button();
            this.labelCurrentFrame = new Label();
            this.buttonRemoveAll = new Button();
            this.buttonRemove = new Button();
            this.trackBarFrame = new TrackBar();
            this.groupBoxPreviewControl = new GroupBox();
            this.pictureBoxStop = new PictureBox();
            this.pictureBoxPlayFromBeginning = new PictureBox();
            this.pictureBoxPlay = new PictureBox();
            this.toolTip = new ToolTip(this.components);
            this.fontDialog = new FontDialog();
            this.saveFileDialog = new SaveFileDialog();
            this.menuStrip.SuspendLayout();
            this.panel.SuspendLayout();
            this.groupBoxCurrentFrame.SuspendLayout();
            ((ISupportInitialize) this.pictureBoxShiftUp).BeginInit();
            ((ISupportInitialize) this.pictureBoxShiftDown).BeginInit();
            ((ISupportInitialize) this.pictureBoxShiftLeft).BeginInit();
            ((ISupportInitialize) this.pictureBoxShiftRight).BeginInit();
            this.groupBoxAutoText.SuspendLayout();
            this.groupBoxDisplay.SuspendLayout();
            this.panel1.SuspendLayout();
            ((ISupportInitialize) this.pictureBoxPreview).BeginInit();
            this.trackBarFrame.BeginInit();
            this.groupBoxPreviewControl.SuspendLayout();
            ((ISupportInitialize) this.pictureBoxStop).BeginInit();
            ((ISupportInitialize) this.pictureBoxPlayFromBeginning).BeginInit();
            ((ISupportInitialize) this.pictureBoxPlay).BeginInit();
            base.SuspendLayout();
            this.menuStrip.Items.AddRange(new ToolStripItem[] { this.sequenceToolStripMenuItem, this.ledTriksToolStripMenuItem });
            this.menuStrip.Location = new Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new Size(0x318, 0x18);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            this.menuStrip.Visible = false;
            this.sequenceToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.saveToolStripMenuItem, this.saveAsToolStripMenuItem, this.toolStripMenuItem6 });
            this.sequenceToolStripMenuItem.MergeAction = MergeAction.MatchOnly;
            this.sequenceToolStripMenuItem.Name = "sequenceToolStripMenuItem";
            this.sequenceToolStripMenuItem.Size = new Size(70, 20);
            this.sequenceToolStripMenuItem.Text = "Sequence";
            this.saveToolStripMenuItem.MergeIndex = 3;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new Size(0x70, 0x16);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Visible = false;
            this.saveToolStripMenuItem.Click += new EventHandler(this.saveToolStripMenuItem_Click);
            this.saveAsToolStripMenuItem.MergeIndex = 4;
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new Size(0x70, 0x16);
            this.saveAsToolStripMenuItem.Text = "Save as";
            this.saveAsToolStripMenuItem.Visible = false;
            this.saveAsToolStripMenuItem.Click += new EventHandler(this.saveAsToolStripMenuItem_Click);
            this.toolStripMenuItem6.MergeIndex = 5;
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new Size(0x6d, 6);
            this.toolStripMenuItem6.Visible = false;
            this.ledTriksToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.boardLayoutToolStripMenuItem, this.cellSizeToolStripMenuItem, this.defaultFrameLengthToolStripMenuItem, this.toolStripMenuItem1, this.ignoreFontDescentToolStripMenuItem, this.confirmOnFrameRemoveToolStripMenuItem, this.ghostThePreviousFrameToolStripMenuItem, this.autoresetAutoTextDefaultsToolStripMenuItem, this.toolStripMenuItem5, this.copyThisFrameToolStripMenuItem, this.pasteToolStripMenuItem, this.toolStripMenuItem7, this.attachedPluginsToolStripMenuItem, this.playControlButtonsToolStripMenuItem });
            this.ledTriksToolStripMenuItem.MergeAction = MergeAction.Insert;
            this.ledTriksToolStripMenuItem.MergeIndex = 5;
            this.ledTriksToolStripMenuItem.Name = "ledTriksToolStripMenuItem";
            this.ledTriksToolStripMenuItem.Size = new Size(0x3f, 20);
            this.ledTriksToolStripMenuItem.Text = "LedTriks";
            this.boardLayoutToolStripMenuItem.Name = "boardLayoutToolStripMenuItem";
            this.boardLayoutToolStripMenuItem.Size = new Size(0xe0, 0x16);
            this.boardLayoutToolStripMenuItem.Text = "Board layout";
            this.boardLayoutToolStripMenuItem.Click += new EventHandler(this.boardLayoutToolStripMenuItem_Click);
            this.cellSizeToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.toolStripMenuItem10, this.toolStripMenuItem11, this.toolStripMenuItem12, this.toolStripMenuItem13 });
            this.cellSizeToolStripMenuItem.Name = "cellSizeToolStripMenuItem";
            this.cellSizeToolStripMenuItem.Size = new Size(0xe0, 0x16);
            this.cellSizeToolStripMenuItem.Text = "Cell size";
            this.toolStripMenuItem10.Name = "toolStripMenuItem10";
            this.toolStripMenuItem10.Size = new Size(0x56, 0x16);
            this.toolStripMenuItem10.Text = "3";
            this.toolStripMenuItem10.Click += new EventHandler(this.toolStripMenuItem10_Click);
            this.toolStripMenuItem11.Checked = true;
            this.toolStripMenuItem11.CheckState = CheckState.Checked;
            this.toolStripMenuItem11.Name = "toolStripMenuItem11";
            this.toolStripMenuItem11.Size = new Size(0x56, 0x16);
            this.toolStripMenuItem11.Text = "5";
            this.toolStripMenuItem11.Click += new EventHandler(this.toolStripMenuItem10_Click);
            this.toolStripMenuItem12.Name = "toolStripMenuItem12";
            this.toolStripMenuItem12.Size = new Size(0x56, 0x16);
            this.toolStripMenuItem12.Text = "7";
            this.toolStripMenuItem12.Click += new EventHandler(this.toolStripMenuItem10_Click);
            this.toolStripMenuItem13.Name = "toolStripMenuItem13";
            this.toolStripMenuItem13.Size = new Size(0x56, 0x16);
            this.toolStripMenuItem13.Text = "10";
            this.toolStripMenuItem13.Click += new EventHandler(this.toolStripMenuItem10_Click);
            this.defaultFrameLengthToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.toolStripTextBoxFrameLength });
            this.defaultFrameLengthToolStripMenuItem.Name = "defaultFrameLengthToolStripMenuItem";
            this.defaultFrameLengthToolStripMenuItem.Size = new Size(0xe0, 0x16);
            this.defaultFrameLengthToolStripMenuItem.Text = "Default frame length";
            this.toolStripTextBoxFrameLength.Name = "toolStripTextBoxFrameLength";
            this.toolStripTextBoxFrameLength.Size = new Size(50, 0x17);
            this.toolStripTextBoxFrameLength.KeyPress += new KeyPressEventHandler(this.toolStripTextBoxFrameLength_KeyPress);
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new Size(0xdd, 6);
            this.ignoreFontDescentToolStripMenuItem.CheckOnClick = true;
            this.ignoreFontDescentToolStripMenuItem.Name = "ignoreFontDescentToolStripMenuItem";
            this.ignoreFontDescentToolStripMenuItem.Size = new Size(0xe0, 0x16);
            this.ignoreFontDescentToolStripMenuItem.Text = "Ignore font descent";
            this.confirmOnFrameRemoveToolStripMenuItem.Checked = true;
            this.confirmOnFrameRemoveToolStripMenuItem.CheckOnClick = true;
            this.confirmOnFrameRemoveToolStripMenuItem.CheckState = CheckState.Checked;
            this.confirmOnFrameRemoveToolStripMenuItem.Name = "confirmOnFrameRemoveToolStripMenuItem";
            this.confirmOnFrameRemoveToolStripMenuItem.Size = new Size(0xe0, 0x16);
            this.confirmOnFrameRemoveToolStripMenuItem.Text = "Confirm on frame remove";
            this.ghostThePreviousFrameToolStripMenuItem.Checked = true;
            this.ghostThePreviousFrameToolStripMenuItem.CheckOnClick = true;
            this.ghostThePreviousFrameToolStripMenuItem.CheckState = CheckState.Checked;
            this.ghostThePreviousFrameToolStripMenuItem.Name = "ghostThePreviousFrameToolStripMenuItem";
            this.ghostThePreviousFrameToolStripMenuItem.Size = new Size(0xe0, 0x16);
            this.ghostThePreviousFrameToolStripMenuItem.Text = "Ghost the previous frame";
            this.ghostThePreviousFrameToolStripMenuItem.Click += new EventHandler(this.ghostThePreviousFrameToolStripMenuItem_Click);
            this.autoresetAutoTextDefaultsToolStripMenuItem.CheckOnClick = true;
            this.autoresetAutoTextDefaultsToolStripMenuItem.Name = "autoresetAutoTextDefaultsToolStripMenuItem";
            this.autoresetAutoTextDefaultsToolStripMenuItem.Size = new Size(0xe0, 0x16);
            this.autoresetAutoTextDefaultsToolStripMenuItem.Text = "Auto-reset auto text defaults";
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new Size(0xdd, 6);
            this.copyThisFrameToolStripMenuItem.Name = "copyThisFrameToolStripMenuItem";
            this.copyThisFrameToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.C;
            this.copyThisFrameToolStripMenuItem.Size = new Size(0xe0, 0x16);
            this.copyThisFrameToolStripMenuItem.Text = "Copy this frame";
            this.copyThisFrameToolStripMenuItem.Click += new EventHandler(this.copyThisFrameToolStripMenuItem_Click);
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.ShortcutKeys = Keys.Control | Keys.V;
            this.pasteToolStripMenuItem.Size = new Size(0xe0, 0x16);
            this.pasteToolStripMenuItem.Text = "Paste";
            this.pasteToolStripMenuItem.Click += new EventHandler(this.pasteToolStripMenuItem_Click);
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new Size(0xdd, 6);
            this.attachedPluginsToolStripMenuItem.Name = "attachedPluginsToolStripMenuItem";
            this.attachedPluginsToolStripMenuItem.Size = new Size(0xe0, 0x16);
            this.attachedPluginsToolStripMenuItem.Text = "Attached plugins";
            this.attachedPluginsToolStripMenuItem.Click += new EventHandler(this.attachedPluginsToolStripMenuItem_Click);
            this.playControlButtonsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { this.previewToolStripMenuItem, this.executeToolStripMenuItem });
            this.playControlButtonsToolStripMenuItem.Name = "playControlButtonsToolStripMenuItem";
            this.playControlButtonsToolStripMenuItem.Size = new Size(0xe0, 0x16);
            this.playControlButtonsToolStripMenuItem.Text = "Play control buttons";
            this.previewToolStripMenuItem.Checked = true;
            this.previewToolStripMenuItem.CheckState = CheckState.Checked;
            this.previewToolStripMenuItem.Name = "previewToolStripMenuItem";
            this.previewToolStripMenuItem.Size = new Size(0x73, 0x16);
            this.previewToolStripMenuItem.Text = "Preview";
            this.previewToolStripMenuItem.Click += new EventHandler(this.previewToolStripMenuItem_Click);
            this.executeToolStripMenuItem.Name = "executeToolStripMenuItem";
            this.executeToolStripMenuItem.Size = new Size(0x73, 0x16);
            this.executeToolStripMenuItem.Text = "Execute";
            this.executeToolStripMenuItem.Click += new EventHandler(this.executeToolStripMenuItem_Click);
            this.panel.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.panel.Controls.Add(this.checkBoxLoop);
            this.panel.Controls.Add(this.groupBoxCurrentFrame);
            this.panel.Controls.Add(this.groupBoxAutoText);
            this.panel.Controls.Add(this.buttonOrganize);
            this.panel.Controls.Add(this.labelLength);
            this.panel.Controls.Add(this.label1);
            this.panel.Controls.Add(this.groupBoxDisplay);
            this.panel.Controls.Add(this.groupBoxPreviewControl);
            this.panel.Location = new Point(11, 12);
            this.panel.Name = "panel";
            this.panel.Size = new Size(0x301, 0x219);
            this.panel.TabIndex = 0;
            this.checkBoxLoop.AutoSize = true;
            this.checkBoxLoop.Location = new Point(0x167, 50);
            this.checkBoxLoop.Name = "checkBoxLoop";
            this.checkBoxLoop.Size = new Size(50, 0x11);
            this.checkBoxLoop.TabIndex = 15;
            this.checkBoxLoop.Text = "Loop";
            this.checkBoxLoop.UseVisualStyleBackColor = true;
            this.checkBoxLoop.CheckedChanged += new EventHandler(this.checkBoxLoop_CheckedChanged);
            this.groupBoxCurrentFrame.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.groupBoxCurrentFrame.Controls.Add(this.buttonClone);
            this.groupBoxCurrentFrame.Controls.Add(this.label2);
            this.groupBoxCurrentFrame.Controls.Add(this.textBoxFrameLength);
            this.groupBoxCurrentFrame.Controls.Add(this.label3);
            this.groupBoxCurrentFrame.Controls.Add(this.buttonSaveFrame);
            this.groupBoxCurrentFrame.Controls.Add(this.buttonSaveAndCreateNew);
            this.groupBoxCurrentFrame.Controls.Add(this.pictureBoxShiftUp);
            this.groupBoxCurrentFrame.Controls.Add(this.buttonClear);
            this.groupBoxCurrentFrame.Controls.Add(this.pictureBoxShiftDown);
            this.groupBoxCurrentFrame.Controls.Add(this.pictureBoxShiftLeft);
            this.groupBoxCurrentFrame.Controls.Add(this.pictureBoxShiftRight);
            this.groupBoxCurrentFrame.Location = new Point(0x13, 0x128);
            this.groupBoxCurrentFrame.Name = "groupBoxCurrentFrame";
            this.groupBoxCurrentFrame.Size = new Size(730, 0x69);
            this.groupBoxCurrentFrame.TabIndex = 2;
            this.groupBoxCurrentFrame.TabStop = false;
            this.groupBoxCurrentFrame.Text = "Current Frame";
            this.buttonClone.Location = new Point(0x7f, 0x47);
            this.buttonClone.Name = "buttonClone";
            this.buttonClone.Size = new Size(0x6f, 0x17);
            this.buttonClone.TabIndex = 6;
            this.buttonClone.Text = "Clone this frame";
            this.buttonClone.UseVisualStyleBackColor = true;
            this.buttonClone.Click += new EventHandler(this.buttonClone_Click);
            this.label2.AutoSize = true;
            this.label2.Location = new Point(15, 20);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x6b, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "This frame will last for";
            this.textBoxFrameLength.Location = new Point(0x80, 0x11);
            this.textBoxFrameLength.Name = "textBoxFrameLength";
            this.textBoxFrameLength.Size = new Size(0x29, 20);
            this.textBoxFrameLength.TabIndex = 1;
            this.label3.AutoSize = true;
            this.label3.Location = new Point(0xaf, 20);
            this.label3.Name = "label3";
            this.label3.Size = new Size(0x3f, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "milliseconds";
            this.buttonSaveFrame.Location = new Point(0x12, 0x2a);
            this.buttonSaveFrame.Name = "buttonSaveFrame";
            this.buttonSaveFrame.Size = new Size(0x67, 0x17);
            this.buttonSaveFrame.TabIndex = 3;
            this.buttonSaveFrame.Text = "Save this frame";
            this.buttonSaveFrame.UseVisualStyleBackColor = true;
            this.buttonSaveFrame.Click += new EventHandler(this.buttonSaveFrame_Click);
            this.buttonSaveAndCreateNew.Location = new Point(0x7f, 0x2a);
            this.buttonSaveAndCreateNew.Name = "buttonSaveAndCreateNew";
            this.buttonSaveAndCreateNew.Size = new Size(0x97, 0x17);
            this.buttonSaveAndCreateNew.TabIndex = 4;
            this.buttonSaveAndCreateNew.Text = "Save and create new frame";
            this.buttonSaveAndCreateNew.UseVisualStyleBackColor = true;
            this.buttonSaveAndCreateNew.Click += new EventHandler(this.buttonSaveAndCreateNew_Click);
            this.pictureBoxShiftUp.Image = (Image) manager.GetObject("pictureBoxShiftUp.Image");
            this.pictureBoxShiftUp.Location = new Point(0x16d, 0x1a);
            this.pictureBoxShiftUp.Name = "pictureBoxShiftUp";
            this.pictureBoxShiftUp.Size = new Size(0x18, 0x18);
            this.pictureBoxShiftUp.TabIndex = 2;
            this.pictureBoxShiftUp.TabStop = false;
            this.toolTip.SetToolTip(this.pictureBoxShiftUp, "Shift frame up");
            this.pictureBoxShiftUp.DoubleClick += new EventHandler(this.pictureBoxShiftUp_DoubleClick);
            this.pictureBoxShiftUp.Click += new EventHandler(this.pictureBoxShiftUp_Click);
            this.buttonClear.Location = new Point(0x12, 0x47);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new Size(0x4b, 0x17);
            this.buttonClear.TabIndex = 5;
            this.buttonClear.Text = "Clear frame";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new EventHandler(this.buttonClear_Click);
            this.pictureBoxShiftDown.Image = (Image) manager.GetObject("pictureBoxShiftDown.Image");
            this.pictureBoxShiftDown.Location = new Point(0x16d, 0x3b);
            this.pictureBoxShiftDown.Name = "pictureBoxShiftDown";
            this.pictureBoxShiftDown.Size = new Size(0x18, 0x18);
            this.pictureBoxShiftDown.TabIndex = 3;
            this.pictureBoxShiftDown.TabStop = false;
            this.toolTip.SetToolTip(this.pictureBoxShiftDown, "Shift frame down");
            this.pictureBoxShiftDown.DoubleClick += new EventHandler(this.pictureBoxShiftDown_DoubleClick);
            this.pictureBoxShiftDown.Click += new EventHandler(this.pictureBoxShiftDown_Click);
            this.pictureBoxShiftLeft.Image = (Image) manager.GetObject("pictureBoxShiftLeft.Image");
            this.pictureBoxShiftLeft.Location = new Point(0x14f, 0x2a);
            this.pictureBoxShiftLeft.Name = "pictureBoxShiftLeft";
            this.pictureBoxShiftLeft.Size = new Size(0x18, 0x18);
            this.pictureBoxShiftLeft.TabIndex = 4;
            this.pictureBoxShiftLeft.TabStop = false;
            this.toolTip.SetToolTip(this.pictureBoxShiftLeft, "Shift frame left");
            this.pictureBoxShiftLeft.DoubleClick += new EventHandler(this.pictureBoxShiftLeft_DoubleClick);
            this.pictureBoxShiftLeft.Click += new EventHandler(this.pictureBoxShiftLeft_Click);
            this.pictureBoxShiftRight.Image = (Image) manager.GetObject("pictureBoxShiftRight.Image");
            this.pictureBoxShiftRight.Location = new Point(0x18b, 0x2a);
            this.pictureBoxShiftRight.Name = "pictureBoxShiftRight";
            this.pictureBoxShiftRight.Size = new Size(0x18, 0x18);
            this.pictureBoxShiftRight.TabIndex = 5;
            this.pictureBoxShiftRight.TabStop = false;
            this.toolTip.SetToolTip(this.pictureBoxShiftRight, "Shift frame right");
            this.pictureBoxShiftRight.DoubleClick += new EventHandler(this.pictureBoxShiftRight_DoubleClick);
            this.pictureBoxShiftRight.Click += new EventHandler(this.pictureBoxShiftRight_Click);
            this.groupBoxAutoText.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.groupBoxAutoText.Controls.Add(this.linkLabelMoreOptions);
            this.groupBoxAutoText.Controls.Add(this.buttonCreateAutoText);
            this.groupBoxAutoText.Controls.Add(this.textBoxAutoText);
            this.groupBoxAutoText.Controls.Add(this.labelFontName);
            this.groupBoxAutoText.Controls.Add(this.buttonFont);
            this.groupBoxAutoText.Location = new Point(0x13, 0x1a3);
            this.groupBoxAutoText.Name = "groupBoxAutoText";
            this.groupBoxAutoText.Size = new Size(730, 100);
            this.groupBoxAutoText.TabIndex = 3;
            this.groupBoxAutoText.TabStop = false;
            this.groupBoxAutoText.Text = "Auto Text";
            this.linkLabelMoreOptions.AutoSize = true;
            this.linkLabelMoreOptions.Location = new Point(8, 0x4a);
            this.linkLabelMoreOptions.Name = "linkLabelMoreOptions";
            this.linkLabelMoreOptions.Size = new Size(70, 13);
            this.linkLabelMoreOptions.TabIndex = 4;
            this.linkLabelMoreOptions.TabStop = true;
            this.linkLabelMoreOptions.Text = "More Options";
            this.linkLabelMoreOptions.VisitedLinkColor = Color.Blue;
            this.linkLabelMoreOptions.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkLabelMoreOptions_LinkClicked);
            this.buttonCreateAutoText.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.buttonCreateAutoText.Location = new Point(0x289, 0x31);
            this.buttonCreateAutoText.Name = "buttonCreateAutoText";
            this.buttonCreateAutoText.Size = new Size(0x4b, 0x17);
            this.buttonCreateAutoText.TabIndex = 3;
            this.buttonCreateAutoText.Text = "Create";
            this.buttonCreateAutoText.UseVisualStyleBackColor = true;
            this.buttonCreateAutoText.Click += new EventHandler(this.buttonCreateAutoText_Click);
            this.textBoxAutoText.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Top;
            this.textBoxAutoText.Location = new Point(11, 0x33);
            this.textBoxAutoText.Name = "textBoxAutoText";
            this.textBoxAutoText.Size = new Size(0x26e, 20);
            this.textBoxAutoText.TabIndex = 2;
            this.textBoxAutoText.Enter += new EventHandler(this.textBoxAutoText_Enter);
            this.textBoxAutoText.Leave += new EventHandler(this.textBoxAutoText_Leave);
            this.labelFontName.AutoSize = true;
            this.labelFontName.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
            this.labelFontName.Location = new Point(0x5c, 0x1b);
            this.labelFontName.Name = "labelFontName";
            this.labelFontName.Size = new Size(0x79, 13);
            this.labelFontName.TabIndex = 1;
            this.labelFontName.Text = "Microsoft Sans Serif";
            this.buttonFont.Location = new Point(11, 0x16);
            this.buttonFont.Name = "buttonFont";
            this.buttonFont.Size = new Size(0x4b, 0x17);
            this.buttonFont.TabIndex = 0;
            this.buttonFont.Text = "Font";
            this.buttonFont.UseVisualStyleBackColor = true;
            this.buttonFont.Click += new EventHandler(this.buttonFont_Click);
            this.buttonOrganize.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.buttonOrganize.Location = new Point(0x29f, 5);
            this.buttonOrganize.Name = "buttonOrganize";
            this.buttonOrganize.Size = new Size(0x5f, 0x17);
            this.buttonOrganize.TabIndex = 14;
            this.buttonOrganize.Text = "Organize frames";
            this.buttonOrganize.UseVisualStyleBackColor = true;
            this.buttonOrganize.Click += new EventHandler(this.buttonOrganize_Click);
            this.labelLength.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.labelLength.AutoSize = true;
            this.labelLength.Location = new Point(0x2b6, 0x33);
            this.labelLength.Name = "labelLength";
            this.labelLength.Size = new Size(0x2b, 13);
            this.labelLength.TabIndex = 7;
            this.labelLength.Text = "0:0.000";
            this.label1.Anchor = AnchorStyles.Right | AnchorStyles.Top;
            this.label1.AutoSize = true;
            this.label1.Location = new Point(0x248, 0x33);
            this.label1.Name = "label1";
            this.label1.Size = new Size(0x68, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Total running length:";
            this.groupBoxDisplay.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.groupBoxDisplay.Controls.Add(this.panel1);
            this.groupBoxDisplay.Controls.Add(this.buttonNext);
            this.groupBoxDisplay.Controls.Add(this.labelPosition);
            this.groupBoxDisplay.Controls.Add(this.buttonPrevious);
            this.groupBoxDisplay.Controls.Add(this.labelCurrentFrame);
            this.groupBoxDisplay.Controls.Add(this.buttonRemoveAll);
            this.groupBoxDisplay.Controls.Add(this.buttonRemove);
            this.groupBoxDisplay.Controls.Add(this.trackBarFrame);
            this.groupBoxDisplay.Location = new Point(0x13, 0x43);
            this.groupBoxDisplay.Name = "groupBoxDisplay";
            this.groupBoxDisplay.Size = new Size(730, 0xce);
            this.groupBoxDisplay.TabIndex = 1;
            this.groupBoxDisplay.TabStop = false;
            this.panel1.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.pictureBoxPreview);
            this.panel1.Location = new Point(5, 0x2c);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(720, 80);
            this.panel1.TabIndex = 0x11;
            this.pictureBoxPreview.Location = new Point(0, 0);
            this.pictureBoxPreview.Name = "pictureBoxPreview";
            this.pictureBoxPreview.Size = new Size(720, 80);
            this.pictureBoxPreview.TabIndex = 0;
            this.pictureBoxPreview.TabStop = false;
            this.pictureBoxPreview.MouseDown += new MouseEventHandler(this.pictureBoxPreview_MouseDown);
            this.pictureBoxPreview.MouseMove += new MouseEventHandler(this.pictureBoxPreview_MouseMove);
            this.pictureBoxPreview.Paint += new PaintEventHandler(this.pictureBoxPreview_Paint);
            this.pictureBoxPreview.MouseUp += new MouseEventHandler(this.pictureBoxPreview_MouseUp);
            this.buttonNext.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.buttonNext.Location = new Point(40, 0xb1);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new Size(0x17, 0x17);
            this.buttonNext.TabIndex = 3;
            this.buttonNext.Text = ">";
            this.toolTip.SetToolTip(this.buttonNext, "Next frame");
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new EventHandler(this.buttonNext_Click);
            this.labelPosition.AutoSize = true;
            this.labelPosition.Location = new Point(6, 0x10);
            this.labelPosition.Name = "labelPosition";
            this.labelPosition.Size = new Size(0x19, 13);
            this.labelPosition.TabIndex = 0x10;
            this.labelPosition.Text = "0, 0";
            this.buttonPrevious.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            this.buttonPrevious.Location = new Point(11, 0xb1);
            this.buttonPrevious.Name = "buttonPrevious";
            this.buttonPrevious.Size = new Size(0x17, 0x17);
            this.buttonPrevious.TabIndex = 2;
            this.buttonPrevious.Text = "<";
            this.toolTip.SetToolTip(this.buttonPrevious, "Previous frame");
            this.buttonPrevious.UseVisualStyleBackColor = true;
            this.buttonPrevious.Click += new EventHandler(this.buttonPrevious_Click);
            this.labelCurrentFrame.AutoSize = true;
            this.labelCurrentFrame.ForeColor = Color.Blue;
            this.labelCurrentFrame.Location = new Point(8, 0);
            this.labelCurrentFrame.Name = "labelCurrentFrame";
            this.labelCurrentFrame.Size = new Size(0x42, 13);
            this.labelCurrentFrame.TabIndex = 0;
            this.labelCurrentFrame.Text = "Frame 0 of 0";
            this.buttonRemoveAll.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonRemoveAll.Location = new Point(0x289, 0xb1);
            this.buttonRemoveAll.Name = "buttonRemoveAll";
            this.buttonRemoveAll.Size = new Size(0x4b, 0x17);
            this.buttonRemoveAll.TabIndex = 5;
            this.buttonRemoveAll.Text = "Remove all";
            this.buttonRemoveAll.UseVisualStyleBackColor = true;
            this.buttonRemoveAll.Click += new EventHandler(this.buttonRemoveAll_Click);
            this.buttonRemove.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            this.buttonRemove.Location = new Point(0x238, 0xb1);
            this.buttonRemove.Name = "buttonRemove";
            this.buttonRemove.Size = new Size(0x4b, 0x17);
            this.buttonRemove.TabIndex = 4;
            this.buttonRemove.Text = "Remove";
            this.buttonRemove.UseVisualStyleBackColor = true;
            this.buttonRemove.Click += new EventHandler(this.buttonRemove_Click);
            this.trackBarFrame.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom;
            this.trackBarFrame.Location = new Point(6, 130);
            this.trackBarFrame.Maximum = 1;
            this.trackBarFrame.Minimum = 1;
            this.trackBarFrame.Name = "trackBarFrame";
            this.trackBarFrame.Size = new Size(0x2ce, 0x2d);
            this.trackBarFrame.TabIndex = 1;
            this.trackBarFrame.TickStyle = TickStyle.TopLeft;
            this.trackBarFrame.Value = 1;
            this.trackBarFrame.Scroll += new EventHandler(this.trackBarFrame_Scroll);
            this.groupBoxPreviewControl.Controls.Add(this.pictureBoxStop);
            this.groupBoxPreviewControl.Controls.Add(this.pictureBoxPlayFromBeginning);
            this.groupBoxPreviewControl.Controls.Add(this.pictureBoxPlay);
            this.groupBoxPreviewControl.Location = new Point(0x150, 5);
            this.groupBoxPreviewControl.Name = "groupBoxPreviewControl";
            this.groupBoxPreviewControl.Size = new Size(0x60, 40);
            this.groupBoxPreviewControl.TabIndex = 0;
            this.groupBoxPreviewControl.TabStop = false;
            this.pictureBoxStop.Image = (Image) manager.GetObject("pictureBoxStop.Image");
            this.pictureBoxStop.Location = new Point(0x42, 12);
            this.pictureBoxStop.Name = "pictureBoxStop";
            this.pictureBoxStop.Size = new Size(0x18, 0x18);
            this.pictureBoxStop.TabIndex = 2;
            this.pictureBoxStop.TabStop = false;
            this.toolTip.SetToolTip(this.pictureBoxStop, "Stop");
            this.pictureBoxStop.Click += new EventHandler(this.pictureBoxStop_Click);
            this.pictureBoxPlayFromBeginning.Image = (Image) manager.GetObject("pictureBoxPlayFromBeginning.Image");
            this.pictureBoxPlayFromBeginning.Location = new Point(0x24, 12);
            this.pictureBoxPlayFromBeginning.Name = "pictureBoxPlayFromBeginning";
            this.pictureBoxPlayFromBeginning.Size = new Size(0x18, 0x18);
            this.pictureBoxPlayFromBeginning.TabIndex = 1;
            this.pictureBoxPlayFromBeginning.TabStop = false;
            this.toolTip.SetToolTip(this.pictureBoxPlayFromBeginning, "Start at the first frame");
            this.pictureBoxPlayFromBeginning.Click += new EventHandler(this.pictureBoxPlayFromBeginning_Click);
            this.pictureBoxPlay.Image = (Image) manager.GetObject("pictureBoxPlay.Image");
            this.pictureBoxPlay.Location = new Point(6, 12);
            this.pictureBoxPlay.Name = "pictureBoxPlay";
            this.pictureBoxPlay.Size = new Size(0x18, 0x18);
            this.pictureBoxPlay.TabIndex = 0;
            this.pictureBoxPlay.TabStop = false;
            this.toolTip.SetToolTip(this.pictureBoxPlay, "Start at the current frame");
            this.pictureBoxPlay.Click += new EventHandler(this.pictureBoxPlay_Click);
            this.toolTip.UseAnimation = false;
            this.toolTip.UseFading = false;
            this.fontDialog.AllowSimulations = false;
            this.fontDialog.Font = new Font("Microsoft Sans Serif", 10f);
            this.fontDialog.FontMustExist = true;
            this.fontDialog.MaxSize = 10;
            this.fontDialog.MinSize = 10;
            this.fontDialog.ShowEffects = false;
            this.saveFileDialog.Title = "Save As";
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.ClientSize = new Size(0x318, 0x22f);
            base.Controls.Add(this.panel);
            base.Controls.Add(this.menuStrip);
            base.KeyPreview = true;
            base.MainMenuStrip = this.menuStrip;
            this.MinimumSize = new Size(800, 0x251);
            base.Name = "LedTriks";
            base.StartPosition = FormStartPosition.CenterScreen;
            base.ResizeBegin += new EventHandler(this.LedTriks_ResizeBegin);
            base.Resize += new EventHandler(this.LedTriks_Resize);
            base.KeyPress += new KeyPressEventHandler(this.LedTriks_KeyPress);
            base.FormClosing += new FormClosingEventHandler(this.LedTriks_FormClosing);
            base.ResizeEnd += new EventHandler(this.LedTriks_ResizeEnd);
            base.Load += new EventHandler(this.LedTriks_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            this.groupBoxCurrentFrame.ResumeLayout(false);
            this.groupBoxCurrentFrame.PerformLayout();
            ((ISupportInitialize) this.pictureBoxShiftUp).EndInit();
            ((ISupportInitialize) this.pictureBoxShiftDown).EndInit();
            ((ISupportInitialize) this.pictureBoxShiftLeft).EndInit();
            ((ISupportInitialize) this.pictureBoxShiftRight).EndInit();
            this.groupBoxAutoText.ResumeLayout(false);
            this.groupBoxAutoText.PerformLayout();
            this.groupBoxDisplay.ResumeLayout(false);
            this.groupBoxDisplay.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((ISupportInitialize) this.pictureBoxPreview).EndInit();
            this.trackBarFrame.EndInit();
            this.groupBoxPreviewControl.ResumeLayout(false);
            ((ISupportInitialize) this.pictureBoxStop).EndInit();
            ((ISupportInitialize) this.pictureBoxPlayFromBeginning).EndInit();
            ((ISupportInitialize) this.pictureBoxPlay).EndInit();
            base.ResumeLayout(false);
            base.PerformLayout();
        }

        private void LedTriks_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Stop();
            if ((e.CloseReason == CloseReason.UserClosing) && base.IsDirty)
            {
                switch (MessageBox.Show("Save changes?", "LedTriks", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                {
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        return;

                    case DialogResult.Yes:
                        ((ISystem) Interfaces.Available["ISystem"]).InvokeSave(this);
                        break;
                }
            }
            this.m_executionInterface.ReleaseContext(this.m_executionContextHandle);
        }

        private void LedTriks_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\x001b')
            {
                e.Handled = true;
                if (this.m_pasting)
                {
                    this.m_pasting = false;
                    this.pictureBoxPreview.Refresh();
                }
            }
        }

        private void LedTriks_Load(object sender, EventArgs e)
        {
            this.SetFont(this.Font);
        }

        private void LedTriks_Resize(object sender, EventArgs e)
        {
            if (!this.m_resizing)
            {
                this.groupBoxPreviewControl.Left = (this.groupBoxPreviewControl.Parent.Width - this.groupBoxPreviewControl.Width) / 2;
                this.checkBoxLoop.Left = (this.checkBoxLoop.Parent.Width - this.checkBoxLoop.Width) / 2;
                this.SetBoardLayout(this.m_boardLayout.Width, this.m_boardLayout.Height);
            }
        }

        private void LedTriks_ResizeBegin(object sender, EventArgs e)
        {
            this.m_resizing = true;
        }

        private void LedTriks_ResizeEnd(object sender, EventArgs e)
        {
            this.m_resizing = false;
        }

        private void linkLabelMoreOptions_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AutoTextOptionsDialog dialog = new AutoTextOptionsDialog(this.m_generator);
            dialog.ShowDialog();
            dialog.Dispose();
            base.AcceptButton = this.buttonCreateAutoText;
        }

        private void LoadFrames()
        {
            XmlNode node = this.m_dataNode.SelectSingleNode("Boards");
            this.SetBoardLayout(int.Parse(node.Attributes["width"].Value), int.Parse(node.Attributes["height"].Value));
            this.SetCellSize(int.Parse(this.m_dataNode.SelectSingleNode("CellSize").InnerText));
            foreach (XmlNode node2 in this.m_dataNode.SelectNodes("Frames/Frame"))
            {
                this.m_frames.Add(new Frame(node2));
            }
        }

        public override EventSequence New()
        {
            this.m_sequence = new EventSequence(this.m_preferences);
            this.m_sequence.Profile = null;
            this.m_sequence.ChannelCount = 0;
            this.m_sequence.Time = 0;
            this.Text = "<Unnamed sequence>";
            this.m_dataNode = this.m_sequence.Extensions[this.FileExtension];
            this.SetBoardLayout(1, 1);
            this.SetCellSize(5);
            this.NewFrame();
            this.Init();
            return this.m_sequence;
        }

        public override EventSequence New(EventSequence seedSequence)
        {
            return null;
        }

        private void NewFrame()
        {
            this.m_frames.Add(new Frame(this.m_defaultFrameLength));
            this.SetFrame(this.m_frames.Count - 1);
            base.IsDirty = true;
        }

        public override void Notify(Notification notification, object data)
        {
        }

        public override EventSequence Open(string filePath)
        {
            this.m_sequence = new EventSequence(filePath);
            this.Text = this.m_sequence.Name;
            this.m_dataNode = this.m_sequence.Extensions[this.FileExtension];
            this.LoadFrames();
            this.SetFrame(0);
            this.Init();
            return this.m_sequence;
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.m_pasting = true;
            this.pictureBoxPreview.Refresh();
        }

        private void pictureBoxPlay_Click(object sender, EventArgs e)
        {
            this.PlayFrom(this.m_currentFrameIndex);
        }

        private void pictureBoxPlayFromBeginning_Click(object sender, EventArgs e)
        {
            this.PlayFrom(0);
        }

        private void pictureBoxPreview_MouseDown(object sender, MouseEventArgs e)
        {
            this.m_mouseDown |= e.Button;
            uint position = (uint) ((this.m_mouseAt.X << 0x10) | this.m_mouseAt.Y);
            if ((this.m_mouseDown & MouseButtons.Left) != MouseButtons.None)
            {
                if (!this.m_pasting)
                {
                    this.SetAt(position);
                    this.pictureBoxPreview.Refresh();
                }
                else
                {
                    this.m_pasting = false;
                    foreach (uint num3 in this.m_clipboard)
                    {
                        int num4 = this.m_mouseAt.X - ((this.pictureBoxPreview.Width / 2) / this.m_cellSize);
                        int num5 = this.m_mouseAt.Y - ((this.pictureBoxPreview.Height / 2) / this.m_cellSize);
                        uint num2 = (uint) (((num3 & 0xffff0000) + (num4 << 0x10)) | ((num3 & 0xffff) + ((short) num5)));
                        this.m_currentFrameCells[num2] = num2;
                    }
                    this.pictureBoxPreview.Refresh();
                }
            }
            else if ((this.m_mouseDown & MouseButtons.Right) != MouseButtons.None)
            {
                this.ResetAt(position);
                this.pictureBoxPreview.Refresh();
            }
        }

        private void pictureBoxPreview_MouseMove(object sender, MouseEventArgs e)
        {
            int num = e.X / this.m_cellSize;
            int num2 = e.Y / this.m_cellSize;
            if ((num != this.m_previousPosition.X) || (num2 != this.m_previousPosition.Y))
            {
                if ((this.m_boardLayout.Width == 1) && (this.m_boardLayout.Height == 1))
                {
                    this.labelPosition.Text = string.Format("{0}, {1}", num % 0x30, num2);
                }
                else
                {
                    int num3 = (num / 0x30) + 1;
                    int num4 = num2 / 0x10;
                    this.labelPosition.Text = string.Format("{0}, {1}\n{2}, {3} in board {4}", new object[] { num, num2, num % 0x30, num2, (num4 * this.m_boardLayout.Width) + num3 });
                }
                this.m_mouseAt.X = num;
                this.m_mouseAt.Y = num2;
                uint position = (uint) ((this.m_mouseAt.X << 0x10) | this.m_mouseAt.Y);
                if ((this.m_mouseDown & MouseButtons.Left) != MouseButtons.None)
                {
                    this.SetAt(position);
                }
                else if ((this.m_mouseDown & MouseButtons.Right) != MouseButtons.None)
                {
                    this.ResetAt(position);
                }
                Rectangle a = new Rectangle(this.m_previousPosition.X * this.m_cellSize, this.m_previousPosition.Y * this.m_cellSize, this.m_cellSize, this.m_cellSize);
                Rectangle b = new Rectangle(num * this.m_cellSize, num2 * this.m_cellSize, this.m_cellSize, this.m_cellSize);
                this.m_previousPosition.X = num;
                this.m_previousPosition.Y = num2;
                if (this.m_pasting)
                {
                    this.pictureBoxPreview.Refresh();
                }
                else
                {
                    this.pictureBoxPreview.Invalidate(Rectangle.Union(a, b));
                }
            }
        }

        private void pictureBoxPreview_MouseUp(object sender, MouseEventArgs e)
        {
            this.m_mouseDown &= ~e.Button;
        }

        private void pictureBoxPreview_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.Black, this.pictureBoxPreview.ClientRectangle);
            if ((this.m_currentFrameIndex > 0) && this.ghostThePreviousFrameToolStripMenuItem.Checked)
            {
                foreach (uint num in this.m_frames[this.m_currentFrameIndex - 1].Cells)
                {
                    e.Graphics.FillRectangle(Brushes.DarkRed, (float) ((num >> 0x10) * this.m_cellSize), (float) ((num & 0xffff) * this.m_cellSize), (float) this.m_cellSize, (float) this.m_cellSize);
                }
            }
            foreach (uint num in this.m_currentFrameCells.Keys)
            {
                e.Graphics.FillRectangle(Brushes.Red, (float) ((num >> 0x10) * this.m_cellSize), (float) ((num & 0xffff) * this.m_cellSize), (float) this.m_cellSize, (float) this.m_cellSize);
            }
            if (!this.m_pasting)
            {
                e.Graphics.FillRectangle(Brushes.Gray, this.m_mouseAt.X * this.m_cellSize, this.m_mouseAt.Y * this.m_cellSize, this.m_cellSize, this.m_cellSize);
            }
            else
            {
                int num2 = this.m_mouseAt.X - ((this.pictureBoxPreview.Width / 2) / this.m_cellSize);
                int num3 = this.m_mouseAt.Y - ((this.pictureBoxPreview.Height / 2) / this.m_cellSize);
                foreach (uint num in this.m_clipboard)
                {
                    e.Graphics.FillRectangle(Brushes.DarkMagenta, (float) ((num2 + (num >> 0x10)) * this.m_cellSize), (float) ((num3 + (num & 0xffff)) * this.m_cellSize), (float) this.m_cellSize, (float) this.m_cellSize);
                }
            }
        }

        private void pictureBoxShiftDown_Click(object sender, EventArgs e)
        {
            this.ShiftDown();
        }

        private void pictureBoxShiftDown_DoubleClick(object sender, EventArgs e)
        {
            this.ShiftDown();
        }

        private void pictureBoxShiftLeft_Click(object sender, EventArgs e)
        {
            this.ShiftLeft();
        }

        private void pictureBoxShiftLeft_DoubleClick(object sender, EventArgs e)
        {
            this.ShiftLeft();
        }

        private void pictureBoxShiftRight_Click(object sender, EventArgs e)
        {
            this.ShiftRight();
        }

        private void pictureBoxShiftRight_DoubleClick(object sender, EventArgs e)
        {
            this.ShiftRight();
        }

        private void pictureBoxShiftUp_Click(object sender, EventArgs e)
        {
            this.ShiftUp();
        }

        private void pictureBoxShiftUp_DoubleClick(object sender, EventArgs e)
        {
            this.ShiftUp();
        }

        private void pictureBoxStop_Click(object sender, EventArgs e)
        {
            this.Stop();
        }

        private void PlayFrom(int frameIndex)
        {
            MethodInvoker method = null;
            this.Cursor = Cursors.WaitCursor;
            if (this.m_execute)
            {
                this.Save();
                this.m_playThread = null;
                if (this.m_executionInterface.EngineStatus(this.m_executionContextHandle) == 1)
                {
                    return;
                }
                int millisecondStart = 0;
                int num3 = 0;
                while (num3 < frameIndex)
                {
                    millisecondStart += this.m_frames[num3].Length;
                    num3++;
                }
                int millisecondCount = millisecondStart;
                while (num3 < this.m_frames.Count)
                {
                    millisecondCount += this.m_frames[num3].Length;
                    num3++;
                }
                this.m_executionInterface.ExecutePlay(this.m_executionContextHandle, millisecondStart, millisecondCount);
            }
            else
            {
                if (this.m_playing || (this.m_frames.Count == 0))
                {
                    return;
                }
                if (base.InvokeRequired)
                {
                    if (method == null)
                    {
                        method = delegate {
                            this.m_ghostPreviousState = this.ghostThePreviousFrameToolStripMenuItem.Checked;
                            this.ghostThePreviousFrameToolStripMenuItem.Checked = false;
                        };
                    }
                    base.Invoke(method);
                }
                else
                {
                    this.m_ghostPreviousState = this.ghostThePreviousFrameToolStripMenuItem.Checked;
                    this.ghostThePreviousFrameToolStripMenuItem.Checked = false;
                }
                this.m_playStartFrame = frameIndex;
                this.m_playThread = new Thread(new ThreadStart(this.PlayThread));
                this.m_playThread.Start();
            }
            this.Cursor = Cursors.Default;
        }

        private void PlayThread()
        {
            int playStartFrame = this.m_playStartFrame;
            int num2 = 0;
            this.SetUIState(false);
            this.m_playing = true;
            this.m_stopwatch.Reset();
            this.m_stopwatch.Start();
            while (this.m_playing && (playStartFrame < this.m_frames.Count))
            {
                num2 += this.m_frames[playStartFrame].Length;
                this.m_setFrame(playStartFrame);
                while (this.m_stopwatch.ElapsedMilliseconds < num2)
                {
                    if (!this.m_playing)
                    {
                        break;
                    }
                }
                playStartFrame++;
            }
            if (this.m_playing)
            {
                this.Stop();
                if (this.checkBoxLoop.Checked)
                {
                    this.PlayFrom(this.m_playStartFrame);
                }
                else
                {
                    this.SetUIState(true);
                }
            }
            else
            {
                this.Stop();
                this.SetUIState(true);
            }
        }

        private void previewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.m_execute = false;
            this.previewToolStripMenuItem.Checked = true;
            this.executeToolStripMenuItem.Checked = false;
        }

        private void RadioSelect(ToolStripMenuItem[] menuItems, string selectedMenuItemText)
        {
            ToolStripMenuItem item = null;
            foreach (ToolStripMenuItem item2 in menuItems)
            {
                item2.Checked = false;
                if (item2.Text == selectedMenuItemText)
                {
                    item = item2;
                }
            }
            item.Checked = true;
        }

        private void ResetAt(uint position)
        {
            if (this.m_currentFrameCells.ContainsKey(position))
            {
                this.m_currentFrameCells.Remove(position);
                base.IsDirty = true;
            }
        }

        private void ResetAutoText()
        {
            this.m_generator.TextScrollDirection = ScrollDirection.Left;
            this.m_generator.TextScrollExtent = ScrollExtent.OnAndOff;
        }

        public override DialogResult RunWizard(ref EventSequence resultSequence)
        {
            return DialogResult.None;
        }

        private void Save()
        {
            XmlNode nodeAlways = Xml.GetNodeAlways(this.m_dataNode, "Boards");
            Xml.SetAttribute(nodeAlways, "width", this.m_boardLayout.Width.ToString());
            Xml.SetAttribute(nodeAlways, "height", this.m_boardLayout.Height.ToString());
            Xml.SetValue(this.m_dataNode, "CellSize", this.m_cellSize.ToString());
            XmlNode emptyNodeAlways = Xml.GetEmptyNodeAlways(this.m_dataNode, "Frames");
            foreach (Frame frame in this.m_frames)
            {
                frame.SaveToXml(emptyNodeAlways);
            }
            this.m_sequence.Time = this.TotalMilliseconds();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void SaveFrame()
        {
            Frame frame = this.m_frames[this.m_currentFrameIndex];
            try
            {
                frame.Length = Convert.ToInt32(this.textBoxFrameLength.Text);
            }
            catch
            {
            }
            frame.Cells.Clear();
            frame.Cells.AddRange(this.m_currentFrameCells.Keys);
            this.UpdateTotalLength();
            base.IsDirty = true;
        }

        public override void SaveTo(string filePath)
        {
            this.Save();
            this.m_sequence.SaveTo(filePath);
            base.IsDirty = false;
            this.Text = this.m_sequence.Name;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void SetAt(uint position)
        {
            if (!this.m_currentFrameCells.ContainsKey(position))
            {
                this.m_currentFrameCells[position] = position;
                base.IsDirty = true;
            }
        }

        private void SetBoardLayout(int width, int height)
        {
            this.m_boardLayout.Width = width;
            this.m_boardLayout.Height = height;
            this.pictureBoxPreview.Width = (this.m_cellSize * 0x30) * this.m_boardLayout.Width;
            this.pictureBoxPreview.Height = (this.m_cellSize * 0x10) * this.m_boardLayout.Height;
            if (this.pictureBoxPreview.Width > this.pictureBoxPreview.Parent.Width)
            {
                this.pictureBoxPreview.Left = 0;
            }
            else
            {
                this.pictureBoxPreview.Left = (this.pictureBoxPreview.Parent.Width - this.pictureBoxPreview.Width) / 2;
            }
            if (this.pictureBoxPreview.Height > this.pictureBoxPreview.Parent.Height)
            {
                this.pictureBoxPreview.Top = 0;
            }
            else
            {
                this.pictureBoxPreview.Top = (this.pictureBoxPreview.Parent.Height - this.pictureBoxPreview.Height) / 2;
            }
            this.pictureBoxPreview.Refresh();
            this.m_generator.BoardPixelHeight = height * 0x10;
            this.m_generator.BoardPixelWidth = width * 0x30;
        }

        private void SetCellSize(int cellSize)
        {
            this.m_cellSize = cellSize;
            this.SetBoardLayout(this.m_boardLayout.Width, this.m_boardLayout.Height);
            ToolStripMenuItem[] menuItems = new ToolStripMenuItem[] { this.toolStripMenuItem10, this.toolStripMenuItem11, this.toolStripMenuItem12, this.toolStripMenuItem13 };
            this.RadioSelect(menuItems, cellSize.ToString());
        }

        private void SetDefaultFrameLength(int milliseconds)
        {
            string str = milliseconds.ToString();
            this.m_defaultFrameLength = milliseconds;
            this.textBoxFrameLength.Text = str;
            this.toolStripTextBoxFrameLength.Text = str;
            this.m_generator.FrameLength = milliseconds;
        }

        private void SetFont(Font font)
        {
            if (font.Style != FontStyle.Regular)
            {
                this.labelFontName.Text = string.Format("{0}, {1}", font.FontFamily.Name, font.Style);
            }
            else
            {
                this.labelFontName.Text = font.FontFamily.Name;
            }
            this.m_generator.TextFont = font;
        }

        private void SetFrame(int index)
        {
            MethodInvoker method = null;
            if (base.InvokeRequired)
            {
                if (method == null)
                {
                    method = delegate {
                        this.SetFrame(index);
                    };
                }
                base.BeginInvoke(method);
            }
            else
            {
                this.trackBarFrame.Maximum = this.m_frames.Count;
                this.UpdateTotalLength();
                Frame frame = this.m_frames[index];
                this.textBoxFrameLength.Text = frame.Length.ToString();
                this.m_currentFrameCells.Clear();
                foreach (uint num in frame.Cells)
                {
                    this.m_currentFrameCells[num] = num;
                }
                int count = this.m_frames.Count;
                this.labelCurrentFrame.Text = string.Format("Frame {0} of {1}", index + 1, count);
                this.m_currentFrameIndex = index;
                this.trackBarFrame.Value = index + 1;
                this.pictureBoxPreview.Refresh();
            }
        }

        private void SetUIState(bool enabled)
        {
            MethodInvoker method = null;
            if (!base.IsDisposed && !base.Disposing)
            {
                if (base.InvokeRequired)
                {
                    if (method == null)
                    {
                        method = delegate {
                            this.buttonOrganize.Enabled = enabled;
                            this.groupBoxDisplay.Enabled = enabled;
                            this.groupBoxCurrentFrame.Enabled = enabled;
                            this.groupBoxAutoText.Enabled = enabled;
                        };
                    }
                    base.Invoke(method);
                }
                else
                {
                    this.buttonOrganize.Enabled = enabled;
                    this.groupBoxDisplay.Enabled = enabled;
                    this.groupBoxCurrentFrame.Enabled = enabled;
                    this.groupBoxAutoText.Enabled = enabled;
                }
            }
        }

        private void ShiftDown()
        {
            List<uint> list = new List<uint>(this.m_currentFrameCells.Keys);
            this.m_currentFrameCells.Clear();
            foreach (uint num2 in list)
            {
                uint num = (num2 & 0xffff0000) | ((ushort) ((num2 & 0xffff) + 1));
                this.m_currentFrameCells[num] = num;
            }
            base.IsDirty = true;
            this.pictureBoxPreview.Refresh();
        }

        private void ShiftLeft()
        {
            List<uint> list = new List<uint>(this.m_currentFrameCells.Keys);
            this.m_currentFrameCells.Clear();
            foreach (uint num2 in list)
            {
                uint num = num2 - 0x10000;
                this.m_currentFrameCells[num] = num;
            }
            base.IsDirty = true;
            this.pictureBoxPreview.Refresh();
        }

        private void ShiftRight()
        {
            List<uint> list = new List<uint>(this.m_currentFrameCells.Keys);
            this.m_currentFrameCells.Clear();
            foreach (uint num2 in list)
            {
                uint num = num2 + 0x10000;
                this.m_currentFrameCells[num] = num;
            }
            base.IsDirty = true;
            this.pictureBoxPreview.Refresh();
        }

        private void ShiftUp()
        {
            List<uint> list = new List<uint>(this.m_currentFrameCells.Keys);
            this.m_currentFrameCells.Clear();
            foreach (uint num2 in list)
            {
                uint num = (num2 & 0xffff0000) | ((ushort) ((num2 & 0xffff) - 1));
                this.m_currentFrameCells[num] = num;
            }
            base.IsDirty = true;
            this.pictureBoxPreview.Refresh();
        }

        private void Stop()
        {
            MethodInvoker method = null;
            if (this.m_execute)
            {
                if (this.m_executionInterface.EngineStatus(this.m_executionContextHandle) != 0)
                {
                    this.m_executionInterface.ExecuteStop(this.m_executionContextHandle);
                }
            }
            else if (this.m_playing)
            {
                this.m_playing = false;
                this.m_stopwatch.Stop();
                if (base.InvokeRequired)
                {
                    if (method == null)
                    {
                        method = delegate {
                            this.ghostThePreviousFrameToolStripMenuItem.Checked = this.m_ghostPreviousState;
                        };
                    }
                    base.Invoke(method);
                }
                else
                {
                    this.ghostThePreviousFrameToolStripMenuItem.Checked = this.m_ghostPreviousState;
                }
            }
        }

        private void textBoxAutoText_Enter(object sender, EventArgs e)
        {
            base.AcceptButton = this.buttonCreateAutoText;
        }

        private void textBoxAutoText_Leave(object sender, EventArgs e)
        {
            base.AcceptButton = null;
        }

        private void toolStripMenuItem10_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem) sender;
            this.SetCellSize(int.Parse(item.Text));
            base.IsDirty = true;
        }

        private void toolStripTextBoxFrameLength_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                e.Handled = true;
                try
                {
                    this.m_defaultFrameLength = Convert.ToInt32(this.toolStripTextBoxFrameLength.Text);
                    this.m_generator.FrameLength = this.m_defaultFrameLength;
                    base.IsDirty = true;
                }
                catch
                {
                }
            }
        }

        private int TotalMilliseconds()
        {
            int num = 0;
            foreach (Frame frame in this.m_frames)
            {
                num += frame.Length;
            }
            return num;
        }

        private void trackBarFrame_Scroll(object sender, EventArgs e)
        {
            this.SetFrame(this.trackBarFrame.Value - 1);
        }

        private void UpdateTotalLength()
        {
            int num = this.TotalMilliseconds();
            this.labelLength.Text = string.Format("{0}:{1:d2}.{2:d3}", num / 0xea60, (num % 0xea60) / 0x3e8, num % 0x3e8);
        }

        public override string Author
        {
            get
            {
                return "K.C. Oaks";
            }
        }

        public override string Description
        {
            get
            {
                return "LedTriks sequence editor";
            }
        }

        public override string FileExtension
        {
            get
            {
                return ".led";
            }
        }

        public override string FileTypeDescription
        {
            get
            {
                return "LedTriks sequence";
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

        private delegate void SetFrameDelegate(int index);
    }
}

